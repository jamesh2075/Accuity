using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;

namespace AccuityService
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5555;
            if (args.Length > 1)
            {
                int.TryParse(args[0], out port);
            }

            Console.WriteLine($"Starting NetMQ at TCP port {port}.");
            Console.WriteLine("Press CTRL+C to stop the service.");

            try
            {
                using (var server = new ResponseSocket())
                {
                    server.Bind($"tcp://*:{port}");
                    while (true)
                    {
                        var message = server.ReceiveFrameString();
                        Console.WriteLine(message);
                        Thread.Sleep(100);
                        server.SendFrame("***Processed***");
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Console.WriteLine("An unexpected error occurred.");
            }
        }
    }
}
