using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *CodeMetrics: 70   33  1   5   61 
 */
namespace Networking_Encryption
{
    #region Pair Exceptions
    public class IsNullException : Exception
    {
        public IsNullException(string message = "", Exception inner = null)
            : base(message, inner)
        { }
    }
    public class InvalidLengthException : Exception
    {
        public InvalidLengthException(string message = "", Exception inner = null)
            : base(message, inner)
        { }
    }
    public class OutOfDomainException : Exception
    {
        public OutOfDomainException(string message = null, Exception inner = null)
            : base(message, inner)
        { }
    }
    #endregion

    /// <summary>
    /// class holds the key , seed and the type of encryption used 
    /// </summary>
    public class KeyHolder
    {
        //class constants
        private const int KeyLen = 32;
        private const int SeedLen = 16;

        // class attributes
        private byte[] key;
        private byte[] seed;

        #region get/set property functions
        public byte[] Key
        {
            get { return key; }
        }
        public byte[] Seed
        {
            get { return seed; }
        }
        #endregion

        #region CTORS
        /// <summary>
        /// default Null CTOR
        /// </summary>
        public KeyHolder()
        {
            key = null;
            seed = null;
        }
        /// <summary>
        /// intilizes the class to given byte[] values given
        /// </summary>
        /// <param name="newKey"> key to set</param>
        /// <param name="newSeed">seed to set</param>
        public KeyHolder(byte[] newKey, byte[] newSeed)
        {
            setKey(newKey);
            setSeed(newSeed);
        }
        /// <summary>
        /// intilizes the class to given string values given
        /// </summary>
        /// <param name="newKey"> key to set</param>
        /// <param name="newSeed">seed to set</param>
        public KeyHolder(string newKey, string newSeed)
        {
            setKey(newKey);
            setSeed(newSeed);
        }
        #endregion

        #region set Functions

        #region setKey functions
        /// <summary>
        /// function takes given key and sets as the class key
        /// </summary>
        /// <param name="newKey">key to set</param>
        public void setKey(byte[] newKey)
        {
            if (newKey.Length != KeyLen)
            {
                throw new InvalidLengthException("key is not of valid size");
            }
            key = newKey;
        }
        /// <summary>
        /// function takes given key and sets as the class key
        /// </summary>
        /// <param name="newKey">key to set</param>
        public void setKey(string newKey)
        {
            setKey(parseString(newKey, true));
        }
        #endregion
        #region SetSeed Functions
        /// <summary>
        /// function takes given seed and sets it as the class seed
        /// </summary>
        /// <param name="newSeed">seed to set</param>
        public void setSeed(byte[] newSeed)
        {
            if (newSeed.Length != SeedLen)
            {
                throw new InvalidLengthException("key is not of valid size");
            }
            seed = newSeed;
        }
        /// <summary>
        /// function takes given seed and sets it as the class seed
        /// </summary>
        /// <param name="newSeed">seed to set</param>
        public void setSeed(string newSeed)
        {
            setSeed(parseString(newSeed, false));
        }
        #endregion

        #endregion

        #region ParseFunctions
        /// <summary>
        /// function parses given string into a valid sized byte array
        /// </summary>
        /// <param name="unparsedStr">string to parse</param>
        /// <<param name="isKey"> true if parse if for a key</param>
        /// <returns></returns>
        static public byte[] parseString(string unparsedStr, bool isKey)
        {
            bool endOfStr = false;
            int arrayIndex = 0;
            int strIndex = 0;
            int strLen = unparsedStr.Length;
            int validSize = isKey ? KeyLen : SeedLen;

            int[] tempArray = new int[validSize];

            while (arrayIndex < tempArray.Length && endOfStr == false)
            {
                if (strIndex + 2 < strLen)
                {
                    tempArray[arrayIndex] = Convert.ToInt32(unparsedStr.Substring(strIndex, 3));
                    strIndex += 3;
                }
                else if (strIndex < strLen && strIndex + 2 > strLen)
                {
                    string leftovrLtrs = unparsedStr.Substring(strIndex);
                    leftovrLtrs = leftovrLtrs.Length == 1 ? "00" + leftovrLtrs : "0" + leftovrLtrs;
                    tempArray[arrayIndex] = Convert.ToInt32(leftovrLtrs);
                    endOfStr = true;
                }
                else if (strIndex >= strLen && strIndex < validSize)
                {
                    endOfStr = true;
                }
                arrayIndex++;
            }
            if (!checkIsValidByte(tempArray))
            {
                throw new OutOfDomainException("string contains numbers outside the domain of a byte");
            }
            if (!checkSize(tempArray))
            {
                throw new InvalidLengthException("string is too short");
            }
            return Array.ConvertAll(tempArray, Convert.ToByte);
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
        /// <returns>returns true if array is within the domain of an array</returns>
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
