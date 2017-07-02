using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Build: 0.2.0
 * Date: 6/30/17
 * Code Metrics:
 * Network Encryption:  76  47  1   15  110
 * Unit Tests: 61   28   1   5   268
 */

namespace Networking_Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            Encryption e = new Encryption();
            while (true)
            {
                var a = e.Encrypt("this is a test");
                var b = a.Length;
            }
        }
    }
}
