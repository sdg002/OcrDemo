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
        Microsoft.Extensions.Configuration.IConfiguration _config;
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

        [Export]
        Microsoft.Extensions.Configuration.IConfiguration Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new entity.ConfigWrapperOverProperties();
                }
                return _config;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
    public class OcrEngineMeta
    {
        public string PluginName { get; set; }

    }
}
