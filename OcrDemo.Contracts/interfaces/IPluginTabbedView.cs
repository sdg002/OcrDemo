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
        /// Fired only once, immediately after creation and after Main has been set
        /// The plugin can use this do any initializations. e.g. set up event handlers
        /// </summary>
        void OnInit();
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
