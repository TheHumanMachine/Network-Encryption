using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    public class HuffmanTree
    {
        private class BinaryNode
        {
            string elemt;
            int freqnt;
            BinaryNode left;
            BinaryNode right;

            private BinaryNode(ref string element, int frequency, BinaryNode lft = null, BinaryNode rght = null)
            {
                elemt = element;
                freqnt = frequency;
                left = lft;
                right = rght;
            }
        }
        BinaryNode root = null;
        /// <summary>
        /// Transform the original string into an encoded string :: Return encodedString
        /// </summary>
        /// <param name="inputText"> Text inputed by User </param>
        public string Encode(string inputText)
        { 
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert back an encoded string into its original text :: Return decodedString
        /// </summary>
        /// <param name="encodedString"> Enconded String</param>
        public void Decode(string encodedString)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Calculate Frequency for each letter in the input string
        /// </summary>
        /// <returns></returns>
        private int CalcFreqnt()
        {
            throw new NotImplementedException();

        }
        public void SaveToString()
        {
            throw new NotImplementedException();
        }
        public int size(int a)
        {
            throw new NotImplementedException();
        }
    }
}
