using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace OcrDemo.Contracts.entity
{
    /// <summary>
    /// The classes used for Ocr engin plugins should be decorated with this attribute
    /// Lets you specify the name and description of the plugin in a strongly typed manner
    /// </summary>
    [MetadataAttribute]//Required
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]//Required
    public class OcrEngineExportAttribute : ExportAttribute, interfaces.IPluginMetaData
    {
        public string Name { get;  }
        public string Description { get ; }

        public string MefContractName => this.ContractName;

        public Type MefContractType => this.ContractType;

        public OcrEngineExportAttribute(string name,string description) :base(typeof(OcrDemo.Contracts.interfaces.IPluginOcrEngine))
        {
            this.Name = name;
            this.Description = description;
        }
    }
}

