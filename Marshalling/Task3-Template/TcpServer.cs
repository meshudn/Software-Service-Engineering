using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    public class TcpServer
    {
        private TcpServer() {}
    
        public static async Task Start(string ip, int port, CancellationTokenSource ts, Func<string, string> handler)
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

                        var resp = handler(new string(buffer));

                        await w.WriteAsync(resp);
                        w.Flush();
                    }
                }
            }
        }
    }
}