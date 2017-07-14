using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
/*
 * Build: 0.5.3
 * Date: 7/13/17
 * Code Metrics:
 * Network Encryption:77    90  1   17  222
 */
namespace Networking_Encryption
{
    public class Encryption
    {

        #region Parse Functions
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
                if (num % 4 == 3)
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
        /// functions parses the last four elements of given str
        /// <para/> returns an int
        /// </summary>
        /// <param name="text">txt to  parse</param>
        /// <returns>returns an int: len</returns>
        private int parseStrSize(ref string text)
        {
            if (text.Length <= 4)
            {
                throw new FormatException("invalid text format");
            }
            string len = text.Substring(text.Length - 4, 4);
            foreach (char num in len)
            {
                if (num < '0' || num > '9')
                {
                    throw new InvalidDataException("invalid char found");
                }
            }
            text = text.Substring(0, text.Length - 4);
            return Convert.ToInt32(len);
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
            while (index < 48) // make ownKey
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
            return textAsBytes.Length >= 48 ? text.Substring(48, text.Length - index) : "";
        }
        /// <summary>
        /// function Parses & sets a key & seed for Rijndael str decryption 
        /// <para/> returns a parsed text back to the user
        /// </summary>
        /// <param name="text">text to parse from</param>
        /// <returns>returns the parsed text back to the user</returns>
        private string parseStrKeyAndSeed(string text)
        {
            byte[] key = new byte[32];
            byte[] seed = new byte[16];
            int stringIndex = 0;
            int arrayIndex = 0;
            while (arrayIndex < 48)
            {
                if (arrayIndex < 32)
                {
                    key[arrayIndex] = Convert.ToByte(text.Substring(stringIndex, 3));
                }
                else
                {
                    seed[arrayIndex - 32] = Convert.ToByte(text.Substring(stringIndex, 3));
                }
                stringIndex += 3;
                arrayIndex++;
            }
            rdKey = key;
            rdSeed = seed;
            var a = text.Substring(stringIndex, text.Length - stringIndex);
            return text.Substring(stringIndex, text.Length - stringIndex);
        }
        #endregion

        #region File Extension Functions
        /// <summary>
        /// function compares the file extentions of two files
        /// <para/>returns true if they hold the same extention 
        /// </summary>
        /// <param name="fileOne"> file name one </param>
        /// <param name="fileTwo">file name two</param>
        /// <returns> returns true if the two files contain the same extention </returns>
        private bool checkExtention(string fileOne, string fileTwo)
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
        #endregion

        #region RijnDael Functions
        /// <summary>
        /// key for Rijndael Algo
        /// </summary>
        private static byte[] rdKey = null;
        /// <summary>
        /// seed to run Rijndael on
        /// </summary>
        private static byte[] rdSeed = null;
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
        private static void rdGenerateKeys(RijndaelManaged provider, byte[] key = null, byte[] seed = null)
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
        #endregion

        #region Rijndael String Encrypt & Decrypt
        /// <summary>
        /// function Encrypts the given Str using the rijndael Class
        /// <para/> returns an encrypted String
        /// </summary>
        /// <param name="strToEncrypt">string to encrypt</param>
        /// <param name="seed"> seed to run Rijndael on</param>
        /// <returns> returns an encrypted string</returns>
        private string StringEncrypt(string strToEncrypt, string seed = "")
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
            string encryptedStr = "";
            encryptedStr += makeStr(rdKey);// add key to Str
            encryptedStr += makeStr(rdSeed); // add seed to Str
            encryptedStr += Convert.ToBase64String(encryptedBytes); // encrypted Str
            encryptedStr += len; // add size of org Str
            rdKey = rdSeed = null;
            return encryptedStr;
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
                        using (CryptoStream cryptostrm = new CryptoStream(memStrm, rdTransfrm, CryptoStreamMode.Read))
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
        #endregion

