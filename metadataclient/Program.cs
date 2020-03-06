using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using Client.ServiceReference1;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Invoking secured Web Service...");

            Service1Client client = new Service1Client();
            client.ClientCredentials.UserName.UserName = "wcfaddservice";
            client.ClientCredentials.UserName.Password = "wcfpassword";
            
            client.ClientCredentials.ServiceCertificate.SetDefaultCertificate(
System.Security.Cryptography.X509Certificates
.StoreLocation.CurrentUser,
                    System.Security.Cryptography.X509Certificates.StoreName.My,
    System.Security.Cryptography.X509Certificates.X509FindType
.FindBySubjectName,
                    "WcfAddService");
            
            Console.WriteLine(client.Add(1, 2));

            Console.WriteLine("Ready...");
            Console.ReadKey();
            
        }
    }
}
