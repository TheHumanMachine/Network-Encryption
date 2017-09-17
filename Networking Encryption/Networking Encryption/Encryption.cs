using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
/*
 * CodeMetrics: 55  58  1   19  167
 */
namespace Networking_Encryption
{
    /// <summary>
    /// class encrypts and decrypts given data
    /// </summary>
    public class Encryption
    {
        //class constants
        private const int KeyLen = 32;
        private const int SeedLen = 16;

        #region Aes Encryption Class
        class AesEncryption
        {
            //class consts
            private const int AesKeySize = 32;
            private const int AesSeedSize = 16;

            private byte[] key = null;
            private byte[] seed = null;

            #region Get/Set Functs
            public byte[] Key
            {
                get { return key; }
                set { key = value; }
            }
            public byte[] Seed
            {
                get { return seed; }
                set { seed = value; }
            }
            #endregion
            public void GenKey()
            {
                if (key == null)
                {
                    key = new byte[AesKeySize];
                    using (RandomNumberGenerator ranGen = RandomNumberGenerator.Create())
                    {
                        ranGen.GetBytes(key);
                    }
                }
            }
            public void GenSeed()
            {
                if (seed == null)
                {
                    seed = new byte[AesSeedSize];
                    using (RandomNumberGenerator ranGen = RandomNumberGenerator.Create())
                    {
                        ranGen.GetBytes(seed);
                    }
                }
            }
            public void FlushKeys()
            {
                key = seed = null;
            }

