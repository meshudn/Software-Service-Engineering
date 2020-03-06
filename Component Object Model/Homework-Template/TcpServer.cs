using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SSE
{
    public class TcpServer
    {
        /// <summary>
        /// The socket used for handling an established connection.
        /// </summary>
        private Socket _connectionSocket;

        /// <summary>
        /// Sends the specified data back via the connectionSocket.
        /// </summary>        
        private void SendString(string message)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(message);
            _connectionSocket.Send(sendBuffer, sendBuffer.Length, SocketFlags.None);
            Console.WriteLine("TCP: Sent answer message, {0} bytes to {1}.", sendBuffer.Length,
                _connectionSocket.RemoteEndPoint);
        }

        /// <summary>
        /// Shuts down the connectionSocket.
        /// </summary>
        private void CloseCurrentConnection()
        {
            Console.WriteLine("TCP: Shutting down connection with {0} in both directions.",
                _connectionSocket.RemoteEndPoint);

            // shut down
            _connectionSocket.Shutdown(SocketShutdown.Both);
        }

        /// <summary>
        /// Waits for bytes arriving on connectionSocket and handles them.
        /// </summary>
        private void ReceiveBytes()
        {
            Console.WriteLine("TCP: Waiting for bytes from {0}.",
                _connectionSocket.RemoteEndPoint);

            var request = "";

            while (true)
            {
                // receive chunk of bytes
                var receiveBuffer = new byte[10000];
                var receivedBytes = _connectionSocket.Receive(receiveBuffer, receiveBuffer.Length, SocketFlags.None);
                Console.WriteLine("TCP: Received {0} bytes from {1}.",
                    receivedBytes, _connectionSocket.RemoteEndPoint);

                // add received bytes to input buffer            
                request += Encoding.ASCII.GetString(receiveBuffer, 0, receivedBytes);

                // see if there are any new lines
                while (request.IndexOf('\n') != -1)
                {
                    // separate line from buffer
                    var line = request.Substring(0, request.IndexOf('\n'));
                    request = request.Substring(request.IndexOf('\n') + 1);

                    // trim all \r at end of line
                    line = line.TrimEnd('\r');

                    // handle line
                    var answer = ReceiveLine(line);

                    // if the line caused the server to generate an answer, send it back
                    if (answer != null)
                    {
                        SendString(answer);
                        CloseCurrentConnection();
                        return;
                    }
                }
            }
        }
        
        /// <summary>
        /// Starts the HTTP server, which keeps on running until the program is aborted.
        /// </summary>
        /// <param name="ip">The IP address to listen tp.</param>
        /// <param name="port">The port to listen to.</param>
        public void Run(string ip, int port)
        {       
            Console.WriteLine("TCP: Starting server.");

            // create listening socket                        
            var listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("TCP: Created listening socket.");

            // bind 
            var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            listeningSocket.Bind(endpoint);
            Console.WriteLine("TCP: Bound to {0}.", endpoint.ToString());

            // listen
            listeningSocket.Listen(10);
            Console.WriteLine("TCP: Start listening.");
            Console.WriteLine();

            // accept loop 
            while (true)
            {
                // accept new connection
                var newSocket = listeningSocket.Accept();
                Console.WriteLine("TCP: Connection established with {0} over socket {1}.",
                                  newSocket.RemoteEndPoint.ToString(), newSocket.LocalEndPoint.ToString());

                // receive bytes and handle them
                _connectionSocket = newSocket;

                // create a new instance of the current server class                
                ReceiveBytes();
            }    
        }
     
        /// <summary>
        /// Handles an incoming line of text. 
        /// </summary>
        /// <param name="line">Incoming data.</param>
        /// <returns>The answer to be sent back as a reaction to the received line or null.</returns>
        protected virtual string ReceiveLine(string line)
        {
             // for testing purposes: wait for 10s and echo back received line 
            Console.WriteLine("TCP: Simulate 10s processing time.");

            // simulate 10s processing time
            var start = DateTime.Now.Ticks;
            while (DateTime.Now.Ticks - start < 100000000) { };

            // example application: provide environment information
            if (line.StartsWith("ask "))
                return Environment.GetEnvironmentVariable(line.Substring(4));
            return "What?";
        }
    }
}
