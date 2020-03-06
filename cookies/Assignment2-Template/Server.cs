using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpLib;
using Vsr.Teaching.Pvs.Client;
using Vsr.Teaching.Pvs.Server;
using Vsr.Teaching.Pvs.Web;

namespace ConsoleApplication1
{
    class Server: TcpServer
    {
        private const string BROKER_URL = "http://127.0.0.1:14000/";

        protected override string ReceiveMessage(string message, string endpoint)
        {
            Console.WriteLine("Server: incoming message: " + message);
            return "";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the server...");
            Console.ReadKey();
            Server server = new Server();
            server.sendStockUpdate("FB", "35.5");
            server.sendStockUpdate("TW", "30.0", true);
            server.sendStockUpdate("LI", "15.0", true);
            server.Run("127.0.0.1", 13000);
        }


        private void sendStockUpdate(string company, string price,bool encrypted = false)
        {
            HttpClient client = new HttpClient();
            int tries = 0;
            bool clientAcknowledged;
            do
            {
                // TODO: signal encrypted content in SOAP headers
                string content = string.Format(
                    "<env:Envelope xmlns:env=\"http://www.w3.org/2001/09/soap-envelope\">" +
                        "<env:Header>" +
                            "<n:StatusRequest xmlns:n=\"http://example.org/status\" env:mustUnderstand=\"true\"/>" +                            
                       "</env:Header><env:Body>" +
                        createSOAPBody( company, price, encrypted ) + 
                    "</env:Body></env:Envelope>",
                    company, price);
                var response = client.Post(BROKER_URL, "application/soap+xml", content);               
                clientAcknowledged = CheckIfClientAcknowledgedReciept(response.Content);
                if (!clientAcknowledged)
                    Console.WriteLine("Server: broker is busy. repeating transmission...");
                tries++;
            } while (tries < 15 && !clientAcknowledged);
        }

        private string createSOAPBody( string company, string price, bool encrypted )
        {
            string body = 
                          string.Format( "<ex:StockUpdate xmlns:ex=\"http://example.org\">" +
                          "<ex:Company>{0}</ex:Company>" +
                          "<ex:Price>{1}</ex:Price>" +
                          "</ex:StockUpdate>", company , price);

            // TODO: Encrypt message if desired            
            return body;
        }

        private bool CheckIfClientAcknowledgedReciept(string response)
        {
            //TODO: Check SOAP acknowledgement
            return false;
        }


    }
}
