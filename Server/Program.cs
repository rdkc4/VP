using Server.Services.Session;
using System;
using System.ServiceModel;

namespace Server
{
    internal class Program
    {
        static void Main()
        {
            ServiceHost host = new ServiceHost(typeof(DroneService));
            host.Open();

            Console.WriteLine("Service is running. Press any key to stop...");
            Console.ReadKey();
            host.Close();
        }
    }
}
