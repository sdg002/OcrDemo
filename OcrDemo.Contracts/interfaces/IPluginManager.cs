using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.interfaces
{
    /// <summary>
    /// This helps other plugins to query about plugins
    /// </summary>
    public interface IPluginManager
    {
        /// <summary>
        /// Gets all the OCR engines that have been discovered
        /// This helps a Tabbed view dynamically pick an OCR engine
        /// </summary>
        interfaces.IPluginMetaData[] OcrEngines { get;  }
        /// <summary>
        /// Gets the plugin object with the specified metadata
        /// Why is this beneficial? This hides the underlying Lazy implementation by only exposing the IPluginMetaData to the consumers
        /// </summary>
        /// <param name="mefmetdata"></param>
        /// <returns></returns>
        IPluginOcrEngine GetInstance(IPluginMetaData mefmetdata);
    }
}
