using System;

namespace SSE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HttpFileServer server = new HttpFileServer();
            server.Run("127.0.0.1", 3000);
            Console.WriteLine("Listening ...");
        }
    }
}
