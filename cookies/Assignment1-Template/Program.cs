using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var parameters = new Dictionary<string, object>()
                {
                    {"joinSymbol", ":"},
                    {"first", "bla"},
                    {"second", 5},
                    {"third", "?"},
                };
                var result = await SoapClient.Invoke("http://pauline.informatik.tu-chemnitz.de/ConcatenatorService/ConcatenatorService.asmx", "http://tempuri.org/", "Concatenate", parameters);
                
                Console.WriteLine("Concatenator Result: " + result.ToString());

                parameters = new Dictionary<string, object>()
                {
                    {"a", 10},
                    {"b", 15}
                };
                result = await SoapClient.Invoke("http://pauline.informatik.tu-chemnitz.de/SoapWebService/Service.asmx", "http://vsr.informatik.tu-chemnitz.de/edu/2008/pvs/soapwebservice", "Add", parameters);

                Console.WriteLine("Add Result: " + result.ToString());
            }).Wait();
        }
    }
}
