using OcrDemo.Contracts.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrDemo.entity
{
    class PluginManager : IPluginManager
    {
        MEFHost _hostMef;
        public PluginManager(MEFHost host)
        {
            _hostMef = host;
        }
        public IPluginMetaData[] OcrEngines
        {
            get
            {
                return _hostMef.OcrEngines.Select(l => l.Metadata).ToArray();
            }
        }

        public Lazy<IPluginTabbedView, Dictionary<string, object>>[] TabbedViews
        {
            get
            {
                return _hostMef.TabbedViews;
            }
        }

        public IPluginOcrEngine GetInstance(IPluginMetaData mefmetdata)
        {
            if (mefmetdata.MefContractType == typeof(OcrDemo.Contracts.interfaces.IPluginOcrEngine))
            {
                var lazyInstance=_hostMef.OcrEngines.FirstOrDefault(l => l.Metadata.Name == mefmetdata.Name);
                if (lazyInstance == null) throw new InvalidOperationException($"The plugin was not found. Name={mefmetdata.Name}");
                return lazyInstance.Value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
