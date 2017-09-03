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
    class Pair
    {
        //class constants
        private const int RijdaelKeySize = 48;
        private const int RijdaelSeedSize = 16;
        private const int DesKeySize = 24;
        private const int DesSeedSize = 8;

        // class attributes
        private byte[] key = null;
        private byte[] seed = null;
        private EncryptionMode mode = EncryptionMode.Null;

        #region get/set property functions
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

        #region Specialized CTORS
        /// <summary>
        /// intilizes the class to specs of the given EncryptionMode
        /// </summary>
        /// <param name="newKey"> key to set</param>
        /// <param name="newSeed">seed to set</param>
        /// <param name="newMode">type of encryption to be used for</param>
        public Pair(byte[] newKey, byte[] newSeed, EncryptionMode newMode)
        {
            mode = newMode;
            setKey(newKey);
            setSeed(newSeed);
        }
        /// <summary>
        /// intilizes the class to specs of the given EncryptionMode
        /// </summary>
        /// <param name="newKey"> key to set</param>
        /// <param name="newSeed">seed to set</param>
        /// <param name="newMode">type of encryption to be used for</param>
        public Pair(string newKey, string newSeed, EncryptionMode newMode)
        {
            mode = newMode;
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
            else if (mode == EncryptionMode.Des)
            {
                validSize = DesKeySize;
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
            else if (mode == EncryptionMode.Des)
            {
                setKey(parseString(newKey,DesKeySize));
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
            else if (mode == EncryptionMode.Des)
            {
                validSize = DesSeedSize;
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
            else if (mode == EncryptionMode.Des)
            {
                setSeed(parseString(newSeed, DesSeedSize));
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
            for (int index = 0; index < validArray.Length; index++)
            {
                if (index + 2 < strLen)
                {
                    validArray[index] = Convert.ToByte(unparsedStr.Substring(index, 3));
                }
                else if (index < strLen && index + 2 > strLen)
                {
                    string temp = unparsedStr.Substring(index);
                    temp = temp.Length == 1 ? "00" + temp : "0" + temp;
                    validArray[index] = Convert.ToByte(temp);
                }
                if (index >= strLen && index < validSize)
                {
                    throw new FormatException("given string is of invalid size");
                }
            }
            return seed;
        }
        /// <summary>
        /// function parses given string into a valid size byte array
        /// </summary>
        /// <param name="unparsedStr">string to parse</param>
        /// <<param name="isKey"> true if parse if for a key</param>
        /// <param name="type"> type of encryption it will be used for</param>
        /// <returns></returns>
        public byte[] parseString(string unparsedStr, EncryptionMode type, bool isKey)
        {
            int validSize = 0;
            if (isKey)
            {
                validSize = type == EncryptionMode.Des ? DesKeySize : RijdaelKeySize;
            }
            else
            {
                validSize = type == EncryptionMode.Des ? DesSeedSize : RijdaelSeedSize;
            }
            int strLen = unparsedStr.Length;
            byte[] validArray = new byte[validSize];
            for (int index = 0; index < validArray.Length; index++)
            {
                if (index + 2 < strLen)
                {
                    validArray[index] = Convert.ToByte(unparsedStr.Substring(index, 3));
                }
                else if (index < strLen && index + 2 > strLen)
                {
                    string temp = unparsedStr.Substring(index);
                    temp = temp.Length == 1 ? "00" + temp : "0" + temp;
                    validArray[index] = Convert.ToByte(temp);
                }
                if (index >= strLen && index < validSize)
                {
                    break;
                }
            }
            return seed;
        }
        #endregion

    }
}
