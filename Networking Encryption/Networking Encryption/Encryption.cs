using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
/*
 * Build: 0.4.0
 * Date: 7/2/17
 * Code Metrics:
 * Network Encryption: 78   59  1   14  138
 */
namespace Networking_Encryption
{
    public class Encryption
    {
        private static byte[] rdKey = null;
        private static byte[] rdSeed = null;

        public byte[] Key
        {
            get
            { return rdKey; }
            set { rdKey = value; }
        }

        public byte[] IV
        {
            get
            { return rdSeed; }
            set { rdSeed = value; }
        }
        /// <summary>
        /// function parses the given byte[] into a valid string for the class user
        /// </summary>
        /// <param name="array">array to convert to String</param>
        /// <returns></returns>
        private string makeStr(byte[] array)
        {
            string retVal = "";
            string temp = "";
            int mod = 0;
            for (int index = 0; index < array.Length; index++)
            {
                temp = array[index].ToString();
                mod = temp.Length % 3;
                if (mod > 0)
                {
                    retVal += mod == 1 ? "00" : "0";// account for digit sig ex: 001 etc
                }
                retVal += temp;
            }
            return retVal;
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
        /// <summary>
        /// generates a random key to be used for the encryption algo
        /// </summary>
        /// <param name="provider"> class to intialize the key</param>
        private static void rdGenerateKey(RijndaelManaged provider)
        {
            if (rdKey == null)
            {
                provider.KeySize = 256;
                provider.GenerateKey();
                rdKey = provider.Key;
            }
        }
        /// <summary>
        /// function intializes key and seed compents for the string encryption
        /// </summary>
        /// <param name="provider"> stream to intialize the key & seed compenents</param>
        /// <param name="key">the key to set if given one</param>
        /// <param name="seed">the seed to intialize of off if given</param>
        private static void rdGenerateKeys(RijndaelManaged provider,byte[] key = null,byte[] seed = null)
        {
            if (key == null)
            {
                rdGenerateKey(provider);
            }
            else
            {
                rdKey = key;
            }
            if (seed == null)
            {
                rdGenerateSeed(provider);
            }
            else
            {
                rdSeed = seed;
            }
        }
        /// <summary>
        /// function intializes the seed to run the encryption algo
        /// </summary>
        /// <param name="provider">class to generate the seed</param>
        private static void rdGenerateSeed(RijndaelManaged provider)
        {
            if (rdSeed == null)
            {
                provider.GenerateKey();
                rdSeed = provider.IV;
            }
        }
        /// <summary>
        /// function parses  the key & seed to be used within string encrytion
        /// </summary>
        /// <param name="text"> given text to parse from</param>
        /// <param name="key">key to be used for encryption algo</param>
        /// <param name="seed">seed to be used within the encryption algo</param>
        /// <returns>a substring of the left over text</returns>
        private static string makeKeyAndSeed(string text, ref byte[] key, ref byte[] seed)
        {
            byte[] textAsBytes = Encoding.ASCII.GetBytes(text);
            int index = 0;
            while (index < 48 && index < textAsBytes.Length)
            {
                if (index < 32) // the size req for a valid key
                {
                    key[index] = textAsBytes[index];
                }
                else // index >= 32
                {
                    seed[index - 32] = textAsBytes[index];
                }
            }
            while (index < 48 ) // make ownKey
            {
                byte temp = 255;
                if (index < 32) // the size req for a valid key
                {
                    key[index] = temp;
                }
                else // index >= 32
                {
                    seed[index - 32] = temp;
                }
                temp--;
            }
            return textAsBytes.Length >= 48 ? text.Substring(48,text.Length - index) : "";
        }

        private string StringEncrypt(string strToEncrypt,string seed = "")
        {
            // encode data
            byte[] strAsBytes = Encoding.ASCII.GetBytes(strToEncrypt);
            byte[] orginalBytes = { };

            //create memstream
            using (MemoryStream memStrm = new MemoryStream(strAsBytes.Length))
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    if (seed != "")
                    {
                        byte[] key = null;
                        byte[] algoSeed = null;
                        makeKeyAndSeed(seed, ref key, ref algoSeed);
                        rdGenerateKeys(rijndael, key, algoSeed);
                    }
                    else
                    {
                        rdGenerateKeys(rijndael);
                    }
                    if (rdSeed == null || rdKey == null)
                    {
                        throw new NullReferenceException(" one of keys is null");
                    }
                    //create encryptor & streams
                    using (ICryptoTransform rdTransfrm = rijndael.CreateEncryptor((byte[])rdKey.Clone(), (byte[])rdSeed.Clone()))
                    {
                        using (CryptoStream cryptoStrm = new CryptoStream(memStrm, rdTransfrm, CryptoStreamMode.Write))
                        {
                            // write encrypted Data
                            cryptoStrm.Write(strAsBytes, 0, strAsBytes.Length);
                            cryptoStrm.FlushFinalBlock();
                            orginalBytes = memStrm.ToArray();
                        }
                    }
                }
            }
            //convert encrypted string

            string encryptedStr = "";
            encryptedStr += makeStr(rdSeed);
            encryptedStr += makeStr(rdKey);
            encryptedStr += Convert.ToBase64String(orginalBytes);
            rdKey = rdSeed = null;
            return encryptedStr;
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
                if (seed == "")
                {
                    temp = StringEncrypt(obj);
                }
                else
                {
                    temp = StringEncrypt(obj,seed);
                }
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
                    if (rdSeed == null || rdKey == null)
                    {
                        throw new NullReferenceException("saved key or iv are  is set to null");
                    }
                    //create decryptor & stream obj
                    using (ICryptoTransform rdTransform = rijndael.CreateDecryptor((byte[])rdKey.Clone(), (byte[])rdSeed.Clone()))
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
