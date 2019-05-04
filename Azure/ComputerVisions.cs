using System;
using Microsoft.Extensions.Configuration;
using OcrDemo.Contracts.entity;

namespace Azure
{
    public class ComputerVisions : OcrDemo.Contracts.interfaces.IExtractTextFromImage
    {
        public TextExtractionResults Extract(byte[] image)
        {
            throw new NotImplementedException();
        }
    }
}
