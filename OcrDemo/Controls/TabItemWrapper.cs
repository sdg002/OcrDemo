using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OcrDemo.Controls
{
    public class TabItemWrapper : TabItem
    {
        /// <summary>
        /// a reference to the plugin object which will be injected as a content into this TabItem
        /// </summary>
        public Lazy<OcrDemo.Contracts.interfaces.IPluginTabbedView, Dictionary<string, object>> Plugin { get; set; }
    }
}
