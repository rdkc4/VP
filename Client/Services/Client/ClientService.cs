using Client.Exceptions;
using Client.Services.Data;
using Client.Services.Log;
using Common.Samples;
using Common.Services.Data;
using Common.Services.Logger;
using Common.Services.Session;
using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace Client.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly string datasetPath;
        private readonly string logDirectoryPath;
        private readonly string logFileName;
        private readonly string eventFileName;
        private readonly string leftoverFileName;
        private readonly int rowLimit;

        private DuplexChannelFactory<IDroneService> channelFactory;
        private IDroneService service;
        private IDataReader dataReader;
        private ILogger logger;
        private IDataAssembler dataAssembler;

        private bool disposed = false;
        private bool serverActive = false;

        public ClientService()
        {
            datasetPath = ConfigurationManager.AppSettings["DatasetPath"] ?? "./../../../Dataset/drone_dataset.csv";
            logDirectoryPath = ConfigurationManager.AppSettings["LogsDirectory"] ?? "./../../Logs";
            logFileName = ConfigurationManager.AppSettings["DroneLogsFileName"] ?? "drone_logs.txt";
            eventFileName = ConfigurationManager.AppSettings["EventLogsFileName"] ?? "event_logs.txt";
            leftoverFileName = ConfigurationManager.AppSettings["LeftoverLogsFileName"] ?? "leftover_logs.txt";

            if (!int.TryParse(ConfigurationManager.AppSettings["RowLimit"], out rowLimit))
            {
                rowLimit = 100;
            }
        }

        public void OnTransferStarted(string message)
        {
            Console.WriteLine($"[Data Transfer]: {message}");
            logger?.LogEvent(_event: "Data Transfer", message: message);
        }

        public void OnTransferCompleted(string message)
        {
            Console.WriteLine($"[Data Transfer]: {message}");
            logger?.LogEvent(_event: "Data Transfer", message: message);
        }

        public void OnSampleReceived(string message)
        {
            Console.WriteLine($"[Sample Received]: {message}");
            logger?.LogEvent(_event: "Sample Received", message: message);
        }

        public void OnWarningRaised(string message)
        {
            Console.WriteLine($"[Data Warning]: {message}");
            logger?.LogEvent(_event: "Data Warning", message: message);
        }

        public void OnAccelerationSpike(string message)
        {
            Console.WriteLine($"[Data Warning]: {message}");
            logger?.LogEvent(_event: "Data Warning", message: message);
        }

        public void OnOutOfBandWarning(string message)
        {
            Console.WriteLine($"[Data Warning]: {message}");
            logger?.LogEvent(_event: "Data Warning", message: message);
        }

        public void OnWindSpike(string message)
        {
            Console.WriteLine($"[Data Warning]: {message}");
            logger?.LogEvent(_event: "Data Warning", message: message);
        }

        public void Run()
        {
            try
            {
                Init();
                SendData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client Error: {ex.Message}");
                logger?.LogEvent(_event: "Client Error", message: ex.Message);
            }
            finally
            {
                Dispose(true);
            }
        }

        private void Init()
        {
            var instanceContext = new InstanceContext(this);
            channelFactory = new DuplexChannelFactory<IDroneService>(instanceContext, "DroneService");
            service = channelFactory.CreateChannel();
            serverActive = true;

            if (service is ICommunicationObject serverChannel)
            {
                serverChannel.Faulted += (sender, e) =>
                {
                    serverActive = false;
                    logger?.LogEvent(_event: "Server Connection", message: "The server connection has faulted");
                };
                serverChannel.Closed += (sender, e) =>
                {
                    serverActive = false;
                    logger?.LogEvent(_event: "Server Connection", message: "Server connection closed");
                };
            }

            dataReader = new DataReader(datasetPath);
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
            logger = new Logger(logDirectoryPath, logFileName, eventFileName, leftoverFileName);

            string header = dataReader.ReadSample();
            dataAssembler = new DataAssembler(header);

            var result = service.StartSession(dataAssembler.GetMeta());
            if (!result.Success)
            {
                throw new ServerUnavailableException(result.Message);
            }
        }

        private void SendData()
        {
            int row = 0;
            double previousTime = 0d;
            string line;

            while ((line = dataReader.ReadSample()) != null && serverActive)
            {
                DroneSample droneSample = dataAssembler.AssembleDroneSample(line, out double time);
                if (droneSample != null && row < rowLimit)
                {
                    try
                    {
                        var result = service.PushSample(droneSample);
                        if (result.Success)
                        {
                            Console.WriteLine($"[Sent Sample]: {droneSample}");
                            logger?.Log(_event: "Sent Sample", message: $"{droneSample}");
                        }
                        else
                        {
                            Console.WriteLine($"[Rejected Sample]: {droneSample}");
                            logger?.LogLeftover(_event: "Rejected Sample", message: result.Message);
                        }

                        int delay = (int)(Math.Max(0d, time - previousTime) * 1000);
                        Thread.Sleep(delay);
                        previousTime = time;
                    }
                    catch
                    {
                        logger?.LogEvent(_event: "Communication Error", message: "Server disconnected");
                        break;
                    }
                }
                else if (droneSample != null)
                {
                    Console.WriteLine($"[Leftover Drone Sample]: {droneSample}");
                    logger.LogLeftover(_event: "Leftover Drone Sample", message: $"{droneSample}");

                    int delay = (int)(Math.Max(0d, time - previousTime) * 1000);
                    Thread.Sleep(delay);
                    previousTime = time;
                }
                else
                {
                    logger.Log(_event: "Invalid Drone Sample", message: $"Row {row + 1} was skipped");
                }
                ++row;
            }
        }

        ~ClientService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                try
                {
                    if (serverActive)
                    {
                        service?.EndSession();
                    }

                    if (channelFactory?.State == CommunicationState.Opened)
                    {
                        channelFactory.Close();
                    }
                    else
                    {
                        channelFactory?.Abort();
                    }
                }
                catch
                {
                    logger?.LogEvent("Client", "Server has already disconnected");
                }
                dataReader?.Dispose();
                logger?.Dispose();
            }

            disposed = true;
        }
    }
}
