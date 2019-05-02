using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class AwsTest
    {
        /// <summary>
        /// This is more of an integration test. Helps in doing a quick end-end check of the AWS wrapper components
        /// </summary>
        [TestMethod]
        public void AwsRekognitionHelloWorld()
        {
            Aws.AwsRekognition ocr = new Aws.AwsRekognition();
            ocr.AccessKeyId = Environment.GetEnvironmentVariable("awsrekognition_accesskeyid");
            ocr.AccessKeySecret = Environment.GetEnvironmentVariable("awsrekognition_secretaccesskey");
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\pics\\NonReadable.PNG");
            byte[] raw = System.IO.File.ReadAllBytes(pathSample);
            var results=ocr.Extract(raw);
            Assert.AreEqual(58, results.Results.Length);
            var txtIDRH = results.Results.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1018 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 80 && txtIDRH.Y2 < 115);
        }
    }
}
