using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Networking_Encryption
{
    class DesEncryption
    {
        private byte[] key = null;
        private byte[] seed = null;
        private SymmetricAlgorithm symetricAlgo;
        /// <summary>
        /// sets the algorithm for the DES encryptor
        /// </summary>
        public SymmetricAlgorithm SymetricAlgo
        {
            set { symetricAlgo = value; }
        }
        /// <summary>
        /// DES key property
        /// </summary>
        public byte[] Key
        {
            set { key = value; }
            get { return key; }
        }
        /// <summary>
        /// DES seed property
        /// </summary>
        public byte[] Seed
        {
            set { seed = value; }
            get { return seed; }
        }
        /// <summary>
        /// generates a key and Seed from the given seed if the seed != "" else generates a random key and Seed
        /// </summary>
        /// <param name="seed"></param>
        private void genKeys(string seed = "")
        {
            genSeed();
            GenKey();
        }
        /// <summary>
        /// encrypts given data to a save location
        /// </summary>
        /// <param name="data">information to encrypt</param>
        /// <param name="saveLocation">path to save the data</param>
        /// <param name="seed">place to start the algo from</param>
        public void TextFileEncrypt(string data, string saveLocation, string seed = "")
        {
            byte[] encodedData = Encoding.Unicode.GetBytes(data);
            using (FileStream fileStrm = new FileStream(saveLocation, FileMode.Create, FileAccess.Write))
            {
                genKeys(seed);
                using (ICryptoTransform transform = symetricAlgo.CreateEncryptor(key, this.seed))
                {
                    using (CryptoStream cryptoStrm = new CryptoStream(fileStrm, transform, CryptoStreamMode.Write))
                    {
                        cryptoStrm.Write(encodedData, 0, encodedData.Length);
                    }
                }
            }
        }
        /// <summary>
        /// generates a new for DES
        /// </summary>
        private void GenKey()
        {
            if (null != (symetricAlgo as TripleDESCryptoServiceProvider))
            {
                TripleDESCryptoServiceProvider tdes;
                tdes = symetricAlgo as TripleDESCryptoServiceProvider;
                tdes.KeySize = 192; // Maximum key size
                tdes.GenerateKey();
                key = tdes.Key;
            }
            else if (null != (symetricAlgo as RijndaelManaged))
            {
                RijndaelManaged rdProvider;
                rdProvider = symetricAlgo as RijndaelManaged;
                rdProvider.KeySize = 256; // Maximum key size
                rdProvider.GenerateKey();
                key = rdProvider.Key;
            }
        }
        /// <summary>
        /// generates a new seed for DES
        /// </summary>
        private void genSeed()
        {
            if (null != (symetricAlgo as TripleDESCryptoServiceProvider))
            {
                TripleDESCryptoServiceProvider tdes;
                tdes = symetricAlgo as TripleDESCryptoServiceProvider;
                tdes.GenerateIV();
                seed = tdes.IV;
            }
            else if (null != (symetricAlgo as RijndaelManaged))
            {
                RijndaelManaged rdProvider;
                rdProvider = symetricAlgo as RijndaelManaged;
                rdProvider.GenerateIV();
                seed = rdProvider.IV;
            }
        }
        /// <summary>
        /// functions decrypts a given path
        /// <para> returns a decrypted string</para>
        /// </summary>
        /// <param name="path">name of the file</param>
        /// <returns>returns a decrypted string</returns>
        public string decryptFile(String path)
        {
            string decrypted = "";
            // Create file stream to read encrypted file back.
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                using (ICryptoTransform transform = symetricAlgo.CreateDecryptor(key,seed))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader strmDec = new StreamReader(cryptoStream, new UnicodeEncoding()))
                        {
                            decrypted = strmDec.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }
    }
}
