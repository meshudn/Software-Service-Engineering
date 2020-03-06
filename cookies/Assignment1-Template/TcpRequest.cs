using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SSE
{
    public class TcpRequest
    {
        private TcpRequest() {}

        public static async Task<string> Do(IPAddress ip, int port, string message)
        {
            var client = new TcpClient(ip.AddressFamily);
            await client.ConnectAsync(ip, port);

            using (var r = new StreamReader(client.GetStream(), Encoding.ASCII))
            using (var w = new StreamWriter(client.GetStream(), Encoding.ASCII))
            {
                await w.WriteAsync(message);
                w.Flush();

                var buffer = new char[4096];
                var byteCount = await r.ReadAsync(buffer, 0, buffer.Length);
                return new string(buffer, 0, byteCount);
            }
        }
    }
}