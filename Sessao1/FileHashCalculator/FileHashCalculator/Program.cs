using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FileHashCalculator
{
    class Program
    {
        static void Main(string[] args)
        {

            var hash = Hash("EULA.doc");
            Console.WriteLine(CheckHash(hash,"EULA.doc"));
            Console.ReadKey();
        }

        private static string Hash(string file)
        {
            using (var fileStream = new FileStream(file, FileMode.Open))
            using (var cryptoServiceProvider = MD5.Create())
            using (var cryptoStream = new CryptoStream(fileStream, cryptoServiceProvider, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static bool CheckHash(string hash, string file)
        {
            return hash == Hash(file);
        }
    }
}
