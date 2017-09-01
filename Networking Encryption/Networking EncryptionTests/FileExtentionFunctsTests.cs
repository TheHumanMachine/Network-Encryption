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
    public class FileExtFunctsTests
    {
        [TestMethod()]
        public void checkExtentionSameExtention()
        {
           Assert.IsTrue(FileExtFuncts.checkExtention(Files.DecryptedGifOne, Files.DecryptedGifTwo));
        }
        [TestMethod()]
        public void checkExtentionDifExtention()
        {
            Assert.IsFalse(FileExtFuncts.checkExtention(Files.DecryptedGifTwo, Files.DecryptedJpegOne));
        }
        [TestMethod()]
        public void checkExtentionNoExtention()
        {
            Assert.IsFalse(FileExtFuncts.checkExtention(Files.DecryptedJpegOne,"this is a test"));
        }
        [TestMethod()]
        public void getExtentionHasExtentionTest()
        {
            Assert.AreEqual(".txt", FileExtFuncts.getExtention(Files.DecryptedTextOne));
        }
        [TestMethod()]
        public void getExtentionNoExtentionTest()
        {
            Assert.ThrowsException<FormatException>(() => FileExtFuncts.getExtention("this is a test"));
        }
        [TestMethod()]
        public void checkHasExtentionHasExtention()
        {
            Assert.IsTrue(FileExtFuncts.checkHasExtention(Files.DecryptedGifOne));
        }
        [TestMethod()]
        public void checkHasExtentionNoExtentionTest()
        {
            Assert.IsFalse(FileExtFuncts.checkHasExtention("this is a test"));
        }
        [TestMethod()]
        public void prependFileNeedsPrepend()
        {
            string root = "..\\";
            string file = "test.txt";
            string a = FileExtFuncts.prependFile((root + file));
            Assert.AreEqual(file, FileExtFuncts.prependFile((root + file)));
        }
        [TestMethod()]
        public void prependFileNoPrependNeeded()
        {
            string file = "test.txt";
            Assert.AreEqual(file, FileExtFuncts.prependFile(file));
        }
        [TestMethod()]
        public void removePathsRemoveTest()
        {
            string path = "..\\..\\EncryptionTestFiles\\DecryptedGifOne.gif";
            string file = "DecryptedGifOne.gif";
            Assert.AreEqual(file, FileExtFuncts.removePaths(path));
        }
        [TestMethod()]
        public void removePathsMultRemovesTest()
        {
            string path = "..\\..\\EncryptionTestFiles\\EncryptionTestFiles\\DecryptedGifOne.gif";
            string file = "DecryptedGifOne.gif";
            Assert.AreEqual(file, FileExtFuncts.removePaths(path));
        }
        [TestMethod()]
        public void removePathsNoRemoves()
        {
            string file = "DecryptedGifOne.gif";
            Assert.AreEqual(file, FileExtFuncts.removePaths(file));
        }
    }
}