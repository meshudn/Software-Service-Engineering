using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SSE
{
    public class HttpFileServer : TcpServer
    {
        // Take the folder DocumentRoot within the project
        private static readonly string DOCUMENT_ROOT = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\..\\..\\..\\DocumentRoot";

        /// <summary>
        /// Buffer for HTTP message.
        /// </summary>
        protected string buffer = "";

        /// <summary>
        /// Handles an incoming line of text. 
        /// </summary>
        /// <param name="line">Incoming data.</param>
        /// <returns>The answer to be sent back as a reaction to the received line or null.</returns>
        protected override string ReceiveLine(string line)
        {
            buffer += line + "\r\n";
            if (line == "")
            {
                // parse message
                HttpMessage request = new HttpMessage(buffer);

                // build answer message
                HttpMessage answer = ReceiveRequest(request);

                // send answer message
                Console.WriteLine("HTTP: Sending answer.");
                return answer.ToString();
            }
            else
                return null;
        }

        /// <summary>
        /// Handle an incoming HTTP request.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <returns>The answer message to be sent back.</returns>
        protected virtual HttpMessage ReceiveRequest(HttpMessage request)
        {
            // check HTTP method
            // TODO

            // look for requested file
            // TODO

            // create answer message                        
            // TODO:
            HttpMessage answerMessage = new HttpMessage("TODO");

            return answerMessage;
        }
    }
}
