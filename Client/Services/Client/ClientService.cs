using Client.Services.Data;
using Client.Services.Log;
using Common.Samples;
using Common.Services.Data;
using Common.Services.Logger;
using Common.Services.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Services.Client
{
    public class ClientService : IClientService
    {
        private DuplexChannelFactory<IDroneService> channelFactory;
        private IDroneService service;
        private IDataReader dataReader;
        private ILogger logger;
        private IDataAssembler dataAssembler;

        private bool disposed = false;
        private bool serverActive = false;

        public void OnTransferStarted(string message)
        {
            Console.WriteLine($"[Data Transfer]: {message}");
            logger?.Log(_event: "Data Transfer", message: message);
        }

        public void OnTransferCompleted(string message)
        {
            Console.WriteLine($"[Data Transfer]: {message}");
            logger?.Log(_event: "Data Transfer", message: message);
        }

        public void OnSampleReceived(string message)
        {
            Console.WriteLine($"[Sample Received]: {message}");
            logger?.Log(_event: "Sample Received", message: message);
        }

        public void OnWarningRaised(string message)
        {
            Console.WriteLine($"[Data Warning]: {message}");
            logger?.Log(_event: "Data Warning", message: message);
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
                logger?.Log(_event: "Client Error", message: ex.Message);
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

            if(service is ICommunicationObject serverChannel)
            {
                serverChannel.Faulted += (sender, e) =>
                {
                    serverActive = false;
                    logger?.Log(_event: "Server Connection", message: "The server connection has faulted");
                };
                serverChannel.Closed += (sender, e) =>
                {
                    serverActive = false;
                    logger?.Log(_event: "Server Connection", message: "Server connection closed");
                };
            }

            const string datasetPath = @"./../../../Dataset/drone_dataset.csv";
            dataReader = new DataReader(datasetPath);

            const string logDirectoryPath = @"./../../Logs";
            const string logFileName = "logs.txt";
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
            logger = new Logger(logDirectoryPath, logFileName);

            string header = dataReader.ReadSample();
            dataAssembler = new DataAssembler(header);

            service.StartSession(dataAssembler.GetMeta());
        }

        private void SendData()
        {
            int row = 0;
            double previousTime = 0d;
            string line;

            while ((line = dataReader.ReadSample()) != null && serverActive)
            {
                DroneSample droneSample = dataAssembler.AssembleDroneSample(line, out double time);
                if (droneSample != null && row < 100)
                {
                    try
                    {
                        service.PushSample(droneSample);
                        Thread.Sleep((int)((time - previousTime) * 1000));
                        previousTime = time;
                    }
                    catch
                    {
                        logger?.Log(_event: "Communication Error", message: "Server disconnected");
                        break;
                    }
                }
                else if (droneSample != null)
                {
                    logger.Log(_event: "Leftover Drone Sample", message: $"{droneSample}");
                    Thread.Sleep((int)((time - previousTime) * 1000));
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
                    logger?.Log("Client", "Server has already disconnected");
                }
                dataReader?.Dispose();
                logger?.Dispose();
            }

            disposed = true;
        }
    }
}
