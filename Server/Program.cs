using Server.Services.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ServiceContract));
            host.Open();

            Console.WriteLine("Service is running. Press any key to stop...");
            Console.ReadKey();
            host.Close();
        }
    }
}
