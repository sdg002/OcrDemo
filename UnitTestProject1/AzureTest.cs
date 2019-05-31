using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