        #region Encryption Functions
        /// <summary>
        /// function encrypts the given  obj
        /// <para/> function returns an encrypted string or a  the name of the file where the file was encrypted
        /// </summary>
        /// <param name="obj"> obj to encrypt</param>
        /// <param name="seed">the specified seed to run the encryption on</param>
        /// <returns>returns an encrypted string ro  the name of the file where the file was encrypted</returns>
        public string Encrypt(string obj, string seed = "")
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
                    temp = StringEncrypt(obj, seed);
                }
            }
            else
            {
                if (seed == "")
                {
                    Encrypt(obj, obj);
                }
                else
                {
                    Encrypt(obj, obj, seed);
                }
            }

            return temp;
        }
        #endregion

        #region Decrypion Functions
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
                int len = parseStrSize(ref obj);
                obj = parseStrKeyAndSeed(obj);
                decryptedObj = decryptString(obj);
                decryptedObj = decryptedObj.Substring(0, len);
            }
            else
            {
                Decrypt(obj, obj);
            }
            return decryptedObj;
        }
        #endregion

        /// <summary>
        /// function encrypts the given file using Des Encryptor Class
        /// </summary>
        /// <param name="readLocation"> file to read from</param>
        /// <param name="SaveLocation"> file to write to</param>
        /// <param name="seed"> seed to run encryption algo</param>
        public void Encrypt(string readLocation, string SaveLocation, string seed = "")
        {
            throw new NotImplementedException();
        }
        private byte[] desKey;
        private byte[] desSeed;
        private SymmetricAlgorithm symetricAlgo;
        private string path;
        private void TextFileEncrypt(string data, string saveLocation)
        {
            byte[] encodedData = Encoding.Unicode.GetBytes(data);
            using (FileStream fileStrm = new FileStream(saveLocation, FileMode.Create, FileAccess.Write))
            {
                //genKey
                //genSeed
                using (ICryptoTransform transform = symetricAlgo.CreateEncryptor(desKey,desSeed))
                {
                    using (CryptoStream cryptoStrm = new CryptoStream(fileStrm,transform,CryptoStreamMode.Write))
                    {
                        cryptoStrm.Write(encodedData, 0, encodedData.Length);
                    }
                }
            }
        }
        /// <summary>
        /// function  decrypts the given file using the DES encrytion class
        /// </summary>
        /// <param name="readLocation"> file to read</param>
        /// <param name="saveLocation">file to write to</param>
        public void Decrypt(string readLocation,string saveLocation)
        {
            throw new NotImplementedException();
        }
        public bool compareFile(string fileName,string SecondFileName)
        {
            bool areEqual = false;
            if (checkExtention(fileName,SecondFileName))
            {
                using(FileStream fStrmOne = new FileStream(fileName,FileMode.Open,FileAccess.Read))
                {
                    using (FileStream fStrmTwo = new FileStream(SecondFileName,FileMode.Open,FileAccess.Read))
                    {
                        using (BinaryReader binReaderOne = new BinaryReader(fStrmOne))
                        {
                            using (BinaryReader binReaderTwo = new BinaryReader(fStrmTwo))
                            {
                                int fileLenOne = Convert.ToInt32(binReaderOne.BaseStream.Length);
                                int fileLenTwo = Convert.ToInt32(binReaderTwo.BaseStream.Length);

                                if (fileLenOne == fileLenTwo)
                                {
                                    int pos = 0;
                                    byte[] fileOneBytes = binReaderOne.ReadBytes(fileLenOne);
                                    byte[] fileTwoBytes = binReaderTwo.ReadBytes(fileLenTwo);
                                    if (fileOneBytes.Length == fileTwoBytes.Length)
                                    {
                                        while (pos < fileOneBytes.Length && fileTwoBytes[pos] == fileTwoBytes[pos])
                                        {
                                            pos++;
                                        }
                                        if (pos == fileOneBytes.Length && pos == fileTwoBytes.Length)
                                        {
                                            areEqual = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return areEqual;
        }
    }
}
