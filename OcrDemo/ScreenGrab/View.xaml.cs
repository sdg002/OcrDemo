using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace OcrDemo.ScreenGrab
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    [Export(typeof(OcrDemo.Contracts.interfaces.IPluginTabbedView))]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME, "screengrab")]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_TAB_TITLE, "Screen grab")]
    public partial class View : UserControl, OcrDemo.Contracts.interfaces.IPluginTabbedView
    {
        System.Windows.Forms.PictureBox ctlPicBox;
        public DataContext ViewModel
        {
            get
            {
                return this.Resources["theModel"] as DataContext;
            }
        }

        public IMainView Main { get ; set ; }

        public View()
        {
            InitializeComponent();
            ctlPicBox = new System.Windows.Forms.PictureBox();
            ctlHost.Child = ctlPicBox;
            this.Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCopyImage_Click(object sender, RoutedEventArgs e)
        {
            var oClip = System.Windows.Forms.Clipboard.GetImage();
            if (oClip == null) return;
            ctlPicBox.Image = oClip;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ctlPicBox.Image = null;
        }

        private async void BtnOCR_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("OCR - to be done");
            Task t = new Task(this.ViewModel.DoOcr);
            this.Main.IsBusy = true;
            Main.SetStatus(0, "Long operation is in progress");
            t.Start();
            await t;
            this.Main.IsBusy = false;
            Main.SetStatus(0, "Long operation is complete");
        }

        public void OnActivate()
        {
            
        }

        public void OnInit()
        {
            INotifyPropertyChanged notifier = this.Main as INotifyPropertyChanged;
            notifier.PropertyChanged += Notifier_PropertyChanged;
        }

        private void Notifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(this.Main.IsBusy))
            {
                ctlToolBar.IsEnabled = !this.Main.IsBusy;
            }
        }
    }
}
