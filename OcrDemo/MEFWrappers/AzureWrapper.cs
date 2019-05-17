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
    [OcrDemo.Contracts.entity.OcrEngineExport("azure", "Ocr Engine built on Microsoft")]
    public class AzureWrapper : OcrDemo.Contracts.interfaces.IPluginOcrEngine
    {
        [Import()]
        public IConfiguration Config { get; set; }

        public TextExtractionResults DoOcr(byte[] image)
        {
            Azure.ComputerVisions engine = new Azure.ComputerVisions();
            engine.Url = "https://uksouth.api.cognitive.microsoft.com/";
            engine.AppKey = Config["azure_accesskey"];
            return engine.Extract(image);
        }
    }
}
