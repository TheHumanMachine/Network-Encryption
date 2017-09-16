using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption
{
    /// <summary>
    /// class holds the key , seed and the type of encryption used 
    /// </summary>
    public class Pair
    {
        //class constants
        private const int RijdaelKeySize = 32;
        private const int RijdaelSeedSize = 16;
        private const int AesKeySize = 32;
        private const int AesSeedSize = 16;
        //private const int AesKeySize = 32;
        //private const int AesSeedSize = 16;

        // class attributes
        private byte[] key;
        private byte[] seed;
        private EncryptionMode mode;
        private int byteLen;

        #region get/set property functions
        public int Length
        {
            get { return byteLen; }
            set { byteLen = value; }
        }
        public byte[] Key
        {
            get { return key; }
        }
        public byte[] Seed
        {
            get { return seed; }
        }
        public EncryptionMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        #endregion

        #region CTORS
        /// <summary>
        /// default null CTOR
        /// </summary>
        public Pair()
        {
            key = null;
            seed = null;
            mode = EncryptionMode.Null;
            byteLen = 0;
        }
        /// <summary>
        /// intilizes the class to specs of the given EncryptionMode
        /// </summary>
        /// <param name="newKey"> key to set</param>
        /// <param name="newSeed">seed to set</param>
        /// <param name="newMode">type of encryption to be used for</param>
        public Pair(byte[] newKey, byte[] newSeed, EncryptionMode newMode, int len)
        {
            mode = newMode;
            byteLen = len;
            setKey(newKey);
            setSeed(newSeed);
        }
        /// <summary>
        /// intilizes the class to specs of the given EncryptionMode
        /// </summary>
        /// <param name="newKey"> key to set</param>
        /// <param name="newSeed">seed to set</param>
        /// <param name="len">length of the encrypted obj</param>
        /// <param name="newMode">type of encryption to be used for</param>
        public Pair(string newKey, string newSeed, EncryptionMode newMode,int len)
        {
            mode = newMode;
            byteLen = len;
            setKey(newKey);
            setSeed(newSeed);
        }
        #endregion

        #region set Functions
        /// <summary>
        /// function takes given param and sets as the class key
        /// </summary>
        /// <param name="newKey">key to set</param>
        public void setKey(byte[] newKey)
        {
            int validSize = 0;
            if (mode == EncryptionMode.Null)
            {
                throw new KeyNotFoundException(" mode is set to null");
            }
            else if (mode == EncryptionMode.Aes)
            {
                validSize = AesKeySize;
            }
            else
            {
                validSize = RijdaelKeySize;
            }
            if (newKey.Length != validSize)
            {
                throw new ArgumentException("key is not of valid size");
            }
            key = newKey;
        }
        /// <summary>
        /// function takes given param and sets as the class key
        /// </summary>
        /// <param name="newKey">key to set</param>
        public void setKey(string newKey)
        {
            if (mode == EncryptionMode.Null)
            {
                throw new KeyNotFoundException(" mode is set to null");
            }
            else if (mode == EncryptionMode.Aes)
            {
                setKey(parseString(newKey,AesKeySize));
            }
            else
            {
                setKey(parseString(newKey,RijdaelKeySize));
            }
        }
        /// <summary>
        /// function takes given param and sets it as the class seed
        /// </summary>
        /// <param name="newSeed">seed to set</param>
        public void setSeed(byte[] newSeed)
        {
            int validSize = 0;
            if (mode == EncryptionMode.Null)
            {
                throw new KeyNotFoundException(" mode is set to null");
            }
            else if (mode == EncryptionMode.Aes)
            {
                validSize = AesSeedSize;
            }
            else
            {
                validSize = RijdaelSeedSize;
            }
            if (newSeed.Length != validSize)
            {
                throw new ArgumentException("key is not of valid size");
            }
            seed = newSeed;
        }
        /// <summary>
        /// function takes given param and sets it as the class seed
        /// </summary>
        /// <param name="newSeed">seed to set</param>
        public void setSeed(string newSeed)
        {
            if (mode == EncryptionMode.Null)
            {
                throw new KeyNotFoundException(" mode is set to null");
            }
            else if (mode == EncryptionMode.Aes)
            {
                setSeed(parseString(newSeed, AesSeedSize));
            }
            else
            {
                setSeed(parseString(newSeed, RijdaelSeedSize));
            }
        }
        #endregion

        #region ParseFucntions
        /// <summary>
        /// function parses given string into a valid size byte array
        /// </summary>
        /// <param name="unparsedStr">string to parse</param>
        /// <param name="validSize">the size of the array</param>
        /// <returns></returns>
        private byte[] parseString(string unparsedStr, int validSize)
        {
            int strLen = unparsedStr.Length;
            byte[] validArray = new byte[validSize];
            int[] tempArray = new int[validSize];
            int strIndex = 0;
            for (int index = 0; index < validArray.Length; index++)
            {

                if (strIndex + 2 < strLen)
                {
                    tempArray[index] = Convert.ToInt32(unparsedStr.Substring(strIndex, 3));
                    strIndex += 3;
                }
                else if (strIndex < strLen &&  strIndex + 2 > strLen)
                {
                    string temp = unparsedStr.Substring(strIndex);
                    temp = temp.Length == 1 ? "00" + temp : "0" + temp;
                    tempArray[index] = Convert.ToInt32(temp);
                    break;
                }
                if (strIndex >= strLen && strIndex < validSize)
                {
                    throw new ArgumentOutOfRangeException("given string is of invalid size");
                }
            }
            if (!checkIsValidByte(tempArray))
            {
                throw new FormatException("contains numbers too large or too small to be a byte");
            }
            if (!checkSize(tempArray))
            {
                throw new ArgumentException("string is of invalid size");
            }
            validArray = Array.ConvertAll(tempArray, Convert.ToByte);
            return validArray;
        }
        /// <summary>
        /// function parses given string into a valid size byte array
        /// </summary>
        /// <param name="unparsedStr">string to parse</param>
        /// <<param name="isKey"> true if parse if for a key</param>
        /// <param name="type"> type of encryption it will be used for</param>
        /// <returns>an array to valid specs</returns>
        public static byte[] parseString(string unparsedStr, EncryptionMode type, bool isKey)
        {
            int validSize = 0;
            if (isKey)
            {
                validSize = type == EncryptionMode.Aes ? AesKeySize : RijdaelKeySize;
            }
            else
            {
                validSize = type == EncryptionMode.Aes ? AesSeedSize : RijdaelSeedSize;
            }
            int strLen = unparsedStr.Length;
            byte[] validArray = new byte[validSize];
            int[] tempArray = new int[validSize];
            int strIndex = 0;
            for (int index = 0; index < validArray.Length; index++)
            {
                if (strIndex + 2 < strLen)
                {
                    tempArray[index] = Convert.ToByte(unparsedStr.Substring(strIndex, 3));
                    strIndex += 3;
                }
                else if (strIndex < strLen && strIndex + 2 > strLen)
                {
                    string temp = unparsedStr.Substring(strIndex);
                    temp = temp.Length == 1 ? "00" + temp : "0" + temp;
                    tempArray[index] = Convert.ToByte(temp);
                }
                if (index >= strLen && index < validSize)
                {
                    break;
                }

            }
            if (!checkIsValidByte(tempArray))
            {
                throw new ArgumentException("contains numbers too large or too small to be a byte");
            }
            if (!checkSize(tempArray))
            {
                throw new ArgumentException("string is of invalid size");
            }
            validArray = Array.ConvertAll(tempArray,Convert.ToByte);
            return validArray;
        }
        /// <summary>
        /// function checks if the parsing of the string was of correct size
        /// <para>returns true if the function is of valid syntax</para>
        /// </summary>
        /// <param name="array">array to check</param>
        /// <returns>returns true if the array is of valid syntax</returns>
        private static bool checkSize(int[] array)
        {
            bool correctSize = true;
            int index = 0;
            while (correctSize == true && index < array.Length)
            {
                if (array[index] == 0 && array[index] == array[index + 1])
                {
                    correctSize = false;
                }
                index++;
            }
            return correctSize;
        }
        /// <summary>
        /// functions checks is if the given array is within the bounds of a valid byte
        /// <para> returns true if it is valid</para>
        /// </summary>
        /// <param name="array">array to check</param>
        /// <returns>returns true if array is within th bounds of an array</returns>
        private static bool checkIsValidByte(int[] array)
        {
            bool isValid = true;
            int index = 0;
            while (isValid == true && index < array.Length)
            {
                if (array[index] > 255 || array[index] < 0)
                {
                    isValid = false;
                }
                index++;
            }
            return isValid;
        }
        #endregion

    }
}
