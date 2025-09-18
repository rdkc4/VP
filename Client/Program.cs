using Client.Services.Data;
using Common.Samples;
using Common.Services.Data;
using Common.Services.Session;
using System;
using System.Collections.Generic;
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
            IDataAssembler dataAssembler = null;

            try
            {
                channelFactory = new ChannelFactory<IServiceContract>("ServiceContract");
                service = channelFactory.CreateChannel();

                const string filePath = @"./../../../Dataset/drone_dataset.csv";
                dataReader = new DataReader(filePath);

                string header = dataReader.ReadSample();
                dataAssembler = new DataAssembler(header);

                service.StartSession(dataAssembler.GetMeta());

                int row = 0;
                double previousTime = 0d;
                string line;

                while ((line = dataReader.ReadSample()) != null && row++ < 100)
                {
                    DroneSample droneSample = dataAssembler.AssembleDroneSample(line, out double time);
                    if (droneSample != null)
                    {
                        service.PushSample(droneSample);
                        Thread.Sleep((int)((time - previousTime) * 1000));
                        previousTime = time;
                    }
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
