using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Build: 0.2.0
 * Date: 6/30/17
 * Code Metrics:
 * Network Encryption: 72  44   1   16   84
 * Unit Tests: 62   28   1   5   268
 */

namespace Networking_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            string strToEncrypt = "this is a test";
            Encryption encryptor = new Encryption();
            int count = 0;
            int keyLen = 0;
            int seedLen = 0;
            while (count < 8)
            {
                strToEncrypt = encryptor.Encrypt(strToEncrypt);
                keyLen += encryptor.Key.ToString().Length;
                seedLen += encryptor.IV.ToString().Length;
                count++;
            }
            Console.WriteLine("avg len of key: {0} \t avg len of seed: {1}",(keyLen / 3),(seedLen / 3));
            Console.ReadLine();
        }
    }
}
