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
using System.Runtime.InteropServices;
using Common.Events;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Services.Client;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IClientService client = new ClientService();
            client.Run();
        }
    }
}
