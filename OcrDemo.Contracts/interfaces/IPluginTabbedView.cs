using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.interfaces
{
    /// <summary>
    /// Should be implemented by an user control that gets embedded inside a Tabbed view
    /// </summary>
    public interface IPluginTabbedView
    {
        /// <summary>
        /// This holds an instance to the main UI
        /// </summary>
        IMainView Main { get; set; }
        /// <summary>
        /// Fired every time a tabbed window is activated
        /// </summary>
        void OnActivate();
    }
}
