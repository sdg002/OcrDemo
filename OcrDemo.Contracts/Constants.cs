using System;
using System.Collections.Generic;
using System.Text;

namespace OcrDemo.Contracts
{
    public class Constants
    {
        /// <summary>
        /// A short and unique name for every plugin. Should be exported by all plugins
        /// </summary>
        public const string MEF_ATTRIBUTE_PLUGINNAME = "PluginName";
        /// <summary>
        /// Should be used by any Tabbed UI - this gives the header text
        /// </summary>
        public const string MEF_ATTRIBUTE_PLUGIN_TAB_TITLE= "PluginTabHeader";
        /// <summary>
        /// A descriptive text of the plugin.
        /// </summary>
        public const string MEF_ATTRIBUTE_PLUGIN_DESCRIPTION = "PluginDescription";

    }
}
