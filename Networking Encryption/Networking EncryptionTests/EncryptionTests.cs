using Microsoft.VisualStudio.TestTools.UnitTesting;
using Networking_Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Build:1.0.1
 * Date: 6/30/17
 * Code Metrics: 61 28  1   5   268
 */ 
namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class EncryptionTests
    {
        [TestMethod()]
        public void compareFileAreEqual()
        {
            string fileOne = @"TextToEncryptOne.txt";
            string fileTwo = @"TextToEncryptTwo.txt";
            Encryption encryptor = new Encryption();
            Assert.IsTrue(encryptor.compareFile(fileOne, fileTwo), "Test: 1");
        }
        [TestMethod()]
        public void compareFileSameFile()
        {
            string fileOne = @"TextToEncryptOne.txt";
            Encryption encryptor = new Encryption();
            Assert.IsTrue(encryptor.compareFile(fileOne, fileOne), "Test: 1");
        }
        public void compareFileNotEqual()
        {
            string fileOne = @"TextToEncryptOne.txt";
            string fileTwo = @"DecryptedTextOne.txt";
            Encryption encryptor = new Encryption();
            Assert.IsFalse(encryptor.compareFile(fileOne, fileTwo), "Test: 1");
        }
        [TestMethod()]
        public void EncryptStringTest()
        {
            string word = "this is a test";
            Encryption encryptor = new Encryption();
            string cipherText = encryptor.Encrypt(word);
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
            string cipherText = encryptor1.Encrypt(word,seed);
            Assert.AreNotEqual(word, cipherText, "Test: 1");
            Assert.IsTrue(cipherText.Count() > word.Count(), "Test: 2");
            Assert.AreEqual(cipherText, encryptor2.Encrypt(word, seed), "Test: 3");
        }
        [TestMethod()]
        public void EncryptTxtFileTest()
        {
            string fileToEncrypt = @"TextToEncryptOne.txt";
            string saveDestination = @"EncryptedTextOne.txt";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = @"TextToEncryptOne.txt";
            string saveDestination1 = @"EncryptedTextOne.txt";
            string fileToEncrypt2 = @"TextToEncryptTwo.txt";
            string saveDestination2 = @"EncryptedTextTwo.txt";
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
            string fileToEncrypt = @"PdfToEncryptOne.pdf";
            string saveDestination = @"EncryptedPdfOne.pdf";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = @"PdfToEncryptOne.pdf";
            string saveDestination1 = @"EncryptedPdfOne.pdf";
            string fileToEncrypt2 = @"PdfToEncryptTwo.pdf";
            string saveDestination2 = @"EncryptedPdfTwo.pdf";
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
            string fileToEncrypt = @"PngImageToEncryptOne.png";
            string saveDestination = @"EncryptedPngOne.png";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = @"PngImageToEncryptOne.png";
            string saveDestination1 = @"EncryptedPngOne.png";
            string fileToEncrypt2 = @"PngImageToEncryptTwo.png";
            string saveDestination2 = @"EncryptedPngTwo.png";
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
            string fileToEncrypt = @"JpegToEncryptOne.jpg";
            string saveDestination = @"EncryptedJpegOne.jpg";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = @"JpegToEncryptOne.jpg";
            string saveDestination1 = @"EncryptedJpegOne.jpg";
            string fileToEncrypt2 = @"JpegToEncryptTwo.jpg";
            string saveDestination2 = @"EncryptedJpegTwo.jpg";
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
            string fileToEncrypt = @"GifToEncryptOne.gif";
            string saveDestination = @"EncryptedGifOne.gif";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = @"GifToEncryptOne.gif";
            string saveDestination1 = @"EncryptedGifOne.gif";
            string fileToEncrypt2 = @"GifToEncryptTwo.gif";
            string saveDestination2 = @"EncryptedGifTwo.gif";
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
            string cipherText = encryptor.Encrypt(word);
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
            string cipherText = encryptor1.Encrypt(word, seed);
            string cipherText2 = encryptor2.Encrypt(word, seed);
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
            string fileToEncrypt = @"TextToEncryptOne.txt";
            string saveEncryption = @"EncryptedTextOne.txt";
            string saveDecryption = @"DecryptedTextOne.txt";
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
            string fileToEncrypt1 = @"TextToEncryptOne.txt";
            string saveEncryption1 = @"EncryptedTextOne.txt";
            string fileToEncrypt2 = @"TextToEncryptTwo.txt";
            string saveEncryption2 = @"EncryptedTextTwo.txt";
            string saveDecryption1 = @"DecryptedTextOne.txt";
            string saveDecryption2 = @"DecryptedTextTwo.txt";
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
            string fileToEncrypt = @"PdfToEncryptOne.pdf";
            string saveEncryption = @"EncryptedPdfOne.pdf";
            string savedecryption = @"DecryptedPdfOne.pdf";
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
            string fileToEncrypt1 = @"PdfToEncryptOne.pdf";
            string saveEncryption1 = @"EncryptedPdfOne.pdf";
            string fileToEncrypt2 = @"PdfToEncryptTwo.pdf";
            string saveEncryption2 = @"EncryptedPdfTwo.pdf";
            string saveDecryption1 = @"DecryptedPdfOne.pdf";
            string saveDecryption2 = @"DecryptedPdfTwo.pdf";
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
            string fileToEncrypt = @"PngImageToEncryptOne.png";
            string saveEncryption = @"EncryptedPngOne.png";
            string saveDecryption = @"DecryptedPngOne.png";
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
            string fileToEncrypt1 = @"PngImageToEncryptOne.png";
            string saveEncryption1 = @"EncryptedPngOne.png";
            string fileToEncrypt2 = @"PngImageToEncryptTwo.png";
            string saveEncryption2 = @"EncryptedPngTwo.png";
            string saveDecryption1 = @"DecryptedPngOne.png";
            string saveDecryption2 = @"DecryptedPngTwo.png";
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
            string fileToEncrypt = @"JpegToEncryptOne.jpg";
            string saveEncryption = @"EncryptedJpegOne.jpg";
            string saveDecryption = @"DecryptedJpegOne.jpg";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsFalse(encryptor.compareFile(saveEncryption, saveDecryption), "test 2");
            Assert.IsTrue(encryptor.compareFile(saveDecryption, fileToEncrypt), "test 3");
        }
        [TestMethod()]
        public void DecryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = @"JpegToEncryptOne.jpg";
            string saveEncryption1 = @"EncryptedJpegOne.jpg";
            string fileToEncrypt2 = @"JpegToEncryptTwo.jpg";
            string saveEncryption2 = @"EncryptedJpegTwo.jpg";
            string saveDecryption1 = @"DecryptedJpegOne.jpg";
            string saveDecryption2 = @"DecryptedJpegTwo.jpg";
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
            string fileToEncrypt = @"GifToEncryptOne.gif";
            string saveEncryption = @"EncryptedGifOne.gif";
            string saveDecryption = @"DecryptedGifOne.gif";
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveDecryption, saveEncryption), "test 3");
        }
        [TestMethod()]
        public void DecryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = @"GifToEncryptOne.gif";
            string saveEncryption1 = @"EncryptedGifOne.gif";
            string fileToEncrypt2 = @"GifToEncryptTwo.gif";
            string saveEncryption2 = @"EncryptedGifTwo.gif";
            string saveDecryption1 = @"DecryptedGifOne.gif";
            string saveDecryption2 = @"DecryptedGifTwo.gif";
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
    }
}