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
        const int RijdaelKeySize = 48;
        const int RijdaelSeedSize = 16;
        const int DesKeySize = 24;
        const int DesSeedSize = 8;

        [TestMethod()]
        public void PairByteCTOR_ForDes()
        {
            byte[] key = Enumerable.Repeat((byte)55,DesKeySize).ToArray();
            byte[] seed = Enumerable.Repeat((byte)60, DesSeedSize).ToArray();
            Pair test = new Pair(key, seed, EncryptionMode.Des);
            Assert.AreEqual(EncryptionMode.Des, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void PairByteCTOR_ForRijdael()
        {
            byte[] key = Enumerable.Repeat((byte)55, RijdaelKeySize).ToArray();
            byte[] seed = Enumerable.Repeat((byte)60, RijdaelSeedSize).ToArray();
            Pair test = new Pair(key, seed, EncryptionMode.RijDanael);
            Assert.AreEqual(EncryptionMode.RijDanael, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void PairStringCTOR_ForDes()
        {
            string key = string.Join("", Enumerable.Repeat("123", DesKeySize).ToArray());
            string seed = string.Join("", Enumerable.Repeat("136", DesSeedSize).ToArray());
            Pair test = new Pair(key, seed, EncryptionMode.Des);
            Assert.AreEqual(EncryptionMode.Des, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void PairStringCTOR_ForRijdael()
        {
            string key = string.Join("", Enumerable.Repeat((byte)125, RijdaelKeySize).ToArray());
            string seed = string.Join("", Enumerable.Repeat((byte)135, RijdaelSeedSize).ToArray());
            Pair test = new Pair(key, seed, EncryptionMode.RijDanael);
            Assert.AreEqual(EncryptionMode.RijDanael, test.Mode);
            Assert.AreEqual(string.Join("", key), string.Join("", test.Key));
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void setKeyByteTest()
        {
            Pair test = new Pair();
            byte[] key = Enumerable.Repeat((byte)55, DesKeySize).ToArray();
            test.Mode = EncryptionMode.Des;
            Assert.AreEqual(EncryptionMode.Des, test.Mode);
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
            test.Mode = EncryptionMode.Des;
            Assert.AreEqual(EncryptionMode.Des, test.Mode);
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
        public void setKeyStringArgumentExceptionTest()
        {
            Pair test = new Pair();
            test.Mode = EncryptionMode.RijDanael;
            Assert.ThrowsException<ArgumentException>(() => test.setKey(string.Join("", Enumerable.Repeat((byte)55, 10).ToArray())));
        }
        [TestMethod()]
        public void setSeedByteTest()
        {
            Pair test = new Pair();
            byte[] seed = Enumerable.Repeat((byte)55, DesSeedSize).ToArray();
            test.Mode = EncryptionMode.Des;
            Assert.AreEqual(EncryptionMode.Des, test.Mode);
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Seed));
        }
        [TestMethod()]
        public void setSeedByteNullExceptionTest()
        {
            Pair test = new Pair();
            Assert.ThrowsException<KeyNotFoundException>(() => test.setSeed(Enumerable.Repeat((byte)55, DesSeedSize).ToArray()));
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
            string seed = string.Join("", Enumerable.Repeat((byte)55, DesSeedSize).ToArray());
            test.Mode = EncryptionMode.Des;
            Assert.AreEqual(EncryptionMode.Des, test.Mode);
            test.setSeed(seed);
            Assert.AreEqual(string.Join("", seed), string.Join("", test.Key));
        }
        [TestMethod()]
        public void setSeedStringNullExceptionTest()
        {
            Pair test = new Pair();
            Assert.ThrowsException<KeyNotFoundException>(() => test.setKey(string.Join("", Enumerable.Repeat((byte)155, DesSeedSize).ToArray())));
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
            Assert.ThrowsException<ArgumentException>(() => Pair.parseString(string.Join("", Enumerable.Repeat((byte)155, 5).ToArray()),EncryptionMode.Des,false));
        }
        [TestMethod()]
        public void parseStringDesIsKey()
        {
            byte[] key = Enumerable.Repeat((byte)155, DesKeySize).ToArray();
            Assert.AreEqual(string.Join("", key), string.Join("", Pair.parseString(string.Join("", key), EncryptionMode.Des, true)));
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
            byte[] seed = Enumerable.Repeat((byte)155, DesSeedSize).ToArray();
            Assert.AreEqual(string.Join("",seed),string.Join("", Pair.parseString(string.Join("", seed), EncryptionMode.Des, false)));
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