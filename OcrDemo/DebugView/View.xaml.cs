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
using System.Windows.Shapes;
using OcrDemo.Contracts.interfaces;

namespace OcrDemo.DebugView
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
#if DEBUG
        //We want this View while debugging only
    [Export(typeof(OcrDemo.Contracts.interfaces.IPluginTabbedView))]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME, "debugpanel")]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_TAB_TITLE, "Debug Widgets and Dialogs")]
#endif
    public partial class View : UserControl, OcrDemo.Contracts.interfaces.IPluginTabbedView
    {
        public View()
        {
            InitializeComponent();
            this.Loaded += View_Loaded;
        }
        public DataContext ViewModel
        {
            get
            {
                return this.Resources["theModel"] as DataContext;
            }
        }

        public IMainView Main { get ; set ; }

        public void OnActivate()
        {
            
        }

        public void OnInit()
        {
            ViewModel.Main = Main;   
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnLaunchResultsViewer_Click(object sender, RoutedEventArgs e)
        {
            Main.SetStatus(0, "");
            if (System.IO.File.Exists(ViewModel.SampleImage)==false)
            {
                Main.SetStatus(0,"Please specify a sample image file");
                return;
            }
            OcrDemo.UI.Lib.OcrResults win = new UI.Lib.OcrResults();
            Contracts.entity.TextExtractionResults lastOcrResults = new Contracts.entity.TextExtractionResults
            {
                Results = new Contracts.entity.TextResult[]
                {
                    new Contracts.entity.TextResult
                    {
                        Text="hello",
                        X1=100, X2=150,
                        Y1=100, Y2=150
                    },
                    new Contracts.entity.TextResult
                    {
                        Text="hello 2",
                        X1=200, X2=250,
                        Y1=200, Y2=250
                    }
                }
            };
            var bytes = System.IO.File.ReadAllBytes(ViewModel.SampleImage);

            win.RenderImageWithOverlay(bytes, lastOcrResults);
            win.Show();
            ViewModel.SaveSettings();
        }
    }
}
