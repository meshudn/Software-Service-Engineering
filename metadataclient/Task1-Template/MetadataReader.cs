using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SSE
{
    public class MetadataReader
    {
        private readonly string _endpoint;

        private MetadataReader(string endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task PrintMetadataSections()
        {
            string content = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"">
                <s:Header>
                    <a:Action s:mustUnderstand=""1"">http://schemas.xmlsoap.org/ws/2004/09/transfer/Get</a:Action>
                    <a:MessageID>urn:uuid:82b9e527-3271-4cbf-876c-87ff11c4225b</a:MessageID>
                    <a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo>
                    <a:To s:mustUnderstand=""1"">http://pauline.informatik.tu-chemnitz.de/WcfAddService/Service1.svc/mex</a:To>
                </s:Header>
                <s:Body/>
            </s:Envelope>";

            // TODO: Get and Parse Response

            // TODO: Print Metadata   
        }

        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                MetadataReader client = new MetadataReader(@"http://pauline.informatik.tu-chemnitz.de/WcfAddService/Service1.svc/mex");

                await client.PrintMetadataSections();
                Console.ReadLine();
            }).Wait();
        }
    }
}