            #region Encryption Functions
            public string Encrypt(string strToEncrypt)
            {
                string encryptedStr = "";
                byte[] strAsBytes = Encoding.ASCII.GetBytes(strToEncrypt);
                byte[] encryptedBytes = null;

                GenKey();
                GenSeed();
                using (MemoryStream memStrm = new MemoryStream(strAsBytes.Length))
                {
                    using (Aes aes = Aes.Create())
                    {
                        using (ICryptoTransform cryptTransfrm = aes.CreateEncryptor(key, seed))
                        {
                            using (CryptoStream cryptoStrm = new CryptoStream(memStrm, cryptTransfrm, CryptoStreamMode.Write))
                            {
                                cryptoStrm.Write(strAsBytes, 0, strAsBytes.Length);
                                cryptoStrm.FlushFinalBlock();
                                encryptedBytes = memStrm.ToArray();
                            }
                        }
                    }
                }
                string len = strToEncrypt.Length.ToString("X");
                len += "X";
                encryptedStr += Convert.ToBase64String(encryptedBytes); // encrypted Str
                return len + encryptedStr;
            }
            public void Encrypt(string inputFile, string outputFile)
            {
                const int BUFFER_SIZE = 4096;//8192
                byte[] buffer = new byte[BUFFER_SIZE];
                GenKey();
                GenSeed();
                using (FileStream inputStream = File.Open(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (Aes cryptoAlgo = Aes.Create())
                        {
                            using (ICryptoTransform encryptor = cryptoAlgo.CreateEncryptor(key, seed))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                                {
                                    int count;
                                    while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        cryptoStream.Write(buffer, 0, count);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Decryption Functions
            public string Decrypt(string encryptedStr)
            {
                char temp = new char();
                int index = 0;
                while (temp != 'X')
                {
                    temp = encryptedStr[index];
                    index++;
                }
                int len = int.Parse(encryptedStr.Substring(0, index - 1), System.Globalization.NumberStyles.HexNumber);
                encryptedStr = encryptedStr.Substring(index);
                byte[] encrypStrAsBytes = Convert.FromBase64String(encryptedStr);
                byte[] orginalText = new Byte[encrypStrAsBytes.Length];
                using (MemoryStream memStrm = new MemoryStream(encrypStrAsBytes))
                {
                    using (Aes aes = Aes.Create())
                    {
                        using (ICryptoTransform cryptoTransfrm = aes.CreateDecryptor(key,seed))
                        {
                            using (CryptoStream cryptostrm = new CryptoStream(memStrm, cryptoTransfrm, CryptoStreamMode.Read))
                            {
                                cryptostrm.Read(orginalText, 0, orginalText.Length);
                            }
                        }
                    }
                }
                key = seed = null;
                return Encoding.ASCII.GetString(orginalText).Substring(0,len);
            }
            public void Decrypt(string inputFile, string outputFile)
            {
                const int BUFFER_SIZE = 4096;
                byte[] buffer = new byte[BUFFER_SIZE];
                using (FileStream inputStream = File.Open(inputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (FileStream outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (Aes cryptoAlgo = Aes.Create())
                        {
                            using (ICryptoTransform decryptor = cryptoAlgo.CreateDecryptor(key, seed))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                                {
                                    int count;
                                    while ((count = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        outputStream.Write(buffer, 0, count);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion
        }
        #endregion

        #region Parse Functions
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
        #endregion

        #region Encryption Functions
        /// <summary>
        /// function encrypts the given  str
        /// <para/> function returns an encrypted string
        /// </summary>
        /// <param name="str"> str to encrypt</param>
        /// <param name="seed">the specified seed to run the encryption on</param>
        /// <returns>returns an encrypted string ro  the name of the file where the file was encrypted</returns>
        public string EncryptStr(string str,ref KeyHolder keys, string seed = "")
        {
            if (!FileExtFuncts.checkHasExtention(str))
            {
                using (RijndaelManaged provider = new RijndaelManaged())
                {
                    int len = str.Length;
                    AesEncryption aes = new AesEncryption();

                    if (seed == "")
                    {
                        str = aes.Encrypt(str);
                    }
                    else
                    {
                        keys = new KeyHolder();
                        try
                        {
                            keys.setSeed(seed);
                        }
                        catch (Exception exception)
                        {
                            if (exception is OutOfDomainException)
                            {
                                aes.GenSeed();
                            }
                            else if (exception is InvalidLengthException)
                            {
                                int seedLen = seed.Length;
                                if (seedLen == 1)
                                {
                                    seed = "00" + seed;
                                    for (int count = 0; count < 4; count++)
                                    {
                                        seed += seed;
                                    }
                                    keys.setSeed(seed);
                                }
                                else if (seedLen == 2)
                                {
                                    seed = "0" + seed;
                                    for (int count = 0; count < 4; count++)
                                    {
                                        seed += seed;
                                    }
                                    keys.setSeed(seed);
                                }
                            }
                            else
                            {
                                throw;
                            }
                        }
                        aes.Seed = keys.Seed;
                        str = aes.Encrypt(str);
                    }
                    keys = new KeyHolder(aes.Key, aes.Seed);
                    aes.FlushKeys();
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
        public string Decrypt(string obj,KeyHolder keys)
        {
            string decryptedObj = "";
            if (!FileExtFuncts.checkHasExtention(obj))
            {
                AesEncryption aes = new AesEncryption();
                aes.Key = keys.Key;
                aes.Seed = keys.Seed;
                decryptedObj = aes.Decrypt(obj);
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
        public KeyHolder Encrypt(string readLocation, string SaveLocation, string seed = "")
        {
            KeyHolder keys = null;
            int len = 0;
            AesEncryption aes = new AesEncryption();
            if (seed == "")
            {
                aes.Encrypt(readLocation, SaveLocation);
            }
            else
            {
                keys = new KeyHolder();
                try
                {
                    keys.setKey(seed);
                }
                catch (Exception exception)
                {
                    if (exception is OutOfDomainException)
                    {
                        //do nothing the encryption will generate a random one later
                    }
                    else if (exception is InvalidLengthException)
                    {
                        int seedLen = seed.Length;
                        if (seedLen == 1)
                        {
                            seed = "00" + seed;
                            for (int count = 0; count < 4; count++)
                            {
                                seed += seed;
                            }
                            keys.setSeed(seed);
                        }
                        else if (seedLen == 2)
                        {
                            seed = "0" + seed;
                            for (int count = 0; count < 3; count++)
                            {
                                seed += seed;
                            }
                            keys.setSeed(seed);
                        } // all other seed lens let the encryption create an random one
                    }
                    else
                    {
                        throw;
                    }
                }
                aes.Seed = keys.Seed;
                aes.Encrypt(readLocation, SaveLocation);
            }
            keys = new KeyHolder(aes.Key, aes.Seed);
            return keys;
        }
        /// <summary>
        /// function  decrypts the given file using the DES encrytion class
        /// </summary>
        /// <param name="readLocation"> file to read</param>
        /// <param name="saveLocation">file to write to</param>
        /// <param name="keys">the holder of decryption key and seed</param>
        public void Decrypt(string readLocation,string saveLocation,KeyHolder keys)
        {
            AesEncryption aes = new AesEncryption();
            aes.Key = keys.Key;
            aes.Seed = keys.Seed;
            aes.Decrypt(readLocation, saveLocation);
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
                                        bool equal = true;
                                        while (pos < fileOneBytes.Length && fileTwoBytes[pos] == fileTwoBytes[pos] && equal == true)
                                        {
                                            equal = checkBits(fileOneBytes[pos], fileTwoBytes[pos]);
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

        private bool checkBits(byte byteOne, byte byteTwo)
        {
            bool equal = false;
            int pos = 0;
            string one = "";
            string two = "";
            int temp = 0;
            while (pos < 8)
            {
                temp = (byteOne >> pos) & 0x1;
                one += temp == 1 ? temp.ToString() : "0";
                temp = (byteTwo >> pos) & 0x1;
                two += temp == 1 ? temp.ToString() : "0";
                pos++;
            }
            if (one == two)
            {
                equal = true;
            }

            return equal;
        }

        /// <summary>
        /// reads in a given file
        /// <para>returns data inside the file</para>
        /// </summary>
        /// <param name="path">name of the file</param>
        /// <param name="len"> read file length</param>
        /// <returns>data of the file in the form of  string</returns>
        private string readFile(string path,ref int len)
        {
            string data = "";
            using (FileStream fileStrm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader strmReader = new StreamReader(fileStrm))
                {
                    data = strmReader.ReadToEnd();
                    len = data.Length;
                }
            }
            return data;
        }
        /// <summary>
        /// reads in a given file
        /// <para>returns data inside the file</para>
        /// </summary>
        /// <param name="path">name of the file</param>
        /// <param name="len"> read file length</param>
        /// <returns>data of the file in the form of  string</returns>
        private byte[] readFile(string path, ref int len, int a = 0)
        {
            byte[] fileBytes = null;
            using (FileStream fStrm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binReader = new BinaryReader(fStrm))
                {
                    len = Convert.ToInt32(binReader.BaseStream.Length);
                    fileBytes = binReader.ReadBytes(len);
                }
            }
            return fileBytes;
        }
        /// <summary>
        /// function writes given data to the given file
        /// </summary>
        /// <param name="data">data to write</param>
        /// <param name="path">file to write to</param>
        /// <param name="len"> length of the original file</param>
        private void writeFile(byte[] data, string path,int len)
        {
            using (FileStream fileStrm = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                using (StreamWriter strmWrtr = new StreamWriter(fileStrm))
                {
                    if (data.Length == len)
                    {

                    }
                    strmWrtr.Write(data);
                }
            }
        }
    }
}
