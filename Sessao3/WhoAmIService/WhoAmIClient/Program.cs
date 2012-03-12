using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WhoAmIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WhoAmIWS.WhoAmIClient();

            client.ClientCredentials.UserName.UserName = "Alice";
            client.ClientCredentials.UserName.Password = "changeit";
            client.ClientCredentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            var result =
                client.Get();

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
