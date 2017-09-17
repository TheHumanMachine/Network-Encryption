using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class PairTests
    {
        const int KeyLen = 32;
        const int SeedLen = 16;

        [TestMethod()]
        public void KeyHolderByteCTOR()
        {
            byte[] key = Enumerable.Repeat((byte)55, KeyLen).ToArray();
            byte[] seed = Enumerable.Repeat((byte)60, SeedLen).ToArray();
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
        [TestMethod()]
        public void setKeyByteTest()
        {
            KeyHolder test = new KeyHolder();
            byte[] key = Enumerable.Repeat((byte)55, KeyLen).ToArray();
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        public void setKeyStringByteTest()
        {
            KeyHolder test = new KeyHolder();
            string key = string.Join("", Enumerable.Repeat((byte)55, KeyLen).ToArray());
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        public void setSeedByteTest()
        {
            KeyHolder test = new KeyHolder();
            byte[] seed = Enumerable.Repeat((byte)55, SeedLen).ToArray();
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        public void setSeedStringTest()
        {
            KeyHolder test = new KeyHolder();
            string seed = string.Join("", Enumerable.Repeat((byte)55, SeedLen).ToArray());
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Key));
        }

        #region Exception is Thrown Tests

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
            Assert.ThrowsException<OutOfDomainException>(() => test.setSeed(string.Join("", Enumerable.Repeat(555, SeedLen).ToArray())));
        }
        [TestMethod()]
        public void setSeedByteInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<InvalidLengthException>(() => test.setSeed(Enumerable.Repeat((byte)55, 10).ToArray()));
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
            Assert.ThrowsException<OutOfDomainException>(() => test.setKey(string.Join("", Enumerable.Repeat(555, SeedLen).ToArray())));
        }
        [TestMethod()]
        public void setKeyByteInvalidLenExceptionTest()
        {
            KeyHolder test = new KeyHolder();
            Assert.ThrowsException<InvalidLengthException>(() => test.setKey(Enumerable.Repeat((byte)55, 10).ToArray()));
        }
        [TestMethod()]
        public void parseStrIsKeyOutOfDomainTest()
        {
            Assert.ThrowsException<OutOfDomainException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat(555, KeyLen).ToArray()), true));
        }
        [TestMethod()]
        public void parseStrIsKeyInvalidLenTest()
        {
            Assert.ThrowsException<InvalidLengthException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, 3).ToArray()), true));
        }
        [TestMethod()]
        public void parseStrNotKeyOutOfDomainTest()
        {
            Assert.ThrowsException<OutOfDomainException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat(555, SeedLen).ToArray()), false));
        }
        [TestMethod()]
        public void parseStrNotKeyInvalidLenTest()
        {
            Assert.ThrowsException<InvalidLengthException>(() => KeyHolder.parseString(string.Join("", Enumerable.Repeat((byte)155, 3).ToArray()), false));
        }
        #endregion
    }
}