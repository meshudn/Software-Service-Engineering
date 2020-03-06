using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using HttpLib;
using Vsr.Teaching.Pvs.Server;

namespace Client1
{
    class Client1: TcpServer
    {
        protected override string ReceiveMessage(string message, string endpoint)
        {
            XDocument doc = XDocument.Parse( message );                        
            string company;
            string price;

            // TODO: detect encryption
            bool encrypted = false;
            if(encrypted)
            {
                //TODO: decrypt and extract data
                company = "";
                price = "";
            }
            else
            {
                company = doc.XPathSelectElement( "//*[1]/*[2]/*[1]/*[1]" ).Value;
                price = doc.XPathSelectElement( "//*[1]/*[2]/*[1]/*[2]" ).Value;
            }
            Console.WriteLine( "Client 1: incoming {2}update: {0},{1}", 
                company, 
                price,
                encrypted ? "encrypted " : "");
            return "";
        }

        static void Main(string[] args)
        {
            Client1 server = new Client1();
            Console.WriteLine("Client1 started...");
            server.Run("127.0.0.1", 15001);
        }
    }

}
