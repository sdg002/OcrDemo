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
        //We do not need this any more because of Lazy initialization we are never going to know when to call OnInit
        //We chose property injection to pass all that is needed
        ///// <summary>
        ///// Any custom initializations that are required on a per instance basis
        ///// </summary>
        //void OnInit();
        entity.TextExtractionResults DoOcr(byte[] image);
        /// <summary>
        /// Implementors must export this property so that settings can be injected at run time
        /// Name-value pairs neccessary for the underlying engine. e.g. AWS might require Access ID and Secret Key
        /// </summary>
        Microsoft.Extensions.Configuration.IConfiguration Config { get; set; }
    }
}
