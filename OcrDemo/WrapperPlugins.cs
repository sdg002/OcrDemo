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
        IConfiguration _config;
        public TextExtractionResults DoOcr(byte[] image)
        {
            Aws.AwsRekognition engine = new Aws.AwsRekognition();
            engine.AccessKeyId = _config["awsrekognition_accesskeyid"];
            engine.AccessKeySecret = _config["awsrekognition_secretaccesskey"];
            return engine.Extract(image);
        }

        public void OnInit(IConfiguration config)
        {
            _config = config;
        }
    }
}
