using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts.interfaces
{
    /// <summary>
    /// Used for providing metadata through Lazy MEF initialization 
    /// Should have Gets only. No Setters allowed on this interface
    /// </summary>
    public interface IPluginMetaData
    {
        string Name { get; }
        string Description { get; }

    }
}
