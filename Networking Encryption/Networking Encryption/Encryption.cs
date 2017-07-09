using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
/*
 * Build: 0.4.4
 * Date: 7/8/17
 * Code Metrics:
 * Network Encryption: 80   73  1   14  175
 */
 /*
  * note to self to to append the size fo the str to encrypt to the decrypted str
  * and also add handling for the decryption to ret correctly parsed str;
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
                    retVal += mod == 1 ? "00" : "0";
                }
                retVal += temp;
            }
            return retVal;
        }
        /// <summary>
        /// function parses the given int into valid str for encryption
        /// <para/> returns the parsed num
        /// </summary>
        /// <param name="num"> num to parse</param>
        /// <returns>parsed num</returns>
        private string makeStr(int num)
        {
            string temp = "";
            if (num > 9999) // last valid size to parse
            {
                throw new FormatException("to large to handle");
            }
            if (num % 4 != 0)
            {
                if ( num % 4 == 3 )
                {
                    temp = "0";
                }
                else
                {
                    temp = num % 4 == 2 ? "00" : "000"; 
                }
            }
            temp += num.ToString();
            return temp;
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
            checkKeyAndSeed();
        }
        /// <summary>
        /// function checks that the generated key and seed are within valid bounds of ascii
        /// </summary>
        private static void checkKeyAndSeed()
        {
            const int MaxAscii = 127; // last valid number for ascii table
            for (int index = 0; index < rdKey.Length; index++)
            {
                if (rdKey[index] > MaxAscii)
                {
                    rdKey[index] /= 2;
                }
            }
            for (int index = 0; index < rdSeed.Length; index++)
            {
                if (rdSeed[index] > MaxAscii)
                {
                    rdSeed[index] /= 2;
                }
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
        private string makeKeyAndSeed(string text, ref byte[] key, ref byte[] seed)
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
                index++;
            }
            while (index < 48 ) // make ownKey
            {
                const byte lastAsciiVal = 127;
                byte temp = lastAsciiVal;
                if (index < 32) // the size req for a valid key
                {
                    key[index] = temp;
                }
                else // index >= 32
                {
                    seed[index - 32] = temp;
                }
                temp = temp > 0 ? temp-- : lastAsciiVal;
                index++;
            }
            return textAsBytes.Length >= 48 ? text.Substring(48,text.Length - index) : "";
        }

        private string StringEncrypt(string strToEncrypt,string seed = "")
        {
            // encode data
            byte[] strAsBytes = Encoding.ASCII.GetBytes(strToEncrypt);
            byte[] encryptedBytes = { };
            string len = makeStr(strToEncrypt.Length);

            //create memstream
            using (MemoryStream memStrm = new MemoryStream(strAsBytes.Length))
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    if (seed != "")
                    {
                        byte[] key = new byte[32];
                        byte[] algoSeed = new byte[16];
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
                            encryptedBytes = memStrm.ToArray();
                        }
                    }
                }
            }
            //convert encrypted string

            string encryptedStr = "";
            encryptedStr += makeStr(rdKey);
            var a = makeStr(rdKey);
            var b = makeStr(rdSeed);
            encryptedStr += makeStr(rdSeed);
            encryptedStr += Convert.ToBase64String(encryptedBytes);
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
                obj = parseStrKeyAndSeed(obj);
                decryptedObj = decryptString(obj);
            }
            return decryptedObj;
        }

        private string parseStrKeyAndSeed(string obj)
        {
            byte[] key = new byte[32];
            byte[] seed = new byte[16];
            int stringIndex = 0;
            int arrayIndex = 0;
            while (arrayIndex < 48)
            {
                if (arrayIndex < 32)
                {
                    key[arrayIndex] = Convert.ToByte(obj.Substring(stringIndex, 3));
                }
                else
                {
                    seed[arrayIndex - 32] = Convert.ToByte(obj.Substring(stringIndex, 3)); 
                }
                stringIndex += 3;
                arrayIndex++;
            }
            rdKey = key;
            rdSeed = seed;
            var a = obj.Substring(stringIndex, obj.Length - stringIndex);
            return obj.Substring(stringIndex,obj.Length - stringIndex);
        }

        /// <summary>
        /// function takes an encrypted string and Decrypts it
        /// </summary>
        /// <param name="encryptedStr">string to decrypt</param>
        /// <returns>returns a decrypted string</returns>
        public string decryptString(string encryptedStr)
        {
            // convert encrypted string
            byte[] encrypStrAsBytes = Convert.FromBase64String(encryptedStr);
            byte[] orginalText = new Byte[encrypStrAsBytes.Length];
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                using (MemoryStream memStrm = new MemoryStream(encrypStrAsBytes))
                {
                    if (rdSeed == null || rdKey == null)
                    {
                        throw new NullReferenceException("saved key or iv are  is set to null");
                    }
                    //create decryptor & stream obj
                    using (ICryptoTransform rdTransfrm = rijndael.CreateDecryptor((byte[])rdKey.Clone(), (byte[])rdSeed.Clone()))
                    {
                        using (CryptoStream cryptostrm = new CryptoStream(memStrm,rdTransfrm,CryptoStreamMode.Read))
                        {
                            // read encryption
                            cryptostrm.Read(orginalText, 0, orginalText.Length);
                        }
                    }
                }
            }
            rdKey = rdSeed = null;
            return Encoding.ASCII.GetString(orginalText);
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
