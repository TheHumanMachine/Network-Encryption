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
        private byte[] Key = null;
        private byte[] Seed = null;
        private SymmetricAlgorithm symetricAlgo;

        public SymmetricAlgorithm SymetricAlgo
        {
            set { symetricAlgo = value; }
        }
        public void TextFileEncrypt(string data, string saveLocation, string seed)
        {
            byte[] encodedData = Encoding.Unicode.GetBytes(data);
            using (FileStream fileStrm = new FileStream(saveLocation, FileMode.Create, FileAccess.Write))
            {
                GenerateSecretInitVector();
                GenKey();
                //genSeed;
                using (ICryptoTransform transform = symetricAlgo.CreateEncryptor(Key, Seed))
                {
                    using (CryptoStream cryptoStrm = new CryptoStream(fileStrm, transform, CryptoStreamMode.Write))
                    {
                        cryptoStrm.Write(encodedData, 0, encodedData.Length);
                    }
                }
            }
        }
        private void GenKey()
        {
            if (null != (symetricAlgo as TripleDESCryptoServiceProvider))
            {
                TripleDESCryptoServiceProvider tdes;
                tdes = symetricAlgo as TripleDESCryptoServiceProvider;
                tdes.KeySize = 192; // Maximum key size
                tdes.GenerateKey();
                Key = tdes.Key;
            }
            else if (null != (symetricAlgo as RijndaelManaged))
            {
                RijndaelManaged rdProvider;
                rdProvider = symetricAlgo as RijndaelManaged;
                rdProvider.KeySize = 256; // Maximum key size
                rdProvider.GenerateKey();
                Key = rdProvider.Key;
            }
        }
        private void GenerateSecretInitVector()
        {
            if (null != (symetricAlgo as TripleDESCryptoServiceProvider))
            {
                TripleDESCryptoServiceProvider tdes;
                tdes = symetricAlgo as TripleDESCryptoServiceProvider;
                tdes.GenerateIV();
                Seed = tdes.IV;
            }
            else if (null != (symetricAlgo as RijndaelManaged))
            {
                RijndaelManaged rdProvider;
                rdProvider = symetricAlgo as RijndaelManaged;
                rdProvider.GenerateIV();
                Seed = rdProvider.IV;
            }
        }
        public string decryptFile(String path)
        {
            string decrypted = "";
            // Create file stream to read encrypted file back.
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // Print out the contents of the encrypted file.
                using (BinaryReader binReader = new BinaryReader(fileStream))
                {
                    int count = Convert.ToInt32(binReader.BaseStream.Length);

                    byte[] bytes = binReader.ReadBytes(count);
                    char[] array = Encoding.Unicode.GetChars(bytes);
                    string encdata = new string(array);
                }
                // Reset the file stream.
                fileStream.Seek(0, SeekOrigin.Begin);
                // Create decryptor.
                using (ICryptoTransform transform = symetricAlgo.CreateDecryptor(Key,Seed))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream,
                    transform,
                    CryptoStreamMode.Read))
                    {
                        // Print out the contents of the decrypted file.
                        StreamReader srDecrypted = new StreamReader(cryptoStream,
                        new UnicodeEncoding());
                        Console.WriteLine("---------- Decrypted Data ---------");
                        decrypted = srDecrypted.ReadToEnd();
                        Console.WriteLine("---------- Decrypted Data ---------");
                    }
                }
            }
            return decrypted;
        }
    }
}
