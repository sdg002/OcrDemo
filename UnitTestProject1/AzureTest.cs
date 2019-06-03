using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OcrDemo.Contracts.entity;

namespace UnitTestProject1
{
    [TestClass]
    public class AzureTest
    {
        [TestMethod]
        public void Live_HappyPath()
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            engine.Url = "https://uksouth.api.cognitive.microsoft.com/";
            engine.AppKey = Environment.GetEnvironmentVariable("azureocr_key");
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\pics\\NonReadable.PNG");
            byte[] raw = System.IO.File.ReadAllBytes(pathSample);
            var rsOuter = engine.Extract(raw);
            Assert.AreEqual(62, rsOuter.Blocks.Length);
            var txtIDRH = rsOuter.Blocks.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1020 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 85 && txtIDRH.Y2 < 115);
        }
        [TestMethod]
        public void Live_InvalidAppKey()
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            engine.Url = "https://uksouth.api.cognitive.microsoft.com/";
            engine.AppKey = "some invalid key";
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\pics\\NonReadable.PNG");
            byte[] raw = System.IO.File.ReadAllBytes(pathSample);
            var rsOuter = engine.Extract(raw);
            //We will not receive an Exception. Azure will return a custom JSON with the error desscription
        }
        [TestMethod]
        public void Live_InvalidPicture()
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            engine.Url = "https://uksouth.api.cognitive.microsoft.com/";
            engine.AppKey = Environment.GetEnvironmentVariable("azureocr_key");
            //We are passing the contents of the TXT file
            string pathInvalidPictureFile = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\pics\\Readme.txt");
            byte[] raw = System.IO.File.ReadAllBytes(pathInvalidPictureFile);
            var rsOuter = engine.Extract(raw);
            //We will not receive an Exception. Azure will return a custom JSON with the error desscription
        }
        [TestMethod]
        public void ProcessJson()
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\Azure\\SampleResponse.json");
            string json = System.IO.File.ReadAllText(pathSample);
            var rsOuter = engine.ExtractEntitiesFromJson(json);
            Assert.AreEqual(62, rsOuter.Blocks.Length);
            var txtIDRH = rsOuter.Blocks.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1020 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 85 && txtIDRH.Y2 < 115);
            Assert.AreEqual(6,rsOuter.Sentences.Length);
            Assert.IsTrue(rsOuter.Sentences[0].ToString().Contains("IDRH"));
            Assert.IsTrue(rsOuter.Sentences[5].ToString().Contains("regardless"));
            Assert.AreEqual(1,rsOuter.Paragraphs.Length);

        }
        //[TestMethod]
        //public void Mocked_HappyPath_Verify_Parameters_Are_Correctly_Received()
        //{
        //    Azure.ComputerVisions engine = null;
        //    byte[] inputpicture = new byte[] { 0, 1, 1, 0 };
        //    string appKey = Guid.NewGuid().ToString();
        //    string url = Guid.NewGuid().ToString();
        //    Mock<Azure.ComputerVisions> mocked = new Mock<Azure.ComputerVisions>();
        //    Func<byte[], TextExtractionResults> fnMockedExtract = null;
        //    fnMockedExtract = delegate (byte[] picture)
        //    {
        //        //Verify input parameters
        //        CollectionAssert.AreEqual(inputpicture, picture);
        //        Assert.AreEqual(engine.Url, url);
        //        Assert.AreEqual(engine.AppKey, appKey);
        //        return new TextExtractionResults();
        //    };

        //    mocked.Setup(e => e.Extract(inputpicture)).Returns <Byte[]>(fnMockedExtract);
        //    engine = mocked.Object;
        //    engine.Url = url;
        //    engine.AppKey = appKey;
        //    engine.Extract(inputpicture);
        //}
        [TestMethod]
        public void Mocked_HappyPath_FakeJson()
        {
            Azure.ComputerVisions engine = null;
            byte[] inputpicture = new byte[] { 0, 1, 1, 0 };
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\Azure\\SampleResponse.json");
            string jsonFake = System.IO.File.ReadAllText(pathSample);
            string appKey = Guid.NewGuid().ToString();
            string url = Guid.NewGuid().ToString();
            Mock<Azure.ComputerVisions> mocked = new Mock<Azure.ComputerVisions>();
            Func<byte[],string> fnGetJsonFromAzure=delegate(byte[] image)
            {
                //Verify that the function has received the picture byte that was intended
                CollectionAssert.AreEqual(inputpicture, image);
                //We are bypassing the live call to Azure by using fake json
                return jsonFake;
            };
            mocked.Setup(e => e.GetJsonFromAzure(inputpicture)).Returns<byte[]>(fnGetJsonFromAzure);
            engine = mocked.Object;
            engine.Url = url;
            engine.AppKey = appKey;
            var resultsFromOcr=engine.Extract(inputpicture);
            var txtIDRH = resultsFromOcr.Blocks.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1020 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 85 && txtIDRH.Y2 < 115);
            Assert.AreEqual(6, resultsFromOcr.Sentences.Length);
            Assert.IsTrue(resultsFromOcr.Sentences[0].ToString().Contains("IDRH"));
            Assert.IsTrue(resultsFromOcr.Sentences[5].ToString().Contains("regardless"));
            Assert.AreEqual(1, resultsFromOcr.Paragraphs.Length);

        }
    }
}
