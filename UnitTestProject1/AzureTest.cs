using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class AzureTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            engine.Url = "https://uksouth.api.cognitive.microsoft.com/";
            engine.AppKey = Environment.GetEnvironmentVariable("azureocr_key");
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\pics\\NonReadable.PNG");
            byte[] raw = System.IO.File.ReadAllBytes(pathSample);
            var rsOuter = engine.Extract(raw);
            Assert.AreEqual(62, rsOuter.Results.Length);
            var txtIDRH = rsOuter.Results.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1020 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 85 && txtIDRH.Y2 < 115);
        }
        [TestMethod]
        public void ProcessJson()
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\Azure\\SampleResponse.json");
            string json = System.IO.File.ReadAllText(pathSample);
            var rsOuter = engine.ExtractEntitiesFromJson(json);
            Assert.AreEqual(62, rsOuter.Results.Length);
            var txtIDRH = rsOuter.Results.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1020 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 85 && txtIDRH.Y2 < 115);

        }
    }
}
