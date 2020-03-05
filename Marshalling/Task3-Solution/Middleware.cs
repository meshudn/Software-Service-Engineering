using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    public class Middleware
    {
        private Func<object, string> _serverCallback;
        private Task _serverTask;
        private CancellationTokenSource _ts;

        private object Demarshall(string line)
        {
            // --------------------- Implement ---------------------
            if(line.StartsWith("rpc"))
                return line;
            string[] pair = line.Split( ':' );
            if ( pair[0].Equals( "double" ) )
                return Double.Parse( pair[1] );
            if ( pair[0].Equals( "string" ) )
                return pair[1];
            if ( pair[0].Equals( "string-array" ) )
            {
                return pair[1].Split( ',' );
            }
            throw new ArgumentException( "Can not demarshall the object" + line );
            // --------------------- /Implement -------------------
        }


        private string Marshall(object input)
        {
            // --------------------- Implement ---------------------    
            if ( input is String )
            {
                return "string:" + input;
            }
            if ( input is Double )
            {
                return "double:" + input; 
            }
            if ( input is String[] )
            {
                return "string-array:" + String.Join(",",input as String[]);
            }
            throw new ArgumentException( "  Middleware: Could not marshall the object" );            
            // --------------------- /Implement ---------------------
        }

        public async Task<string> SendObjectTo(string address, object input)
        {
            string ip = address.Split(':')[0];
            int port = int.Parse(address.Split(':')[1]);

            string payload = Marshall(input);

            Console.WriteLine("\n\r  Middleware: Transferring payload '{0}' to {1}:{2}\n\r", payload, address, port);

            string answer = await TcpRequest.Do(ip, port, payload);
            answer = answer.Substring(0, answer.IndexOf("\0")); // deleting all \0 of buffer for printing
            return answer;
        }


        public async Task<string> CallFunction(string address, string name, string[] args)
        {
            // --------------------- Implement ---------------------
            string ip = address.Split(':')[0];
            int port = int.Parse(address.Split(':')[1]);

            string payload = "rpc:" + name + ":" + String.Join(",", args);

            string answer = await TcpRequest.Do(ip, port, payload + "\n");
            answer = answer.Substring(0, answer.IndexOf("\0")); // deleting all \0 of buffer for printing
            return answer;
            // --------------------- /Implement -------------------            
        }

        protected virtual string ProcessIncomingRequest(string line)
        {
            line = line.Substring(0, line.IndexOf("\0")); // deleting \0 of buffer for printing
            Console.WriteLine("\n\r  Middleware: Received payload '{0}'\n\r", line);
            object answer = Demarshall(line);
            return _serverCallback(answer);
        }

        public void StartServer(string ip, int port, Func<object, string> callback)
        {
            _serverCallback = callback;
            _ts = new CancellationTokenSource();
            _serverTask = TcpServer.Start(ip, port, _ts, ProcessIncomingRequest);
        }

        public void StopServer()
        {
            _ts.Cancel();
            _serverTask.Wait();
        }
    }
}
