using System;
using System.Collections.Generic;
using Vsr.Teaching.Pvs.Client;
using Vsr.Teaching.Pvs.Server;
using Vsr.Teaching.Pvs.Web;

namespace Broker
{
    class Broker: TcpServer
    {
        private string buffer;
        private List<Tuple<string, int>> clientEndpoints = new List<Tuple<string, int>>()
                                                   {
                                                       new Tuple<string, int>("127.0.0.1",15001), 
                                                       new Tuple<string, int>("127.0.0.1",15002)
                                                   };
        
        protected override string ReceiveMessage(string messagePlain, string endpoint)
        {
            // Somulate interrupt
            int rand = new Random().Next(0, 2);
            if (rand != 0)
            {
                // TODO: Construct SOAP message with appropriate headers
                string busyAcknowledgement = "";
                return new HttpMessage( "200", "OK", new Dictionary<String, String>(), busyAcknowledgement ).ToString();
            }

            Console.WriteLine( "Broker: recieved server update: \n" + messagePlain.Replace("\n","") + "\n");

            messagePlain = messagePlain.Replace( "<n:StatusRequest xmlns:n=\"http://example.org/status\" env:mustUnderstand=\"true\"/>","");
            HttpMessage message = new HttpMessage(messagePlain);
            // Forward message to clientEndpoints             
            foreach (var clientEndpoint in clientEndpoints)
            {
                TcpClient client = new TcpClient();
                client.Request(clientEndpoint.Item1, clientEndpoint.Item2, message.Content);
            }          


            // TODO: Construct SOAP message with appropriate headers
            string recievedAcknowledgement = "";


            return new HttpMessage("200","OK",new Dictionary<String,String>(),recievedAcknowledgement).ToString();
        }

        static void Main(string[] args)
        {
            Broker server = new Broker();
            Console.WriteLine("Broker started...");
            server.Run("127.0.0.1", 14000);            
        }
    }
}
