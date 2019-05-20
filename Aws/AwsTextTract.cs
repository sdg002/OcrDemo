using System;
using System.Collections.Generic;
using System.Text;
using OcrDemo.Contracts.entity;

namespace Aws
{
    public class AwsTextTract : OcrDemo.Contracts.interfaces.IExtractTextFromImage
    {
        public string AmazonSecretKey
        {
            get; set;
        }

        public string AmazonAccessKey
        {
            get; set;
        }
        /// <summary>
        /// One of the published data centers where the service is located. E.g. eu-west-1
        /// </summary>
        public string Region { get; set; }
        public double Width { get; private set; }
        public double Height { get; private set; }

        public TextExtractionResults Extract(byte[] image)
        {
            var end = Amazon.RegionEndpoint.EUWest1;
            Amazon.Runtime.BasicAWSCredentials awsCreds =
                new Amazon.Runtime.BasicAWSCredentials(this.AmazonAccessKey, this.AmazonSecretKey);
            var region = Amazon.RegionEndpoint.GetBySystemName("eu-west-1");
            var cfg = new Amazon.Textract.AmazonTextractConfig();

            Amazon.Textract.AmazonTextractClient client = new Amazon.Textract.AmazonTextractClient(awsCreds, end);
            //Amazon.Textract.AmazonTextractClient client1 = new Amazon.Textract.AmazonTextractClient(awsCreds, cfg);
            var request = new Amazon.Textract.Model.DetectDocumentTextRequest();
            request.Document = new Amazon.Textract.Model.Document();
            Amazon.Textract.Model.DetectDocumentTextResponse result = null;
            using ( var memstm = new System.IO.MemoryStream(image))
            {
                request.Document.Bytes = memstm;
                result = client.DetectDocumentTextAsync(request).Result;
            }
            TextExtractionResults rs = new TextExtractionResults();
            List<TextResult> lstTextBoxes = new List<TextResult>();
            DetermineDimensions(image);
            foreach(var block in result.Blocks)
            {
                TextResult rsText = new TextResult();
                rsText.Text = block.Text;
                if (string.IsNullOrWhiteSpace(rsText.Text)) continue;//The first element is always NULL, so it appears
                if (block.BlockType == Amazon.Textract.BlockType.WORD)
                {
                    double xTopLeft = block.Geometry.BoundingBox.Left * this.Width;
                    double yTopLeft = block.Geometry.BoundingBox.Top * this.Height;
                    double width = block.Geometry.BoundingBox.Width * this.Width;
                    double height = block.Geometry.BoundingBox.Height * this.Height;
                    rsText.X1 = xTopLeft;
                    rsText.Y1 = yTopLeft;
                    rsText.X2 = rsText.X1 + width;
                    rsText.Y2 = rsText.Y1 + height;
                    lstTextBoxes.Add(rsText);
                }
            }
            rs.Results = lstTextBoxes.ToArray();
            return rs;
        }

        private void DetermineDimensions(byte[] image)
        {
            using (var memStm = new System.IO.MemoryStream(image))
            {
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(memStm))
                {
                    this.Width = bmp.Width;
                    this.Height = bmp.Height;
                }
            }
        }
    }
}
