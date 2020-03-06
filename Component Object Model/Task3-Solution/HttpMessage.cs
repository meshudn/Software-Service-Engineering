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
            // loop through lines in message
            var lines = message.Split('\n');
            var firstLine = lines.First();

            // parse first line
            var parts = firstLine.Split(' ');
            if (parts.Count() != 3)
            {
                throw new FormatException("Malformed HTTP message: " + firstLine);
            }

            if (parts[0] == "HTTP/1.0" || parts[0] == "HTTP/1.1")
            { // is response
                StatusCode = parts[1];
                StatusMessage = parts[2];
            }
            else
            { // is request
                Method = parts[0];
                Resource = parts[1];
            }

            // parse other lines
            for (int i = 1; i < lines.Count(); i++)
            {
                var line = lines[i];
                if (line == "")
                { // empty line -> end of header; the rest is content
                    Content = String.Join("\n", lines.Skip(i + 1));
                    break;
                }
                else
                { // parse header (name-value-pair)
                    var colonAt = line.IndexOf(':');
                    if (colonAt == -1)
                    {
                        throw new FormatException("Malformed header: " + line);
                    }

                    var name = line.Substring(0, colonAt).Trim();
                    var value = line.Substring(colonAt + 1).Trim();

                    if (name == "Host")
                    {
                        Host = value;
                    }
                    else
                    {
                        Headers[name] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the string representation of the message.
        /// </summary>        
        public override string ToString()
        {
            // set content length
            Headers["Content-Length"] = Content.Length.ToString();

            // build first line
            string message;
            if (!String.IsNullOrEmpty(Method))
            {
                message = Method + " " + Resource + " HTTP/1.1\nHost: " + Host + "\n";
            }
            else
            {
                message = "HTTP/1.1 " + StatusCode + " " + StatusMessage + "\n";
            }

            // add Headers
            foreach (string name in Headers.Keys)
            {
                message += name + ": " + Headers[name] + "\n";
            }

            // add content
            message += "\n" + Content;

            return message;
        }
    }
}
