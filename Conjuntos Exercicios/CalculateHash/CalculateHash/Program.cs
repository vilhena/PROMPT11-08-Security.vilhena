using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CalculateHash
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("filename->");
                var file = Console.ReadLine();
                Console.Write("HashAlg->");
                var hashAlg = Console.ReadLine();

                using (var fileStream = new FileStream(file, FileMode.Open))
                {
                    if (hashAlg != null)
                    {
                        using (var hashAlgorithm = HashAlgorithm.Create(hashAlg))
                        {
                            if (hashAlgorithm == null)
                            {
                                Console.WriteLine("invalid algoritm");
                            }
                            else
                            {
                                var hash = hashAlgorithm.ComputeHash(fileStream);
                                Console.WriteLine(BitConverter.ToString(hash));
                            }
                        }
                    }
                }
                Console.ReadLine();
                Console.Clear(); 
            }
        }
    }
}
