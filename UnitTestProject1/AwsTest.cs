using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class AwsTest
    {
        /// <summary>
        /// This is more of an integration test. Helps in doing a quick end-end check of the AWS wrapper components
        /// You will need to set the Environment variables for AWS keys
        /// Here we are testing AWS Rekognition
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
            Assert.AreEqual(58, results.Blocks.Length);
            var txtIDRH = results.Blocks.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1018 && txtIDRH.X2 < 1108);
            Assert.IsTrue(txtIDRH.Y1 > 80 && txtIDRH.Y2 < 115);
        }
        /// <summary>
        /// This is an integration test over AWS Textract service
        /// </summary>
        [TestMethod]
        public void AwsTextTract()
        {
            IConfiguration cfg = BuildConfig();
            Aws.AwsTextTract ocr = new Aws.AwsTextTract();
            ocr.AmazonAccessKey = cfg["awstextract_accesskey"];
            ocr.AmazonSecretKey = cfg["awstextract_secretkey"];
            ocr.Region = "eu-west-1";
            string pathSample = System.IO.Path.Combine(util.Util.GetProjectDir(), "Data\\pics\\NonReadable.PNG");
            byte[] raw = System.IO.File.ReadAllBytes(pathSample);
            var results = ocr.Extract(raw);
            Assert.AreEqual(62, results.Blocks.Length);
            var txtIDRH = results.Blocks.First(t => t.Text.Contains("IDRH"));
            Assert.IsTrue(txtIDRH.X1 > 1018 && txtIDRH.X2 < 1111);
            Assert.IsTrue(txtIDRH.Y1 > 85 && txtIDRH.Y2 < 117);

        }

        private IConfigurationRoot BuildConfig()
        {
            var configbuilder = new ConfigurationBuilder();
            configbuilder.AddUserSecrets("78810728-6df2-4953-aa98-b77a6a77b66e");
            return configbuilder.Build();
        }
    }
}
