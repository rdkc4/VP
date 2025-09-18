using Client.Services.Data;
using Client.Services.Logger;
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

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IServiceContract> channelFactory = null;
            IServiceContract service = null;
            IDataReader dataReader = null;
            ILogger logger = null;
            IDataAssembler dataAssembler = null;

            try
            {
                channelFactory = new ChannelFactory<IServiceContract>("ServiceContract");
                service = channelFactory.CreateChannel();

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

                int row = 0;
                double previousTime = 0d;
                string line;

                while ((line = dataReader.ReadSample()) != null)
                {
                    DroneSample droneSample = dataAssembler.AssembleDroneSample(line, out double time);
                    if (droneSample != null && row < 100)
                    {
                        service.PushSample(droneSample);
                        Thread.Sleep((int)((time - previousTime) * 1000));
                        previousTime = time;
                    }
                    else if(droneSample != null)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                service?.EndSession();
                channelFactory?.Close();
                dataReader?.Dispose();
            }

        }
    }
}
