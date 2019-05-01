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
    }
}
