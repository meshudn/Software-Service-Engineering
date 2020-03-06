using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSE
{
    public class TcpServer
    {
        private CancellationTokenSource _ts;
        private Task _serverTask;

        protected virtual string HandleRequest(string msg)
        {
            return "";
        }

        public Task Start(string ip, int port)
        {
            _ts = new CancellationTokenSource();
            _serverTask = Start(ip, port, _ts);
            return _serverTask;
        }

        public void Stop()
        {
            _ts.Cancel();
            _serverTask.Wait();
        }
    
        private async Task Start(string ip, int port, CancellationTokenSource ts)
        {
            var server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();

            var ct = ts.Token;
            using (ct.Register(server.Stop))
            {
                while (true)
                {
                    // wait for client connection
                    TcpClient client;
                    try
                    {
                        client = await server.AcceptTcpClientAsync();
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
        
                    using (var r = new StreamReader(client.GetStream(), Encoding.ASCII))
                    using (var w = new StreamWriter(client.GetStream(), Encoding.ASCII))
                    {
                        var buffer = new char[4096];
                        var byteCount = await r.ReadAsync(buffer, 0, buffer.Length);

                        var resp = HandleRequest(new string(buffer, 0, byteCount));

                        await w.WriteAsync(resp);
                        w.Flush();
                    }
                }
            }
        }
    }
}