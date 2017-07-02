using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
/*
 * Build: 0.1.0
 * Date: 6/30/17
 * Code Metrics:
 * Network Encryption: 72   44  1   16  84
 */
namespace Networking_Encryption
{
    public class Encryption
    {
        private static byte[] savedKey = null;
        private static byte[] savedIV = null;

        public byte[] Key
        {
            get { return savedKey; }
            set { savedKey = value; }
        }

        public byte[] IV
        {
            get { return savedIV; }
            set { savedIV = value; }
        }
        /// <summary>
        /// function compares the file extentions of two files
        /// <para/>returns true if they hold the same extention 
        /// </summary>
        /// <param name="fileOne"> file name one </param>
        /// <param name="fileTwo">file name two</param>
        /// <returns> returns true if the two files contain the same extention </returns>
        private bool checkExtention(string fileOne,string fileTwo)
        {
            bool areSame = false;
            if (fileOne.Count() >= 4 && fileTwo.Count() >= 4) // 4 is the minimal len for a valid extention
            {
                if (getExtention(fileOne) == getExtention(fileTwo))
                {
                    areSame = true;
                }
            }
            return areSame;
        }
        /// <summary>
        /// function finds the extention type of a given file name
        /// <para/>returns a string with the extention type of the file
        /// </summary>
        /// <param name="file"> name of the file</param>
        /// <returns> returns a string associated with the file extention type</returns>
        private string getExtention(string file)
        {
            int index = file.Count() - 1;
            string extention = "";
            char token = ' ';
            while (token != '.' && index > 0)
            {
                token = file[index];
                extention = extention.Insert(0, token.ToString());
                index--;
            }
            if (!extention.Contains('.'))
            {
                throw new FormatException("not a valid extention");
            }
            return extention;
        }
        /// <summary>
        /// function checks given string has a valid file extention
        /// <para/>returns true if string has an extention
        /// </summary>
        /// <param name="FileName"> string to check</param>
        /// <returns>returns a bool whether or not file has an extention</returns>
        private bool checkHasExtention(string FileName)
        {
            bool hasExtention = true;
            try
            {
                getExtention(FileName);
            }
            catch (Exception)
            {
                hasExtention = false;
            }
            return hasExtention;
        }
        //begin stringEncryption
        private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
        {
            if (savedKey == null)
            {
                rdProvider.KeySize = 256;
                rdProvider.GenerateKey();
                savedKey = rdProvider.Key;
            }
        }
        private static void RdGenerateInitVector(RijndaelManaged rdProvider)
        {
            if (savedIV == null)
            {
                rdProvider.GenerateKey();
                savedIV = rdProvider.IV;
            }
        }
        private string StringEncrypt(string orignalString)
        {
            // encode data
            byte[] orginalStringAsBytes = Encoding.ASCII.GetBytes(orignalString);
            byte[] orginalBytes = { };

            //create memstream
            using (MemoryStream memStream = new MemoryStream(orginalStringAsBytes.Length))
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    RdGenerateSecretKey(rijndael);
                    RdGenerateInitVector(rijndael);

                    if (savedIV == null || savedKey == null)
                    {
                        throw new NullReferenceException(" one of keys is null");
                    }
                    //create encryptor & streams
                    using (ICryptoTransform rdTransform = rijndael.CreateEncryptor((byte[])savedKey.Clone(), (byte[])savedIV.Clone()))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, CryptoStreamMode.Write))
                        {
                            // write encrypted Data
                            cryptoStream.Write(orginalStringAsBytes, 0, orginalStringAsBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            orginalBytes = memStream.ToArray();
                        }
                    }
                }
            }
            //convert encrypted string
            string encyptedStr = Convert.ToBase64String(orginalBytes);
            return encyptedStr;
        }
        /// <summary>
        /// function encrypts the given  obj
        /// <para/> function returns an encrypted string or a  the name of the file where the file was encrypted
        /// </summary>
        /// <param name="obj"> obj to encrypt</param>
        /// <param name="seed">the specified seed to run the encryption on</param>
        /// <returns>returns an encrypted string ro  the name of the file where the file was encrypted</returns>
        public string Encrypt(string obj,string seed = "")
        {
            string temp = "";
            if (!checkHasExtention(obj))
            {
                temp = StringEncrypt(obj);
            }
            return temp;
        }
        public void Encrypt(string readLocation, string SaveLocation, string seed = "")
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// function decrypts the given obj
        /// <para/> returns a decrypted string or the name of the decrypted file
        /// </summary>
        /// <param name="obj"> the obj to decrypt</param>
        /// <returns>returns a decrypted string or the name of the decrypted file name</returns>
        public string Decrypt(string obj)
        {
            string decryptedObj = "";
            if (!checkHasExtention(obj))
            {
                string key = "";
                string seed = "";
                decryptedObj = decryptString(obj,key,seed);
            }
            return decryptedObj;
        }
        public string decryptString(string temp, string key, string seed)
        {
            // convert encrypted string
            byte[] encrypStrAsBytes = Convert.FromBase64String(temp);
            byte[] intialText = new Byte[encrypStrAsBytes.Length];
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                using (MemoryStream memstream = new MemoryStream(encrypStrAsBytes))
                {
                    if (savedIV == null || savedKey == null)
                    {
                        throw new NullReferenceException("saved key or iv are  is set to null");
                    }
                    //create decryptor & stream obj
                    using (ICryptoTransform rdTransform = rijndael.CreateDecryptor((byte[])savedKey.Clone(), (byte[])savedIV.Clone()))
                    {
                        using (CryptoStream cryptostream = new CryptoStream(memstream,rdTransform,CryptoStreamMode.Read))
                        {
                            // read encryption
                            cryptostream.Read(intialText, 0, intialText.Length);
                        }
                    }
                }
            }
            // convet to str
            string decryptedStr = Encoding.ASCII.GetString(intialText);
            return decryptedStr;
        }
        public void Decrypt(string readLocation,string saveLocation)
        {
            throw new NotImplementedException();
        }
        public bool compareFile(string fileName,string SecondFileName)
        {
            throw new NotImplementedException();
        }
    }
}
