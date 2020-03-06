using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace SSE
{
    public class SoapClient
    {
        private SoapClient()
        {
        }

        public static async Task<object> Invoke(string url, string ns, string operationName, Dictionary<String,object> parameters)
        {
            
            string message = "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"><soap12:Body>\n";
            
            // TODO: Complete message body

            message += "</soap12:Body></soap12:Envelope>";

            // TODO Send HTTP request
            
            // TODO Parse response 
            return "";
        }
    }
}