﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        const int RijdaelKeySize = 32;
        const int RijdaelSeedSize = 16;
        const int AesKeySize = 32;
        const int AesSeedSize = 16;

        [TestMethod()]
        public void PairByteCTOR_ForDes()
        {
            byte[] key = Enumerable.Repeat((byte)55,AesKeySize).ToArray();
            byte[] seed = Enumerable.Repeat((byte)60, AesSeedSize).ToArray();
            Pair test = new Pair(key, seed, EncryptionMode.Aes, 0);
            Assert.AreEqual(EncryptionMode.Aes, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
            Assert.AreEqual(0, test.Length);
        }
        [TestMethod()]
        public void PairByteCTOR_ForRijdael()
        {
            byte[] key = Enumerable.Repeat((byte)55, RijdaelKeySize).ToArray();
            byte[] seed = Enumerable.Repeat((byte)60, RijdaelSeedSize).ToArray();
            Pair test = new Pair(key, seed, EncryptionMode.RijDanael, 0);
            Assert.AreEqual(EncryptionMode.RijDanael, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
            Assert.AreEqual(0, test.Length);
        }
        [TestMethod()]
        public void PairStringCTOR_ForDes()
        {
            string key = string.Join("", Enumerable.Repeat("123", AesKeySize).ToArray());
            string seed = string.Join("", Enumerable.Repeat("136", AesSeedSize).ToArray());
            Pair test = new Pair(key, seed, EncryptionMode.Aes, 0);
            Assert.AreEqual(EncryptionMode.Aes, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
            Assert.AreEqual(0, test.Length);
        }
        [TestMethod()]
        public void PairStringCTOR_ForRijdael()
        {
            string key = string.Join("", Enumerable.Repeat((byte)125, RijdaelKeySize).ToArray());
            string seed = string.Join("", Enumerable.Repeat((byte)135, RijdaelSeedSize).ToArray());
            Pair test = new Pair(key, seed, EncryptionMode.RijDanael, 0);
            Assert.AreEqual(EncryptionMode.RijDanael, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
            Assert.AreEqual(0, test.Length);
        }
        [TestMethod()]
        public void setKeyByteTest()
        {
            Pair test = new Pair();
            byte[] key = Enumerable.Repeat((byte)55, AesKeySize).ToArray();
            test.Mode = EncryptionMode.Aes;
            Assert.AreEqual(EncryptionMode.Aes, test.Mode);
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        public void setKeyByteNullExceptionTest()
        {
            Pair test = new Pair();
            Assert.ThrowsException<KeyNotFoundException>(() => test.setKey(Enumerable.Repeat((byte)55, RijdaelKeySize).ToArray()));
        }
        [TestMethod()]
        public void setKeyByteArgumentExceptionTest()
        {
            Pair test = new Pair();
            test.Mode = EncryptionMode.RijDanael;
            Assert.ThrowsException<ArgumentException>(() => test.setKey(Enumerable.Repeat((byte)55, 10).ToArray()));
        }
        public void setStringByteTest()
        {
            Pair test = new Pair();
            string key = string.Join("", Enumerable.Repeat((byte)55, RijdaelKeySize).ToArray());
            test.Mode = EncryptionMode.Aes;
            Assert.AreEqual(EncryptionMode.Aes, test.Mode);
            test.setKey(key);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
        }
        [TestMethod()]
        public void setKeyStringNullExceptionTest()
        {
            Pair test = new Pair();
            Assert.ThrowsException<KeyNotFoundException>(() => test.setKey(string.Join("", Enumerable.Repeat((byte)55, RijdaelKeySize).ToArray())));
        }
        [TestMethod()]
        public void setKeyStringFormatExceptionTest()
        {
            Pair test = new Pair();
            test.Mode = EncryptionMode.RijDanael;
            Assert.ThrowsException<FormatException>(() => test.setKey(string.Join("", Enumerable.Repeat((byte)55, 10).ToArray())));
        }
        [TestMethod()]
        public void setSeedByteTest()
        {
            Pair test = new Pair();
            byte[] seed = Enumerable.Repeat((byte)55, AesSeedSize).ToArray();
            test.Mode = EncryptionMode.Aes;
            Assert.AreEqual(EncryptionMode.Aes, test.Mode);
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void setSeedByteNullExceptionTest()
        {
            Pair test = new Pair();
            Assert.ThrowsException<KeyNotFoundException>(() => test.setSeed(Enumerable.Repeat((byte)55, AesSeedSize).ToArray()));
        }
        [TestMethod()]
        public void setSeedByteArgumentExceptionTest()
        {
            Pair test = new Pair();
            test.Mode = EncryptionMode.RijDanael;
            Assert.ThrowsException<ArgumentException>(() => test.setSeed(Enumerable.Repeat((byte)55, 10).ToArray()));
        }
        public void setSeedStringTest()
        {
            Pair test = new Pair();
            string seed = string.Join("", Enumerable.Repeat((byte)55, AesSeedSize).ToArray());
            test.Mode = EncryptionMode.Aes;
            Assert.AreEqual(EncryptionMode.Aes, test.Mode);
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Key));
        }
        [TestMethod()]
        public void setSeedStringNullExceptionTest()
        {
            Pair test = new Pair();
            Assert.ThrowsException<KeyNotFoundException>(() => test.setKey(string.Join("", Enumerable.Repeat((byte)155, AesSeedSize).ToArray())));
        }
        [TestMethod()]
        public void setSeedStringArgumentExceptionTest()
        {
            Pair test = new Pair();
            test.Mode = EncryptionMode.RijDanael;
            Assert.ThrowsException<ArgumentException>(() => test.setSeed(string.Join("", Enumerable.Repeat((byte)155, 10).ToArray())));
        }
        [TestMethod()]
        public void parseStringArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => Pair.parseString(string.Join("", Enumerable.Repeat((byte)155, 5).ToArray()),EncryptionMode.Aes,false));
        }
        [TestMethod()]
        public void parseStringDesIsKey()
        {
            byte[] key = Enumerable.Repeat((byte)155, AesKeySize).ToArray();
            Assert.AreEqual(string.Join("", key), string.Join("", Pair.parseString(string.Join("", key), EncryptionMode.Aes, true)));
        }
        [TestMethod()]
        public void parseStringRijdaelIsKey()
        {
            byte[] key = Enumerable.Repeat((byte)155, RijdaelKeySize).ToArray();
            Assert.AreEqual(string.Join("",key), string.Join("", Pair.parseString(string.Join("", key), EncryptionMode.RijDanael, true)));
        }
        [TestMethod()]
        public void parseStringDesNotKey()
        {
            byte[] seed = Enumerable.Repeat((byte)155, AesSeedSize).ToArray();
            Assert.AreEqual(string.Join("",seed),string.Join("", Pair.parseString(string.Join("", seed), EncryptionMode.Aes, false)));
        }
        [TestMethod()]
        public void parseStringRijdaelNotKey()
        {
            byte[] seed = Enumerable.Repeat((byte)155, RijdaelSeedSize).ToArray();
            Pair.parseString(string.Join("", seed), EncryptionMode.RijDanael, false);
            Assert.AreEqual(string.Join("", seed), string.Join("", Pair.parseString(string.Join("", seed), EncryptionMode.RijDanael, false)));
        }
    }
}