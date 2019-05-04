using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OcrDemo.Contracts.entity;

namespace OcrDemo
{
    [OcrDemo.Contracts.entity.OcrEngineExport("awsrekognition", "Ocr Engine built on AWS Rekognition")]
    //[ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME, "awsrekognition")]
    //[ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_TAB_TITLE, "Ocr Engine built on AWS Rekognition")]
    public class AwsOcrPlugin : OcrDemo.Contracts.interfaces.IPluginOcrEngine
    {
        [Import()]
        public IConfiguration Config { get; set ; }

        public TextExtractionResults DoOcr(byte[] image)
        {
            if (Config == null) throw new InvalidOperationException($"The property {nameof(Config)} has not been initialized");
            Aws.AwsRekognition engine = new Aws.AwsRekognition();
            engine.AccessKeyId = Config["awsrekognition_accesskeyid"];
            engine.AccessKeySecret = Config["awsrekognition_secretaccesskey"];
            if (string.IsNullOrWhiteSpace(engine.AccessKeyId)) throw new InvalidOperationException($"The setting 'awsrekognition_accesskeyid' has not been initialized");
            if (string.IsNullOrWhiteSpace(engine.AccessKeySecret)) throw new InvalidOperationException($"The setting 'awsrekognition_secretaccesskey' has not been initialized");
            return engine.Extract(image);
        }
    }


    [OcrDemo.Contracts.entity.OcrEngineExport("ocrspace", "Ocr Engine built on OCR.SPACE")]
    public class OcrSpacePlugin : OcrDemo.Contracts.interfaces.IPluginOcrEngine
    {
        [Import()]
        public IConfiguration Config { get; set; }
        public TextExtractionResults DoOcr(byte[] image)
        {
            if (Config == null) throw new InvalidOperationException($"The property {nameof(Config)} has not been initialized");
            var engine = new OcrSpace.Ocr();   
            engine.ApiKey = Config["ocrspace_accesskey"];
            return engine.Extract(image);
        }
    }

}
