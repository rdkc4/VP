using System;
using Client.Services.Client;
using Common.Services.Session;

namespace Client
{
    internal class Program
    {
        static void Main()
        {
            IClientService client = new ClientService();

            bool running = true;

            while (running)
            {
                ShowMenu();
                Console.Write("Choose option: ");
                if (!int.TryParse(Console.ReadLine(), out int option) || option > 2 || option < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        client.Run();
                        break;
                    case 2:
                        Console.Clear();
                        break;
                    case 0:
                        client.Dispose();
                        running = false;
                        break;
                    default:
                        continue;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("1. Start Session");
            Console.WriteLine("2. Clear Console");
            Console.WriteLine("0. Quit");
            Console.WriteLine("-----------------");
        }
    }
}
