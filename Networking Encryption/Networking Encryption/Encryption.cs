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
    public enum EncryptionMode { Null, RijDanael, Des };
    /// <summary>
    /// class encrypts and decrypts given data
    /// </summary>
    public class Encryption
    {
        #region DES Encryption Class
        private class DesEncryption
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
            /// function resets the key & seed to null
            /// </summary>
            public void flush()
            {
                key = seed = null;
                symetricAlgo = null;
            }
            /// <summary>
            /// generates a key and Seed from the given seed if the seed != "" else generates a random key and Seed
            /// </summary>
            /// <param name="seed"></param>
            private void genKeys(byte[] seed)
            {
                if (seed == null)
                {
                    genSeed();
                }
                else
                {
                    Seed = seed;
                }
                GenKey();
            }
            /// <summary>
            /// encrypts given data to a save location
            /// </summary>
            /// <param name="data">information to encrypt</param>
            /// <param name="saveLocation">path to save the data</param>
            /// <param name="seed">place to start the algo from</param>
            public void TextFileEncrypt(string data, string saveLocation, byte[] seed = null)
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
                    using (ICryptoTransform transform = symetricAlgo.CreateDecryptor(key, seed))
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
        #endregion

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
        /// function encrypts the given  str
        /// <para/> function returns an encrypted string
        /// </summary>
        /// <param name="str"> str to encrypt</param>
        /// <param name="seed">the specified seed to run the encryption on</param>
        /// <returns>returns an encrypted string ro  the name of the file where the file was encrypted</returns>
        public string EncryptStr(string str,ref Pair keys, string seed = "")
        {
            if (!FileExtFuncts.checkHasExtention(str))
            {
                if (seed == "")
                {
                    str = StringEncrypt(str);
                }
                else
                {
                    str = StringEncrypt(str, seed);
                }
            }
            return str;
        }
        #endregion

        #region Decrypion Functions
        /// <summary>
        /// function decrypts the given obj
        /// <para/> returns a decrypted string or the name of the decrypted file
        /// </summary>
        /// <param name="obj"> the obj to decrypt</param>
        /// <returns>returns a decrypted string or the name of the decrypted file name</returns>
        public string Decrypt(string obj,Pair keys)
        {
            string decryptedObj = "";
            if (!FileExtFuncts.checkHasExtention(obj))
            {
                int len = parseStrSize(ref obj);
                obj = parseStrKeyAndSeed(obj);
                decryptedObj = decryptString(obj);
                decryptedObj = decryptedObj.Substring(0, len);
            }
            else
            {
                Decrypt(obj, obj, keys);
            }
            return decryptedObj;
        }
        #endregion

        /// <summary>
        /// function encrypts the given file using Des Encryptor Class
        /// <para>returns a pair that holds key seed and type of encryption used</para>
        /// </summary>
        /// <param name="readLocation"> file to read from</param>
        /// <param name="SaveLocation"> file to write to</param>
        /// <param name="seed"> seed to run encryption algo</param>
        /// <returns>a pair that holds key seed and type of encryption used</returns>
        public Pair Encrypt(string readLocation, string SaveLocation, string seed = "")
        {
            Pair keys = null;
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                {
                    DesEncryption des = new DesEncryption();
                    des.SymetricAlgo = tdes;
                    if (seed == "")
                    {
                        des.TextFileEncrypt(readFile(readLocation), SaveLocation);
                    }
                    else
                    {
                        des.TextFileEncrypt(readFile(readLocation), SaveLocation, makeSeed(seed, EncryptionMode.Des));
                    }
                    keys = new Pair(des.Key, des.Seed, EncryptionMode.Des);
                }
                return keys;
            }
        }
        /// <summary>
        /// function  decrypts the given file using the DES encrytion class
        /// </summary>
        /// <param name="readLocation"> file to read</param>
        /// <param name="saveLocation">file to write to</param>
        /// <param name="keys">the holder of decryption key and seed</param>
        public void Decrypt(string readLocation,string saveLocation,Pair keys)
        {
            if (keys.Mode != EncryptionMode.Des)
            {
                throw new ArgumentException("wrong type of key");
            }
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                DesEncryption des = new DesEncryption();
                des.SymetricAlgo = tdes;
                des.Key = keys.Key;
                des.Seed = keys.Seed;
                writeFile(des.decryptFile(readLocation), saveLocation);
            }
        }
        public bool compareFile(string fileName,string SecondFileName)
        {
            bool areEqual = false;
            if (FileExtFuncts.checkExtention(fileName,SecondFileName))
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
        /// <summary>
        /// reads in a given file
        /// <para>returns data inside the file</para>
        /// </summary>
        /// <param name="path">name of the file</param>
        /// <returns>data of the file in the form of  string</returns>
        private string readFile(string path)
        {
            string data = "";
            using (FileStream fileStrm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader strmReader = new StreamReader(fileStrm))
                {
                    data = strmReader.ReadToEnd();
                }
            }
            return data;
        }
        /// <summary>
        /// function makes a valid seed out of the given string
        /// </summary>
        /// <param name="seed">string to parse</param>
        /// <param name="mode">type of valid seed to make</param>
        /// <returns> a valid seed to be used for encryption</returns>
        private byte[] makeSeed(string seed,EncryptionMode mode)
        {
            byte[] seedArray;
            if (mode == EncryptionMode.RijDanael)
            {
                seedArray = new byte[16];
            }
            else
            {
                seedArray = new byte[8];
            }
            seedArray = Pair.parseString(seed, mode, false);
           // parseSeed(seed, seedArray);
            return seedArray;
        }
        /// <summary>
        /// function writes given data to the given file
        /// </summary>
        /// <param name="data">data to write</param>
        /// <param name="path">file to write to</param>
        private void writeFile(string data, string path)
        {
            using (FileStream fileStrm = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                using (StreamWriter strmWrtr = new StreamWriter(fileStrm))
                {
                    strmWrtr.Write(data);
                }
            }
        }
    }
}
