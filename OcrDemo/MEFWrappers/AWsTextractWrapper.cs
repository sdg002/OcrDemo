using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OcrDemo.Contracts.entity;

namespace OcrDemo.MEFWrappers
{
    [OcrDemo.Contracts.entity.OcrEngineExport("awstextract", "Ocr Engine built on AWS Textextract")]
    public class AWsTextractWrapper : OcrDemo.Contracts.interfaces.IPluginOcrEngine
    {
        [Import()]
        public IConfiguration Config { get; set; }

        public TextExtractionResults DoOcr(byte[] image)
        {
            var aws = new Aws.AwsTextTract();
            aws.AmazonAccessKey = this.Config["awstextract_accesskey"];
            aws.AmazonSecretKey = this.Config["awstextract_secretkey"];
            return aws.Extract(image);
        }
    }
}
