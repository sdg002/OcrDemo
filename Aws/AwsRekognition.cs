using System;
using System.Collections.Generic;
using Amazon.Rekognition;
using OcrDemo.Contracts.entity;

namespace Aws
{
    public class AwsRekognition : OcrDemo.Contracts.interfaces.IExtractTextFromImage
    {
        public AwsRekognition()
        {
            this.AccessKeyId = "AKIA4DDH53HIIT3TOZUD";
            this.AccessKeySecret = "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";
            this.EndPoint = "rekognition.eu-west-1.amazonaws.com	";
        }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string EndPoint { get; private set; }

        public TextExtractionResults Extract(byte[] image)
        {
            TextExtractionResults rs = new TextExtractionResults();
            List<TextResult> rsTextBoxes = new List<TextResult>();
            AmazonRekognitionClient rekognitionClient = CreateAwsClient();
            using (var stm = new System.IO.MemoryStream(image))
            {
                double imgWidth, imgHeight;
                using (var bmp = new System.Drawing.Bitmap(stm))
                {
                    imgWidth = bmp.Width;
                    imgHeight = bmp.Height;
                }
                var awsImage = new Amazon.Rekognition.Model.Image();
                awsImage.Bytes = new System.IO.MemoryStream(image);
                var req = new Amazon.Rekognition.Model.DetectTextRequest
                {
                    Image = awsImage
                };
                var detectTextResponse = rekognitionClient.DetectTextAsync(req).Result;
                foreach (Amazon.Rekognition.Model.TextDetection textResult in detectTextResponse.TextDetections)
                {
                    string text = textResult.DetectedText;
                    Amazon.Rekognition.Model.Geometry oGeom = textResult.Geometry;
                    TextResult rsText = new TextResult();
                    rsText.Text = text;
                    rsText.X1 = oGeom.BoundingBox.Left * imgWidth;
                    rsText.X2 = rsText.X1+oGeom.BoundingBox.Width * imgWidth;
                    rsText.Y1 = oGeom.BoundingBox.Top * imgHeight;
                    rsText.Y2 = rsText.Y1 + oGeom.BoundingBox.Height * imgHeight;
                    rsTextBoxes.Add(rsText);
                }
            }
            rs.Results = rsTextBoxes.ToArray();
            return rs;
        }

        private AmazonRekognitionClient CreateAwsClient()
        {
            var cred = new Amazon.Runtime.BasicAWSCredentials(this.AccessKeyId, this.AccessKeySecret);
            var cli = new AmazonRekognitionClient(cred, Amazon.RegionEndpoint.EUWest1);
            return cli;
        }
    }
}
