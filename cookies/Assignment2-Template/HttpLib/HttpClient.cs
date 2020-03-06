// Course Material "Protokolle Verteilte Systeme"
// (c) 2008 by Professur Verteilte und Selbstorganisierende Rechnersysteme, TUC

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using Vsr.Teaching.Pvs.Web;

namespace Vsr.Teaching.Pvs.Client
{
    /// <summary>
    /// Simple implementation of an HTTP client.
    /// </summary>
    public class HttpClient : TcpClient
    {     
        /// <summary>
        /// Performs an HTTP operation.
        /// </summary>
        /// <param name="operation">The operation (e.g. "GET")</param>
        /// <param name="urlStr">The URL to perform the operation on.</param>
        /// <param name="content">The content passed within the request (if any).</param>
        /// <param name="headerParameters">Additional HTTP parameters to be included inside the header.</param>        
        /// <returns>The HTTP response message.</returns>   
        public HttpMessage Request(string operation, string urlStr, string content, Dictionary<string,string> headerParameters)
        {
            // parse url
            Url url = new Url(urlStr);
            if (url.Scheme != "http")
                throw new Exception("The protocol scheme in " + urlStr + " is not supported. Only HTTP is supported.");
                        
            // determine IP address
            string ip = System.Net.Dns.GetHostAddresses(url.Host)[0].ToString();
            if (url.Host.ToLower().Equals("localhost"))
                ip = "127.0.0.1";

            // contruct HTTP message
            HttpMessage requestMessage = new HttpMessage(operation, url.Host, url.Path + url.Query, headerParameters, content);

            // send query via TCP
            string resultMessageStr = Request(ip, url.Port, requestMessage.ToString());

            // parse HTTP message
            HttpMessage resultMessage = new HttpMessage(resultMessageStr);

            return resultMessage;
        }        

        /// <summary>
        /// Performs an HTTP GET.
        /// </summary>
        /// <param name="url">The URL of the resource</param>
        /// <returns></returns>
        public HttpMessage Get(string urlStr)
        {
            return Request(HttpMessage.GET, urlStr, "", null);            
        }

        /// <summary>
        /// Performs an HTTP POST.
        /// </summary>
        /// <param name="url">The POST of the resource</param>
        /// <returns></returns>
        public HttpMessage Post(string urlStr, string contentType, string content)
        {
            Dictionary<string, string> para = new Dictionary<string, string>();
            para["content-type"] = contentType;
            return Request(HttpMessage.POST, urlStr, content, para);
        }

        /// <summary>
        /// Performs an HTTP DELETE.
        /// </summary>
        /// <param name="url">The URL of the resource</param>
        /// <returns></returns>
        public HttpMessage Delete(string urlStr)
        {            
            return Request(HttpMessage.DELETE, urlStr, "", null);
        }
    }
}
