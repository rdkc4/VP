using Client.Services.Client;
using Common.Services.Session;

namespace Client
{
    internal class Program
    {
        static void Main()
        {
            IClientService client = new ClientService();
            client.Run();
        }
    }
}
