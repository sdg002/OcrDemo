using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.interfaces
{
    /// <summary>
    /// Abstracts the main application Window
    /// </summary>
    public interface IMainView
    {
        /// <summary>
        /// When set to True, a busy indicator is shown
        /// </summary>
        bool IsBusy { get; set; }
        /// <summary>
        /// Sets the text on the specified panel of the main status bar
        /// </summary>
        /// <param name="panel">Should be zero for now</param>
        /// <param name="text">The text to be displayed</param>
        void SetStatus(int panel, string text);
        /// <summary>
        /// Gets the text from the specified panel of the main status bar
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        string GetStatus(int panel, string text);
        /// <summary>
        /// This will make the specified tabbed plugin active
        /// </summary>
        /// <param name="tabpluginname">the unique name of the tab plugin</param>
        void ActivateTab(string tabpluginname);

        ///// <summary>
        ///// Returns a list of all OCR engines that have been loaded. This helps a Tabbed view dynamically pick an OCR engine
        ///// </summary>
        //Lazy<OcrDemo.Contracts.interfaces.IPluginOcrEngine, Contracts.interfaces.IPluginMetaData>[] OcrEngines { get;  }
        /// <summary>
        /// Exposes an interface to query/negotiate/invole all dynamically loaded plugins
        /// </summary>
        IPluginManager Plugins { get; }
    }
}
