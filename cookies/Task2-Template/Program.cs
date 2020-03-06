using System;
using System.Threading.Tasks;

namespace SSE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var client = new AddServiceClient("http://vsr-demo.informatik.tu-chemnitz.de/webservices/SoapWebService/Service.asmx");
                var answer = await client.Add(2,3);

                // display result and wait for user
                Console.WriteLine("Result: " + answer.ToString());
            }).Wait();
        }
    }
}
