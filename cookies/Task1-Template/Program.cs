using System;
using System.Threading.Tasks;

namespace SSE
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var server = new HttpServer();
                await server.Start("127.0.0.1", 3000);

                Console.WriteLine("Listening on http://127.0.0.1:3000");
                Console.WriteLine("Stop server with CTRL+C");
            }).Wait();
        }
    }
}