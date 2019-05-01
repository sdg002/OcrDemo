using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OcrDemo.Contracts.interfaces;

namespace OcrDemo.Settings
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    [Export(typeof(OcrDemo.Contracts.interfaces.IPluginTabbedView))]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME, "settings")]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_TAB_TITLE, "Settings")]
    public partial class View : UserControl, OcrDemo.Contracts.interfaces.IPluginTabbedView
    {
        public View()
        {
            InitializeComponent();
        }

        public IMainView Main { get ; set ; }

        public void OnActivate()
        {
            
        }
    }
}
