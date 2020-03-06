// Course Material "Protokolle Verteilte Systeme"
// (c) 2008 by Professur Verteilte und Selbstorganisierende Rechnersysteme, TUC

using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

// Registration
// ============
// - Start Start Menu -> Visual Studio Tools -> Visual Studio Command Prompt
// - Register component in .NET GAC: gacutil /i ComBrowser.dll
// - Register component in Windows registry and create the type library file:
//   regasm ComBrowser.dll /tlb:ComBrowser.tlb
//   -> the DLL should now be registered under
//      HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Interface\{79C42849-7C99-4290-BD70-40F7E9608647}\TypeLib

namespace Vsr.Teaching.Pvs.Component
{
    [Guid("79C42849-7C99-4290-BD70-40F7E9608647")]    
    [ComVisible(true)]
    public interface IBrower
    {
        string HttpGet(string url);
    }    

    /// <summary>
    /// Implementation of the browser component interface.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("73D9F80E-6BBE-4AC9-AAB2-99F03CED9FA1")]    
    [ComVisible(true)]
    public class Browser : IBrower
    {
        public string HttpGet(string url)
        {
            Client.TcpClient client = new Client.TcpClient();

            Uri urlObj = new Uri(url);
            if (urlObj.Scheme != "http")
                return "URL not supported";

            string ip = System.Net.Dns.GetHostAddresses(urlObj.Host)[0].ToString();
            string answer = client.Request(ip,80,"GET / HTTP/1.1\n\n");                     
            return answer;                        
        }
    }
  
}
