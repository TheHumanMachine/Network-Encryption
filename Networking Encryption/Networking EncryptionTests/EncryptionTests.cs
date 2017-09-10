using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
using Networking_EncryptionTests.Properties;

/* stopped at line 580
 * add a function for path parse
 * add assertions for the pair class
 * fix string encryption with the pair class
 */ 
namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class EncryptionTests
    {
        [TestMethod()]
        public void compareFileAreEqual()
        {
            string fileOne = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            string fileTwo = Directory.GetParent(Files.TextToEncryptTwo).FullName + Files.TextToEncryptTwo;
            Encryption encryptor = new Encryption();
            Assert.IsTrue(encryptor.compareFile(@fileOne, @fileTwo), "Test: 1");
        }
        [TestMethod()]
        public void compareFileSameFile()
        {
            string fileOne = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            Encryption encryptor = new Encryption();
            Assert.IsTrue(encryptor.compareFile(fileOne, fileOne), "Test: 1");
        }
        public void compareFileNotEqual()
        {
            string fileOne = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            string fileTwo = Directory.GetParent(Files.DecryptedTextOne).FullName + Files.DecryptedTextOne;
            Encryption encryptor = new Encryption();
            Assert.IsFalse(encryptor.compareFile(fileOne, fileTwo), "Test: 1");
        }
        [TestMethod()]
        public void EncryptStringTest()
        {
            string word = "this is a test";
            Encryption encryptor = new Encryption();
            Pair keys = null;
            string cipherText = encryptor.EncryptStr(word,ref keys);
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.RijDanael, keys.Mode);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
        }
        [TestMethod()]
        public void EncryptStringSameSeedTest()
        {
            string word = "this is a test";
            string seed = "1";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            Pair keyOne = null;
            Pair keyTwo = null;
            string cipherText = encryptor1.EncryptStr(word,ref keyOne, seed);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, encryptor2.EncryptStr(word,ref keyTwo, seed), "Test: 3");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveDestination = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1,seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1),"test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptPdfTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveDestination = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void EncryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PngToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptOne);
            string saveDestination = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void EncryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveDestination = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void EncryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveDestination = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void EncryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptStringTest()
        {
            string word = "this is a test";
            Encryption encryptor = new Encryption();
            Pair keys = null;
            string cipherText = encryptor.EncryptStr(word,ref keys);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(word, encryptor.Decrypt(cipherText,keys), "Test: 3");
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.RijDanael, keys.Mode);
        }
        [TestMethod()]
        public void DecryptStringSameSeedTest()
        {
            string word = "this is a test";
            string seed = "1";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            Pair keyOne = null;
            Pair keyTwo = null;
            string cipherText = encryptor1.EncryptStr(word,ref keyOne, seed);
            string cipherText2 = encryptor2.EncryptStr(word,ref keyTwo, seed);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, cipherText2, "Test: 3");
            string newText = encryptor1.Decrypt(cipherText, keyOne);
            string newText2 = encryptor2.Decrypt(cipherText2, keyTwo);
            Assert.AreEqual(word, newText, "test 4");
            Assert.AreEqual(word, newText2, "test 5");
            Assert.AreEqual(newText, newText2, "test 6");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveEncryption = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string saveDecryption = Directory.GetParent(Files.DecryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
            encryptor.Decrypt(saveEncryption, saveDecryption, keys);
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveDecryption, saveEncryption), "test 3");
        }
        [TestMethod()]
        public void DecryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1,saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptPdfTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveEncryption = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string savedecryption = Directory.GetParent(Files.DecryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption), "test 1");
            encryptor.Decrypt(saveEncryption, savedecryption, keys);
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, savedecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveEncryption, savedecryption), "test 3");
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void DecryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgPngTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PngToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptOne);
            string saveEncryption = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string saveDecryption = Directory.GetParent(Files.DecryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            encryptor.Decrypt(saveEncryption, saveDecryption, keys);
            Assert.IsFalse(encryptor.compareFile(saveDecryption,saveEncryption),"test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 3");
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void DecryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgJpegTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveEncryption = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string saveDecryption = Directory.GetParent(Files.DecryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsFalse(encryptor.compareFile(saveEncryption, saveDecryption), "test 2");
            Assert.IsTrue(encryptor.compareFile(saveDecryption, fileToEncrypt), "test 3");
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
            checkPairNotNull(keys);
        }
        [TestMethod()]
        public void DecryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkSameSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgGifTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveEncryption = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string saveDecryption = Directory.GetParent(Files.DecryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifOne);
            Encryption encryptor = new Encryption();
            Pair keys = encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveDecryption, saveEncryption), "test 3");
            checkPairNotNull(keys);
            Assert.AreEqual(EncryptionMode.Des, keys.Mode);
        }
        [TestMethod()]
        public void DecryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifTwo);
            string seed = "1";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkSameSeed(keyOne, keyTwo);
        }
        //static String prependFile()
        [TestMethod()]
        public void testResourceLocations()
        {
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedGifOne).FullName) + "\\" +  FileExtFuncts.removePaths(Files.DecryptedGifOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedGifTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedJpegOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedJpegTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedPdfOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedPdfTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedPngOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedPngTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedTextOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.DecryptedTextTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedGifOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedGifTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedJpegOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedJpegTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedPdfOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedPdfTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedPngOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedPngTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedTextOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.EncryptedTextTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.GifToEncryptOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.GifToEncryptTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.JpegToEncryptOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.JpegToEncryptTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.PdfToEncryptOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.PdfToEncryptTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.PngToEncryptOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.PngToEncryptTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.TextToEncryptOne).FullName) + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne)));
            Assert.IsTrue(File.Exists((Directory.GetParent(Files.TextToEncryptTwo).FullName) + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo)));
        }
        [TestMethod()]
        public void EncryptStringDifSeedTest()
        {
            string word = "this is a test";
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            Pair keyOne = null;
            Pair keyTwo = null;
            string cipherText = encryptor1.EncryptStr(word,ref keyOne, seedOne);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreNotEqual(cipherText, encryptor2.EncryptStr(word,ref keyTwo, seedTwo), "Test: 3");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptPdfDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        public void DecryptStringDifSeedTest()
        {
            string word = "this is a test";
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            Pair keyOne = null;
            Pair keyTwo = null;
            string cipherText = encryptor1.EncryptStr(word,ref keyOne, seedOne);
            string cipherText2 = encryptor2.EncryptStr(word,ref keyTwo, seedTwo);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, cipherText2, "Test: 3");
            string newText = encryptor1.Decrypt(cipherText, keyOne);
            string newText2 = encryptor2.Decrypt(cipherText2, keyTwo);
            Assert.AreEqual(word, newText, "test 4");
            Assert.AreEqual(word, newText2, "test 5");
            Assert.AreEqual(newText, newText2, "test 6");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptPdfDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgPngDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgJpegDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgGifDifSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifTwo);
            string seedOne = "1";
            string seedTwo = "2";
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seedOne);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seedTwo);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptStringDifAlgoTest()
        {
            string word = "this is a test";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            Pair keyOne = null;
            Pair keyTwo = null;
            string cipherText = encryptor1.EncryptStr(word,ref keyOne);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreNotEqual(cipherText, encryptor2.EncryptStr(word,ref keyTwo), "Test: 3");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptTxtFileDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptPdfDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgPngDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne  = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgJpegDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void EncryptImgGifDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveDestination1 = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo);
            string saveDestination2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveDestination1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveDestination2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
            checkDifSeed(keyOne, keyTwo);
        }
        public void DecryptStringDifAlgoTest()
        {
            string word = "this is a test";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            Pair keyOne = null;
            Pair keyTwo = null;
            string cipherText = encryptor1.EncryptStr(word,ref keyOne);
            string cipherText2 = encryptor2.EncryptStr(word,ref keyTwo);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, cipherText2, "Test: 3");
            string newText = encryptor1.Decrypt(cipherText, keyOne);
            string newText2 = encryptor2.Decrypt(cipherText2, keyTwo);
            Assert.AreEqual(word, newText, "test 4");
            Assert.AreEqual(word, newText2, "test 5");
            Assert.AreEqual(newText, newText2, "test 6");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptTxtFileDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextOne);
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.TextToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedTextTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedTextOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedTextTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedTextTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptPdfDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PdfToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPdfTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPdfOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPdfTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPdfTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgPngDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngOne);
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.PngToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedPngTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPngOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPngTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedPngTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgJpegDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegOne);
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.JpegToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedJpegTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedJpegOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedJpegTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedJpegTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        [TestMethod()]
        public void DecryptImgGifDifAlgoTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptOne);
            string saveEncryption1 = Directory.GetParent(Files.EncryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifOne);
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.GifToEncryptTwo);
            string saveEncryption2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.EncryptedGifTwo);
            string saveDecryption1 = Directory.GetParent(Files.DecryptedGifOne).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifOne);
            string saveDecryption2 = Directory.GetParent(Files.DecryptedGifTwo).FullName + "\\" + FileExtFuncts.removePaths(Files.DecryptedGifTwo);
            Encryption encryptor = new Encryption();
            Pair keyOne = encryptor.Encrypt(fileToEncrypt1, saveEncryption1);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            Pair keyTwo = encryptor.Encrypt(fileToEncrypt2, saveEncryption2);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1, keyOne);
            encryptor.Decrypt(saveEncryption2, saveDecryption2, keyTwo);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
            checkDifSeed(keyOne, keyTwo);
        }
        /// <summary>
        /// asserts that the given class is not null referenced
        /// </summary>
        /// <param name="keys"></param>
        static void checkPairNotNull(Pair keys)
        {
            Assert.IsNotNull(keys);
            Assert.IsNotNull(keys.Key);
            Assert.IsNotNull(keys.Seed);
            Assert.AreNotEqual(EncryptionMode.Null, keys.Mode);
        }
        /// <summary>
        /// function asserts the following: that the given pairs are not null, encryption mode and seeds are equal and that the keys are diferent
        /// </summary>
        /// <param name="keyOne">first pair to check</param>
        /// <param name="keyTwo">second pair to check</param>
        static void checkSameSeed(Pair keyOne, Pair keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreEqual(EncryptionMode.Des, keyOne.Mode);
            Assert.AreEqual(keyOne.Mode, keyTwo.Mode);
            Assert.AreNotEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key));
            Assert.AreEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed));
        }
        /// <summary>
        /// function asserts the following: given pairs not null,encryption modes are all equal and that seed and key are different.
        /// </summary>
        /// <param name="keyOne">first pair to check</param>
        /// <param name="keyTwo">second pair to check</param>
        static void checkDifSeed(Pair keyOne, Pair keyTwo)
        {
            checkPairNotNull(keyOne);
            checkPairNotNull(keyTwo);
            Assert.AreEqual(EncryptionMode.Des, keyOne.Mode);
            Assert.AreEqual(keyOne.Mode, keyTwo.Mode);
            Assert.AreNotEqual(string.Join("", keyOne.Key), string.Join("", keyTwo.Key));
            Assert.AreNotEqual(string.Join("", keyOne.Seed), string.Join("", keyTwo.Seed));
        }
    }
}