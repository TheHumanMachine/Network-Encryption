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

/*Build:1.0.5
 * Date: 7/12/17
 * Code Metrics: 58 29  1   8   298
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
            string cipherText = encryptor.EncryptStr(word);
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
            string cipherText = encryptor1.EncryptStr(word,seed);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, encryptor2.EncryptStr(word, seed), "Test: 3");
        }
        [TestMethod()]
        public void EncryptTxtFileTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            string saveDestination = Directory.GetParent(Files.EncryptedTextOne).FullName + Files.EncryptedTextOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedTextOne).FullName + Files.EncryptedTextOne;
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + Files.TextToEncryptTwo;
            string saveDestination2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + Files.EncryptedTextTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveDestination1,seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1),"test 1");
            encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
        }
        [TestMethod()]
        public void EncryptPdfTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PdfToEncryptOne).FullName + Files.PdfToEncryptOne;
            string saveDestination = Directory.GetParent(Files.EncryptedPdfOne).FullName + Files.EncryptedPdfOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + Files.PdfToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + Files.EncryptedPdfOne;
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + Files.PdfToEncryptTwo;
            string saveDestination2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + Files.EncryptedPdfTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
        }
        [TestMethod()]
        public void EncryptImgPngTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveDestination = Directory.GetParent(Files.EncryptedPngOne).FullName + Files.EncryptedPngOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedPngOne).FullName + Files.EncryptedPngOne;
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + Files.PngToEncryptTwo;
            string saveDestination2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + Files.EncryptedPngTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
        }
        [TestMethod()]
        public void EncryptImgJpegTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.JpegToEncryptOne).FullName + Files.JpegToEncryptOne;
            string saveDestination = Directory.GetParent(Files.EncryptedJpegOne).FullName + Files.EncryptedJpegOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + Files.JpegToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + Files.EncryptedJpegOne;
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + Files.JpegToEncryptTwo;
            string saveDestination2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + Files.EncryptedJpegTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
        }
        [TestMethod()]
        public void EncryptImgGifTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.GifToEncryptOne).FullName + Files.GifToEncryptOne;
            string saveDestination = Directory.GetParent(Files.EncryptedGifOne).FullName + Files.EncryptedGifOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + Files.GifToEncryptOne;
            string saveDestination1 = Directory.GetParent(Files.EncryptedGifOne).FullName + Files.EncryptedGifOne;
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + Files.GifToEncryptTwo;
            string saveDestination2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + Files.EncryptedGifTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveDestination1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveDestination1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveDestination2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveDestination2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveDestination1, saveDestination2), "test 4");
        }
        [TestMethod()]
        public void DecryptStringTest()
        {
            string word = "this is a test";
            Encryption encryptor = new Encryption();
            string cipherText = encryptor.EncryptStr(word);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(word, encryptor.Decrypt(cipherText), "Test: 3");
        }
        [TestMethod()]
        public void DecryptStringSameSeedTest()
        {
            string word = "this is a test";
            string seed = "1";
            Encryption encryptor1 = new Encryption();
            Encryption encryptor2 = new Encryption();
            string cipherText = encryptor1.EncryptStr(word, seed);
            string cipherText2 = encryptor2.EncryptStr(word, seed);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, cipherText2, "Test: 3");
            string newText = encryptor1.Decrypt(cipherText);
            string newText2 = encryptor2.Decrypt(cipherText2);
            Assert.AreEqual(word, newText, "test 4");
            Assert.AreEqual(word, newText2, "test 5");
            Assert.AreEqual(newText, newText2, "test 6");
        }
        [TestMethod()]
        public void DecryptTxtFileTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            string saveEncryption = Directory.GetParent(Files.EncryptedTextOne).FullName + Files.EncryptedTextOne;
            string saveDecryption = Directory.GetParent(Files.DecryptedTextOne).FullName + Files.DecryptedTextOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            encryptor.Decrypt(saveEncryption, saveDecryption);
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveDecryption, saveEncryption), "test 3");
        }
        [TestMethod()]
        public void DecryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.TextToEncryptOne).FullName + Files.TextToEncryptOne;
            string saveEncryption1 = Directory.GetParent(Files.EncryptedTextOne).FullName + Files.EncryptedTextOne;
            string fileToEncrypt2 = Directory.GetParent(Files.TextToEncryptTwo).FullName + Files.TextToEncryptTwo;
            string saveEncryption2 = Directory.GetParent(Files.EncryptedTextTwo).FullName + Files.EncryptedTextTwo;
            string saveDecryption1 = Directory.GetParent(Files.DecryptedTextOne).FullName + Files.DecryptedTextOne;
            string saveDecryption2 = Directory.GetParent(Files.DecryptedTextTwo).FullName + Files.DecryptedTextTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1,saveDecryption1);
            encryptor.Decrypt(saveEncryption2, saveDecryption2);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
        }
        [TestMethod()]
        public void DecryptPdfTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PdfToEncryptOne).FullName + Files.PdfToEncryptOne;
            string saveEncryption = Directory.GetParent(Files.EncryptedPdfOne).FullName + Files.EncryptedPdfOne;
            string savedecryption = Directory.GetParent(Files.DecryptedPdfOne).FullName + Files.DecryptedPdfOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption), "test 1");
            encryptor.Decrypt(saveEncryption, savedecryption);
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, savedecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveEncryption, savedecryption), "test 3");
        }
        [TestMethod()]
        public void DecryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PdfToEncryptOne).FullName + Files.PdfToEncryptOne;
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPdfOne).FullName + Files.EncryptedPdfOne;
            string fileToEncrypt2 = Directory.GetParent(Files.PdfToEncryptTwo).FullName + Files.PdfToEncryptTwo;
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPdfTwo).FullName + Files.EncryptedPdfTwo;
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPdfOne).FullName + Files.DecryptedPdfOne;
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPdfTwo).FullName + Files.DecryptedPdfTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1);
            encryptor.Decrypt(saveEncryption2, saveDecryption2);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
        }
        [TestMethod()]
        public void DecryptImgPngTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveEncryption = Directory.GetParent(Files.EncryptedPngOne).FullName + Files.EncryptedPngOne;
            string saveDecryption = Directory.GetParent(Files.DecryptedPngOne).FullName + Files.DecryptedPngOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            encryptor.Decrypt(saveEncryption, saveDecryption);
            Assert.IsFalse(encryptor.compareFile(saveDecryption,saveEncryption),"test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 3");
        }
        [TestMethod()]
        public void DecryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.PngToEncryptOne).FullName + Files.PngToEncryptOne;
            string saveEncryption1 = Directory.GetParent(Files.EncryptedPngOne).FullName + Files.EncryptedPngOne;
            string fileToEncrypt2 = Directory.GetParent(Files.PngToEncryptTwo).FullName + Files.PngToEncryptTwo;
            string saveEncryption2 = Directory.GetParent(Files.EncryptedPngTwo).FullName + Files.EncryptedPngTwo;
            string saveDecryption1 = Directory.GetParent(Files.DecryptedPngOne).FullName + Files.DecryptedPngOne;
            string saveDecryption2 = Directory.GetParent(Files.DecryptedPngTwo).FullName + Files.DecryptedPngTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1);
            encryptor.Decrypt(saveEncryption2, saveDecryption2);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
        }
        [TestMethod()]
        public void DecryptImgJpegTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.JpegToEncryptOne).FullName + Files.JpegToEncryptOne;
            string saveEncryption = Directory.GetParent(Files.EncryptedJpegOne).FullName + Files.EncryptedJpegOne;
            string saveDecryption = Directory.GetParent(Files.DecryptedJpegOne).FullName + Files.DecryptedJpegOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsFalse(encryptor.compareFile(saveEncryption, saveDecryption), "test 2");
            Assert.IsTrue(encryptor.compareFile(saveDecryption, fileToEncrypt), "test 3");
        }
        [TestMethod()]
        public void DecryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.JpegToEncryptOne).FullName + Files.JpegToEncryptOne;
            string saveEncryption1 = Directory.GetParent(Files.EncryptedJpegOne).FullName + Files.EncryptedJpegOne;
            string fileToEncrypt2 = Directory.GetParent(Files.JpegToEncryptTwo).FullName + Files.JpegToEncryptTwo;
            string saveEncryption2 = Directory.GetParent(Files.EncryptedJpegTwo).FullName + Files.EncryptedJpegTwo;
            string saveDecryption1 = Directory.GetParent(Files.DecryptedJpegOne).FullName + Files.DecryptedJpegOne;
            string saveDecryption2 = Directory.GetParent(Files.DecryptedJpegTwo).FullName + Files.DecryptedJpegTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1);
            encryptor.Decrypt(saveEncryption2, saveDecryption2);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
        }
        [TestMethod()]
        public void DecryptImgGifTest()
        {
            string fileToEncrypt = Directory.GetParent(Files.GifToEncryptOne).FullName + Files.GifToEncryptOne;
            string saveEncryption = Directory.GetParent(Files.EncryptedGifOne).FullName + Files.EncryptedGifOne;
            string saveDecryption = Directory.GetParent(Files.DecryptedGifOne).FullName + Files.DecryptedGifOne;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveDecryption, saveEncryption), "test 3");
        }
        [TestMethod()]
        public void DecryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Files.GifToEncryptOne).FullName + Files.GifToEncryptOne;
            string saveEncryption1 = Directory.GetParent(Files.EncryptedGifOne).FullName + Files.EncryptedGifOne;
            string fileToEncrypt2 = Directory.GetParent(Files.GifToEncryptTwo).FullName + Files.GifToEncryptTwo;
            string saveEncryption2 = Directory.GetParent(Files.EncryptedGifTwo).FullName + Files.EncryptedGifTwo;
            string saveDecryption1 = Directory.GetParent(Files.DecryptedGifOne).FullName + Files.DecryptedGifOne;
            string saveDecryption2 = Directory.GetParent(Files.DecryptedGifTwo).FullName + Files.DecryptedGifTwo;
            string seed = "1";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt1, saveEncryption1, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt1, saveEncryption1), "test 1");
            encryptor.Encrypt(fileToEncrypt2, saveEncryption2, seed);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt2, saveEncryption2), "test 2");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, fileToEncrypt2), "test 3");
            Assert.IsTrue(encryptor.compareFile(saveEncryption1, saveEncryption2), "test 4");
            encryptor.Decrypt(saveEncryption1, saveDecryption1);
            encryptor.Decrypt(saveEncryption2, saveDecryption2);
            Assert.IsFalse(encryptor.compareFile(saveEncryption1, saveDecryption2), "test 5");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt1, saveDecryption1), "test 6");
            Assert.IsTrue(encryptor.compareFile(saveDecryption1, saveDecryption2), "test 7");
        }
        //static String prependFile()
        [TestMethod()]
        public void testResourceLocations()
        {
            //C:\Users\fran\Documents\Visual Studio 2017\Git Repository\Network Encryption\Networking Encryption\Networking EncryptionTests\EncryptionTestFiles
            string a = Directory.GetParent(Files.DecryptedGifOne).FullName;
            a += FileExtentionFuncts.prependFile(Files.DecryptedGifOne);
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedGifOne).FullName) +  FileExtentionFuncts.prependFile(Files.DecryptedGifOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedGifTwo).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedGifTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedJpegOne).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedJpegOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedJpegTwo).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedJpegTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedPdfOne).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedPdfOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedPdfTwo).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedPdfTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedPngOne).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedPngOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedPngTwo).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedPngTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedTextOne).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedTextOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.DecryptedTextTwo).FullName) + FileExtentionFuncts.prependFile(Files.DecryptedTextTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedGifOne).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedGifOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedGifTwo).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedGifTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedJpegOne).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedJpegOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedJpegTwo).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedJpegTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedPdfOne).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedPdfOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedPdfTwo).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedPdfTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedPngOne).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedPngOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedPngTwo).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedPngTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedTextOne).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedTextOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.EncryptedTextTwo).FullName) + FileExtentionFuncts.prependFile(Files.EncryptedTextTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.GifToEncryptOne).FullName) + FileExtentionFuncts.prependFile(Files.GifToEncryptOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.GifToEncryptTwo).FullName) + FileExtentionFuncts.prependFile(Files.GifToEncryptTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.JpegToEncryptOne).FullName) + FileExtentionFuncts.prependFile(Files.JpegToEncryptOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.JpegToEncryptTwo).FullName) + FileExtentionFuncts.prependFile(Files.JpegToEncryptTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.PdfToEncryptOne).FullName) + FileExtentionFuncts.prependFile(Files.PdfToEncryptOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.PdfToEncryptTwo).FullName) + FileExtentionFuncts.prependFile(Files.PdfToEncryptTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.PngToEncryptOne).FullName) + FileExtentionFuncts.prependFile(Files.PngToEncryptOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.PngToEncryptTwo).FullName) + FileExtentionFuncts.prependFile(Files.PngToEncryptTwo)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.TextToEncryptOne).FullName) + FileExtentionFuncts.prependFile(Files.TextToEncryptOne)));
            Assert.IsTrue(Directory.Exists((Directory.GetParent(Files.TextToEncryptTwo).FullName) + FileExtentionFuncts.prependFile(Files.TextToEncryptTwo)));
        }
    }
}