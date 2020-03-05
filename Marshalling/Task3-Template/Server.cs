using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Task3
{
    public class Server
    {
        private readonly Middleware _middleware;

        public Server(Middleware middleware)
        {
            _middleware = middleware;
        }

        private string IncomingConnection(object request)
        {
            // --------------------- Implement ---------------------    
            // TODO: Check if the incoming object is a RPC, invoke the requested method using Reflection
            // --------------------- /Implement ---------------------
            return PrintToConsole(request);
        }

        private string PrintToConsole(object request)
        {
            if (request is Double)
            {
                Console.WriteLine("Server: Printing double: " + Double.Parse(request.ToString()).ToString("0#.##0"));
                return "Double printed";
            }
            else if (request is String)
            {
                Console.WriteLine("Server: Printing string: <<" + request + ">> (length " + request.ToString().Length + ")");
                return "String printed";
            }
            else if (request is String[])
            {
                string output = "Server: Printing string array: [" + string.Join(" - ", (String[])request) + "] (number of elements: " + ((String[])request).Length + ")";
                Console.WriteLine(output);
                return "String array printed";
            }
            throw new ArgumentException("Can not print the object " + request);
        }

        public void Start(string ip, int port)
        {
            _middleware.StartServer(ip, port, IncomingConnection);
        }

        public void Stop()
        {
            _middleware.StopServer();
        }

        public string concat(string arg1, string arg2, string arg3)
        {
            return arg1 + arg2 + arg3;
        }

        public string substring(string s, string startingPosition)
        {
            return s.Substring(Int32.Parse(startingPosition));
        }
    }
}
