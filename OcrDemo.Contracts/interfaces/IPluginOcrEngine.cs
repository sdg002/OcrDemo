using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.interfaces
{
    /// <summary>
    /// MEF consumable plugin
    /// </summary>
    public interface IPluginOcrEngine
    {
        /// <summary>
        /// Name-value pairs neccessary for the underlying engine. e.g. AWS might require Access ID and Secret Key
        /// </summary>
        /// <param name="config"></param>
        void OnInit(Microsoft.Extensions.Configuration.IConfiguration config);
        entity.TextExtractionResults DoOcr(byte[] image);
    }
}
