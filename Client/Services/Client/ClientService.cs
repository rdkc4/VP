using Common.Exceptions;
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
        private readonly string datasetPath = ConfigurationManager.AppSettings["DatasetPath"] ?? "./../../../Dataset/drone_dataset.csv";
        private readonly string logDirectoryPath = ConfigurationManager.AppSettings["LogsDirectoryPath"] ?? "./../../Logs";
        private readonly string logFileName = ConfigurationManager.AppSettings["DroneLogsFileName"] ?? "drone_logs.txt";
        private readonly string eventFileName = ConfigurationManager.AppSettings["EventLogsFileName"] ?? "event_logs.txt";
        private readonly string leftoverFileName = ConfigurationManager.AppSettings["LeftoverLogsFileName"] ?? "leftover_logs.txt";
        private readonly int rowLimit;

        private DuplexChannelFactory<IDroneService> channelFactory;
        private IDroneService service;
        private IDataReader dataReader;
        private ILogger logger;
        private IDataAssembler dataAssembler;

        private bool initialized = false;
        private bool disposed = false;
        private bool serverActive = false;

        private double previousTime = 0d;

        public ClientService()
        {
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
                if (!initialized)
                {
                    Init();
                }

                StartSession();
                SendData();
                EndSession();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client Error]: {ex.Message}");
                logger?.LogEvent(_event: "Client Error", message: ex.Message);
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
                    Console.WriteLine("[Server Connection]: The server connection has faulted");
                    logger?.LogEvent(_event: "Server Connection", message: "The server connection has faulted");
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

            initialized = true;
        }

        private void StartSession()
        {
            var result = service.StartSession(dataAssembler.GetMeta());
            if (!result.Success)
            {
                throw new ServerUnavailableException(result.Message);
            }
        }

        private void SendData()
        {
            int row = 0;
            string line;

            while ((line = dataReader.ReadSample()) != null && row < rowLimit)
            {
                DroneSample droneSample = dataAssembler.AssembleDroneSample(line, out double time);
                if (droneSample != null)
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
                        Console.WriteLine("[Communication Error]: Server disconnected");
                        logger?.LogEvent(_event: "Communication Error", message: "Server disconnected");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"[Invalid Drone Sample]: Row {row + 1} was skipped");
                    logger?.Log(_event: "Invalid Drone Sample", message: $"Row {row + 1} was skipped");
                }
                ++row;
            }
        }

        private void EndSession()
        {
            if (serverActive)
            {
                service?.EndSession();
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
            Console.WriteLine("[Client Service]: Disposing...");
            if (disposed) return;

            if (disposing)
            {
                dataReader?.Dispose();
                logger?.Dispose();
            }

            disposed = true;
        }
    }
}
