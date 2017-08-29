using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking_EncryptionTests.Properties;

namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class FileExtentionFunctsTests
    {
        [TestMethod()]
        public void checkExtentionSameExtention()
        {
           Assert.IsTrue(FileExtentionFuncts.checkExtention(Files.DecryptedGifOne, Files.DecryptedGifTwo));
        }
        [TestMethod()]
        public void checkExtentionDifExtention()
        {
            Assert.IsFalse(FileExtentionFuncts.checkExtention(Files.DecryptedGifTwo, Files.DecryptedJpegOne));
        }
        [TestMethod()]
        public void checkExtentionNoExtention()
        {
            Assert.IsFalse(FileExtentionFuncts.checkExtention(Files.DecryptedJpegOne,"this is a test"));
        }
        [TestMethod()]
        public void getExtentionHasExtentionTest()
        {
            Assert.AreEqual(".txt", FileExtentionFuncts.getExtention(Files.DecryptedTextOne));
        }
        [TestMethod()]
        public void getExtentionNoExtentionTest()
        {
            Assert.ThrowsException<FormatException>(() => FileExtentionFuncts.getExtention("this is a test"));
        }
        [TestMethod()]
        public void checkHasExtentionHasExtention()
        {
            Assert.IsTrue(FileExtentionFuncts.checkHasExtention(Files.DecryptedGifOne));
        }
        [TestMethod()]
        public void checkHasExtentionNoExtentionTest()
        {
            Assert.IsFalse(FileExtentionFuncts.checkHasExtention("this is a test"));
        }
        [TestMethod()]
        public void prependFileNeedsPrepend()
        {
            string root = "..\\";
            string file = "test.txt";
            string a = FileExtentionFuncts.prependFile((root + file));
            Assert.AreEqual(file, FileExtentionFuncts.prependFile((root + file)));
        }
        [TestMethod()]
        public void prependFileNoPrependNeeded()
        {
            string file = "test.txt";
            Assert.AreEqual(file, FileExtentionFuncts.prependFile(file));
        }
    }
}