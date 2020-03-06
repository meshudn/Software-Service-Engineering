// Course Material "Protokolle Verteilte Systeme"
// (c) 2008 by Professur Verteilte und Selbstorganisierende Rechnersysteme, TUC

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vsr.Teaching.Pvs.Web
{
    /// <summary>
    /// A class for generating and parsing HTTP 1.1 messages.
    /// </summary>
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
        /// Constructs an HTTP message by parsing a (received) string.
        /// </summary>
        /// <param name="message"></param>
        public HttpMessage(string message)
        {
            bool firstTime = true;

            // loop through lines in message
            while (message.Contains("\n"))
            {
                // separate first line from rest of messge
                string line = message.Substring(0, message.IndexOf("\n")).Trim();
                message = message.Substring(message.IndexOf("\n") + 1);

                if (firstTime)
                {
                    // parse first line 
                    if (line.StartsWith("HTTP"))
                    {
                        // response message                        
                        Match match = Regex.Match(line, @"^HTTP\/1\.1\s(\S*)\s(.*)$");
                        // extract important Headers
                        if (match.Groups.Count == 3)
                        {
                            StatusCode = match.Groups[1].Value.Trim();
                            StatusMessage = match.Groups[2].Value.Trim();
                        }
                        else
                            throw new FormatException("Could not parse HTTP message, the first line is misformed: " + line);
                    }
                    else
                    {
                        // request message
                        Match match = Regex.Match(line, @"^(\S*)\s(\S*)\sHTTP\/1\.1$");
                        // extract important Headers
                        if (match.Groups.Count == 3)
                        {
                            Method = match.Groups[1].Value.Trim();
                            Resource = match.Groups[2].Value.Trim();
                        }
                        else
                            throw new FormatException("Could not parse HTTP message, the first line is misformed: " + line);
                    }
                }
                else if (line.Equals(""))
                {
                    // empty line -> end of header; the rest is content
                    Content = message;
                    message = "";
                }
                else
                {
                    // parse name-value-pairs
                    int pos = line.IndexOf(':');
                    if (pos == -1)
                        throw new FormatException("Could not parse HTTP message, invalid header line: " + line);
                    string name = line.Substring(0, pos).ToLower().Trim();
                    string value = line.Substring(pos + 1).Trim();

                    if (name.Equals("host"))
                        // store host parameter in extra variable
                        Host = value;
                    else
                        // store all other Headers
                        Headers[name] = value;

                }
                firstTime = false;
            }

            // handle chunked encoding
            if (Headers.ContainsKey("transfer-encoding"))
            {
                string encoding = Headers["transfer-encoding"];
                int length;
                if ((encoding != null) && encoding.Contains("chunked"))
                {
                    string chunked = Content;
                    Content = "";
                    do
                    {
                        string lengthStr = chunked.Substring(0, chunked.IndexOf("\n")).Trim();
                        length = (System.Int32.Parse(lengthStr, System.Globalization.NumberStyles.AllowHexSpecifier));
                        Content += chunked.Substring(chunked.IndexOf("\n") + 1, length);
                        chunked = chunked.Substring(chunked.IndexOf("\n") + 3 + length);
                    } while (length > 0);
                }
            }

        }

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
        /// Returns the string representation of the message.
        /// </summary>        
        public override string ToString()
        {
            // automatically set content length
            Headers["content-length"] = "" + Content.Length;


            // build obligatory part
            string message;
            if (Method != null)
                message = Method + " " + Resource + " HTTP/1.1\nhost: " + Host + "\n";
            else
                message = "HTTP/1.1 " + StatusCode + " " + StatusMessage + "\n";

            // add Headers
            foreach (string name in Headers.Keys)
                message += name + ": " + Headers[name] + "\n";

            // add content
            message += "\n" + Content;

            return message;
        }

        /// <summary>
        /// Sets the value of a cookie inside an HTTP Message.
        /// </summary>        
        public void SetCookie(string name, string value)
        {
            Headers["set-cookie"] = name + "=" + value;
        }

        /// <summary>
        /// Returns a list of cookie parts inside the HTTP message.
        /// </summary>                
        public Dictionary<string, string> GetCookies()
        {
            // access HTTP header parameter
            Dictionary<string, string> cookies = new Dictionary<string, string>();
            if (Headers.ContainsKey("cookie"))
            {
                // split into individual cookie parts
                string[] parts = Headers["cookie"].Split(new char[1] { ';' });
                foreach (string part in parts)
                {
                    // parse name and value
                    int middle = part.IndexOf('=');
                    if (middle != -1)
                    {
                        string name = part.Substring(0, middle).Trim();
                        string value = part.Substring(middle + 1).Trim();
                        cookies[name] = value;
                    }
                }
            }
            return cookies;
        }
    }
}
