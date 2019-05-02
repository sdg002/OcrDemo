using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrDemo
{
    public class MEFHost
    {
        public MEFHost()
        {
            var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
        [ImportMany]
        public Lazy<OcrDemo.Contracts.interfaces.IPluginTabbedView, Dictionary<string, object>>[]  TabbedViews { get; set; }

        [ImportMany]
        //public Lazy<OcrDemo.Contracts.interfaces.IPluginOcrEngine, Dictionary<string, object>>[] OcrEngines { get; set; }
        public Lazy<OcrDemo.Contracts.interfaces.IPluginOcrEngine, Contracts.interfaces.IPluginMetaData>[] OcrEngines { get; set; }
    }
    public class OcrEngineMeta
    {
        public string PluginName { get; set; }

    }
}
