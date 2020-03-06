using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using HttpLib;
using Vsr.Teaching.Pvs.Server;

namespace Client2
{
    class Client2: TcpServer
    {
        protected override string ReceiveMessage( string message, string endpoint )
        {
            XDocument doc = XDocument.Parse( message );

            // TODO: detect encryption based on SOAP headers and ignore the message

            var company = doc.XPathSelectElement( "//*[1]/*[2]/*[1]/*[1]" ).Value;
            var price = doc.XPathSelectElement( "//*[1]/*[2]/*[1]/*[2]" ).Value;
            
            Console.WriteLine( "Client 2: incoming update: {0},{1}",
                company,
                price);
            return "";
        }

        static void Main( string[] args )
        {
            Client2 server = new Client2();
            Console.WriteLine( "Client2 started..." );
            server.Run( "127.0.0.1", 15002 );
        }
    }
}
