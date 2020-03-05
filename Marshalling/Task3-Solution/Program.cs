using System;
using System.Threading.Tasks;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                Server server = new Server( new Middleware() );
                server.Start("127.0.0.1", 13000);

                Client client = new Client( new Middleware() );

                await client.Print("127.0.0.1:13000", 2.25 );
                await client.Print("127.0.0.1:13000", "hello");
                await client.Print("127.0.0.1:13000", new String[] { "a", "b", "c" });

                Console.WriteLine("Performing RPC...");
                Console.WriteLine(await client.concat("a","b","c"));
                Console.WriteLine(await client.substring("hello world", "6"));

                server.Stop();
            }).Wait();
        }
    }
}