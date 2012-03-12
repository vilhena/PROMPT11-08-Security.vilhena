using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace ProtectFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtectFile("EULA.doc","EULA.doc.protected","changeit");
            UnprotectFile("EULA.doc.protected", "EULA.recovered.doc", "changeit");
        }

        private static void UnprotectFile(string filename, string fileOutName, string password)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open))
            using (var md5Hash = MD5.Create())
            using (var cryptoServiceProvider = TripleDES.Create())
            {
                var asciiEncoding = new ASCIIEncoding();

                cryptoServiceProvider.Key = md5Hash.ComputeHash(asciiEncoding.GetBytes(password));

                using (var cs = new CryptoStream(fileStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Read))
                using (var fileWriter = new FileStream(fileOutName, FileMode.CreateNew))
                {
                    cs.CopyTo(fileWriter);
                }
            }
        }

        public static void ProtectFile(string filename, string fileOutName, string password)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open))
            using (var md5Hash = MD5.Create())
            using (var cryptoServiceProvider = TripleDES.Create())
            {
                var asciiEncoding = new ASCIIEncoding();

                cryptoServiceProvider.Key = md5Hash.ComputeHash(asciiEncoding.GetBytes(password));

                using (var cs = new CryptoStream(fileStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Read))
                using (var fileWriter = new FileStream(fileOutName, FileMode.CreateNew))
                {
                    cs.CopyTo(fileWriter);
                }
            }
        }
    }
}
