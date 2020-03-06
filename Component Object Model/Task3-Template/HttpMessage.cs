using System;
using System.Collections.Generic;
using System.Linq;

namespace SSE
{
    public class HttpMessage
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
        public const string HEAD = "HEAD";
        public const string OPTIONS = "OPTIONS";
        public const string TRACE = "TRACE";

        public string Method = "";
        public string Host = "";
        public string Resource = "";
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
        public string Content = "";
        public string StatusCode = "";
        public string StatusMessage = "";

        /// <summary>
        /// Construct an HTTP request.
        /// </summary>
        public HttpMessage(string method, string host, string resource, Dictionary<string, string> headers, string content)
        {
            this.Method = method;
            this.Host = host;
            this.Resource = resource;
            this.Headers = headers;
            if (Headers == null)
                Headers = new Dictionary<string, string>();
            this.Content = content;
            StatusCode = null;
            StatusMessage = null;
        }

        /// <summary>
        /// Construct an HTTP response.
        /// </summary>        
        public HttpMessage(string statusCode, string statusMessage, Dictionary<string, string> headers, string content)
        {
            this.Method = null;
            this.Host = null;
            this.Resource = null;
            this.Headers = headers;
            if (Headers == null)
                Headers = new Dictionary<string, string>();
            this.Content = content;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }

        /// <summary>
        /// Constructs an HTTP message by parsing a (received) string.
        /// </summary>
        /// <param name="message"></param>
        public HttpMessage(string message)
        {
            // TODO: parse HTTP request/response message
        }

        /// <summary>
        /// Returns the string representation of the message.
        /// </summary>        
        public override string ToString()
        {
            // TODO: construct HTTP request/response message

            return "";
        }
    }
}
