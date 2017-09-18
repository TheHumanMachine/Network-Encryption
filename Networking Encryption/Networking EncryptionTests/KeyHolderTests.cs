using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * CodeMetrics: 74  32  1   16  56
 */
namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class KeyHolderTest
    {
        #region Test Constants
        const int KeyLen = 32;
        const int SeedLen = 16;
        const byte TestNum = 55;
        const int invalidNum = 55;
        #endregion

        #region CTOR Unit Tests
        [TestMethod()]
        public void KeyHolderDefaultCTOR()
        {
            KeyHolder test = new KeyHolder();
            Assert.IsNull(test.Key);
            Assert.IsNull(test.Seed);
        }
        [TestMethod()]
        public void KeyHolderByteCTOR()
        {
            byte[] key = Enumerable.Repeat(TestNum, KeyLen).ToArray();
            byte[] seed = Enumerable.Repeat((byte)(TestNum + 5), SeedLen).ToArray();
            KeyHolder test = new KeyHolder(key, seed);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void KeyHolderStringCTOR()
        {
            string key = string.Join("", Enumerable.Repeat("123", KeyLen).ToArray());
            string seed = string.Join("", Enumerable.Repeat("136", SeedLen).ToArray());
            KeyHolder test = new KeyHolder(key, seed);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        #endregion

        #region SetFunctions Unit Tests
        [TestMethod()]
        public void setKeyByteTest()
        {
            KeyHolder test = new KeyHolder();
            byte[] key = Enumerable.Repeat(TestNum, KeyLen).ToArray();
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        public void setKeyStringByteTest()
        {
            KeyHolder test = new KeyHolder();
            string key = string.Join("", Enumerable.Repeat(TestNum, KeyLen).ToArray());
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        public void setSeedByteTest()
        {
            KeyHolder test = new KeyHolder();
            byte[] seed = Enumerable.Repeat(TestNum, SeedLen).ToArray();
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        public void setSeedStringTest()
        {
            KeyHolder test = new KeyHolder();
            string seed = string.Join("", Enumerable.Repeat(TestNum, SeedLen).ToArray());
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Key));
        }
        #endregion

        #region Exception is Thrown Unit Tests

        [TestMethod()]
        public void setSeedStringInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<InvalidLengthException>(() => test.setSeed(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray())));
        }
        [TestMethod()]
        public void setSeedStringOutOfDomainExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<OutOfDomainException>(() => test.setSeed(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray())));
        }
        [TestMethod()]
        public void setSeedByteInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<InvalidLengthException>(() => test.setSeed(Enumerable.Repeat(TestNum, (SeedLen - 5)).ToArray()));
        }
        [TestMethod()]
        public void setKeyStringInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<InvalidLengthException>(() => test.setKey(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray())));
        }
        [TestMethod()]
        public void setKeyStringOutOfDomianExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<OutOfDomainException>(() => test.setKey(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray())));
        }
        [TestMethod()]
        public void setKeyByteInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<InvalidLengthException>(() => test.setKey(Enumerable.Repeat(TestNum, (KeyLen - 5)).ToArray()));
        }
        [TestMethod()]
        public void parseStrIsKeyOutOfDomainTest()
        {
            Assert.ThrowsException<OutOfDomainException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat(invalidNum, KeyLen).ToArray()), true));
        }
        [TestMethod()]
        public void parseStrIsKeyInvalidLenTest()
        {
            Assert.ThrowsException<InvalidLengthException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, (KeyLen - 3)).ToArray()), true));
        }
        [TestMethod()]
        public void parseStrNotKeyOutOfDomainTest()
        {
            Assert.ThrowsException<OutOfDomainException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat(invalidNum, SeedLen).ToArray()), false));
        }
        [TestMethod()]
        public void parseStrNotKeyInvalidLenTest()
        {
            Assert.ThrowsException<InvalidLengthException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, (SeedLen - 5)).ToArray()), false));
        }
        #endregion
    }
}