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

/*Build:1.0.4
 * Date: 7/12/17
 * Code Metrics: 59 29  1   8   302
 */ 
namespace Networking_Encryption.Tests
{
    [TestClass()]
    public class EncryptionTests
    {
        [TestMethod()]
        public void compareFileAreEqual()
        {
            string fileOne = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            string fileTwo = Directory.GetParent(Resource.TextToEncryptTwo).FullName;
            bool a = Directory.Exists(fileOne);
            bool b = Directory.Exists(fileTwo);
            Encryption encryptor = new Encryption();
            Assert.IsTrue(encryptor.compareFile(@fileOne, @fileTwo), "Test: 1");
        }
        [TestMethod()]
        public void compareFileSameFile()
        {
            string fileOne = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            Encryption encryptor = new Encryption();
            Assert.IsTrue(encryptor.compareFile(fileOne, fileOne), "Test: 1");
        }
        public void compareFileNotEqual()
        {
            string fileOne = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            string fileTwo = Directory.GetParent(Resource.DecryptedTextOne).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            string saveDestination = Directory.GetParent(Resource.EncryptedTextOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptTxtFileSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            string saveDestination1 = Directory.GetParent(Resource.EncryptedTextOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.TextToEncryptTwo).FullName;
            string saveDestination2 = Directory.GetParent(Resource.EncryptedTextTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.PdfToEncryptOne).FullName;
            string saveDestination = Directory.GetParent(Resource.EncryptedPdfOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptPdfSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.PdfToEncryptOne).FullName;
            string saveDestination1 = Directory.GetParent(Resource.EncryptedPdfOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.PdfToEncryptTwo).FullName;
            string saveDestination2 = Directory.GetParent(Resource.EncryptedPdfTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.PngToEncryptOne).FullName;
            string saveDestination = Directory.GetParent(Resource.EncryptedPngOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgPngSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.PngToEncryptOne).FullName;
            string saveDestination1 = Directory.GetParent(Resource.EncryptedPngOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.PngToEncryptTwo).FullName;
            string saveDestination2 = Directory.GetParent(Resource.EncryptedPngTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.JpegToEncryptOne).FullName;
            string saveDestination = Directory.GetParent(Resource.EncryptedJpegOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.JpegToEncryptOne).FullName;
            string saveDestination1 = Directory.GetParent(Resource.EncryptedJpegOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.JpegToEncryptTwo).FullName;
            string saveDestination2 = Directory.GetParent(Resource.EncryptedJpegTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.GifToEncryptOne).FullName;
            string saveDestination = Directory.GetParent(Resource.EncryptedGifOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveDestination);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveDestination));
        }
        [TestMethod()]
        public void EncryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.GifToEncryptOne).FullName;
            string saveDestination1 = Directory.GetParent(Resource.EncryptedGifOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.GifToEncryptTwo).FullName;
            string saveDestination2 = Directory.GetParent(Resource.EncryptedGifTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            string saveEncryption = Directory.GetParent(Resource.EncryptedTextOne).FullName;
            string saveDecryption = Directory.GetParent(Resource.DecryptedTextOne).FullName;
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
            string fileToEncrypt1 = Directory.GetParent(Resource.TextToEncryptOne).FullName;
            string saveEncryption1 = Directory.GetParent(Resource.EncryptedTextOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.TextToEncryptTwo).FullName;
            string saveEncryption2 = Directory.GetParent(Resource.EncryptedTextTwo).FullName;
            string saveDecryption1 = Directory.GetParent(Resource.DecryptedTextOne).FullName;
            string saveDecryption2 = Directory.GetParent(Resource.DecryptedTextTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.PdfToEncryptOne).FullName;
            string saveEncryption = Directory.GetParent(Resource.EncryptedPdfOne).FullName;
            string savedecryption = Directory.GetParent(Resource.DecryptedPdfOne).FullName;
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
            string fileToEncrypt1 = Directory.GetParent(Resource.PdfToEncryptOne).FullName;
            string saveEncryption1 = Directory.GetParent(Resource.EncryptedPdfOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.PdfToEncryptTwo).FullName;
            string saveEncryption2 = Directory.GetParent(Resource.EncryptedPdfTwo).FullName;
            string saveDecryption1 = Directory.GetParent(Resource.DecryptedPdfOne).FullName;
            string saveDecryption2 = Directory.GetParent(Resource.DecryptedPdfTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.PngToEncryptOne).FullName;
            string saveEncryption = Directory.GetParent(Resource.EncryptedPngOne).FullName;;
            string saveDecryption = Directory.GetParent(Resource.DecryptedPngOne).FullName;
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
            string fileToEncrypt1 = Directory.GetParent(Resource.PngToEncryptOne).FullName;
            string saveEncryption1 = Directory.GetParent(Resource.EncryptedPngOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.PngToEncryptOne).FullName;
            string saveEncryption2 = Directory.GetParent(Resource.EncryptedPngTwo).FullName;
            string saveDecryption1 = Directory.GetParent(Resource.DecryptedPngOne).FullName;
            string saveDecryption2 = Directory.GetParent(Resource.DecryptedPngTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.JpegToEncryptOne).FullName;
            string saveEncryption = Directory.GetParent(Resource.EncryptedJpegOne).FullName;
            string saveDecryption = Directory.GetParent(Resource.DecryptedJpegOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsFalse(encryptor.compareFile(saveEncryption, saveDecryption), "test 2");
            Assert.IsTrue(encryptor.compareFile(saveDecryption, fileToEncrypt), "test 3");
        }
        [TestMethod()]
        public void DecryptImgJpegSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.JpegToEncryptOne).FullName;
            string saveEncryption1 = Directory.GetParent(Resource.EncryptedJpegOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.JpegToEncryptTwo).FullName;
            string saveEncryption2 = Directory.GetParent(Resource.EncryptedJpegTwo).FullName;
            string saveDecryption1 = Directory.GetParent(Resource.DecryptedJpegOne).FullName;
            string saveDecryption2 = Directory.GetParent(Resource.DecryptedJpegTwo).FullName;
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
            string fileToEncrypt = Directory.GetParent(Resource.GifToEncryptOne).FullName;
            string saveEncryption = Directory.GetParent(Resource.EncryptedGifOne).FullName;
            string saveDecryption = Directory.GetParent(Resource.DecryptedGifOne).FullName;
            Encryption encryptor = new Encryption();
            encryptor.Encrypt(fileToEncrypt, saveEncryption);
            Assert.IsFalse(encryptor.compareFile(fileToEncrypt, saveEncryption),"test 1");
            Assert.IsTrue(encryptor.compareFile(fileToEncrypt, saveDecryption), "test 2");
            Assert.IsFalse(encryptor.compareFile(saveDecryption, saveEncryption), "test 3");
        }
        [TestMethod()]
        public void DecryptImgGifSameSeedTest()
        {
            string fileToEncrypt1 = Directory.GetParent(Resource.GifToEncryptOne).FullName;
            string saveEncryption1 = Directory.GetParent(Resource.EncryptedGifOne).FullName;
            string fileToEncrypt2 = Directory.GetParent(Resource.GifToEncryptTwo).FullName;
            string saveEncryption2 = Directory.GetParent(Resource.EncryptedGifTwo).FullName;
            string saveDecryption1 = Directory.GetParent(Resource.DecryptedGifOne).FullName;
            string saveDecryption2 = Directory.GetParent(Resource.DecryptedGifTwo).FullName;
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
        public void testResourceLocations()
        {
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedGifOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedGifTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedJpegOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedJpegTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedPdfOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedPdfTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedPngOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedPngTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedTextOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.DecryptedTextTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedGifOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedGifTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedJpegOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedGifTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedJpegOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedJpegTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedPdfOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedPdfTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedPngOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedPngTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedTextOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.EncryptedTextTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.GifToEncryptOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.GifToEncryptTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.JpegToEncryptOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.JpegToEncryptTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.PdfToEncryptOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.PdfToEncryptTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.PngToEncryptOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.PngToEncryptTwo).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.TextToEncryptOne).FullName));
            Assert.IsTrue(Directory.Exists(Directory.GetParent(Resource.TextToEncryptTwo).FullName));
        }
    }
}