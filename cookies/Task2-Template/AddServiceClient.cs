using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SSE
{
    public class AddServiceClient
    {
        private readonly string _serviceLocation;
        
        public AddServiceClient(string serviceLcoation)
        {
            _serviceLocation = serviceLcoation;
        }

        /// <summary>
        /// Sends a SOAP request via HTTP to a Web service endpoint.
        /// </summary>
        public async Task<int> Add(int a, int b)
        {
            // TODO: create and send SOAP message
            // HttpMessage answer = await HttpRequest.Post(_serviceLocation, content, headers);

            return 0;
        }
    }
}