using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
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

        internal void LoadPicture(string fileName)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fileName);
            ctlPicBox.Image = bmp;
            ViewModel.ImageBytes = BytesFromImage();
        }
        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCopyImage_Click(object sender, RoutedEventArgs e)
        {
            var oClip = System.Windows.Forms.Clipboard.GetImage();
            if (oClip == null) return;
            ctlPicBox.Image = oClip;
            ViewModel.ImageBytes = BytesFromImage();
        }
        /// <summary>
        /// Gets the raw bytes from the currently loaded image
        /// </summary>
        /// <returns></returns>
        private byte[] BytesFromImage()
        {
            byte[] raw = null;
            using (var mem = new System.IO.MemoryStream())
            {
                ctlPicBox.Image.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                raw = mem.ToArray();
            }
            return raw;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ctlPicBox.Image = null;
            ViewModel.Clear();
        }

        private async void BtnOCR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ViewModel.SelectedOcrEngine == null)
                {
                    MessageBox.Show("Select an OCR engine");
                    return;
                }
                if (this.ctlPicBox.Image == null)
                {
                    MessageBox.Show("No image was found. Paste some image before attempting an OCR");
                    return;
                }
                ViewModel.LastOcrResults = null;
                Task t = new Task(this.ViewModel.DoOcr);
                this.Main.IsBusy = true;
                Main.SetStatus(0, $"OCR is in progress, using engine='{ViewModel.SelectedOcrEngine.Description}'");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                t.Start();
                await t;
                this.Main.IsBusy = false;
                sw.Stop();
                Main.SetStatus(0, $"Long operation is complete. Found {ViewModel.LastOcrResults.Results.Length} text objects. Time={sw.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(),"Error");
                Main.SetStatus(0, ex.Message);
                this.Main.IsBusy = false;
            }
            finally
            {

            }
        }

        public void OnActivate()
        {
            
        }

        public void OnInit()
        {
            ViewModel.Main = Main;
            INotifyPropertyChanged notifier = this.Main as INotifyPropertyChanged;
            notifier.PropertyChanged += Notifier_PropertyChanged;
            foreach(var ocr in Main.Plugins.OcrEngines)
            {
                this.ViewModel.OcrEngines.Add(ocr);
            }
        }

        private void Notifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(this.Main.IsBusy))
            {
                ctlToolBar.IsEnabled = !this.Main.IsBusy;
            }
        }

        private void CtlLaunchViewer_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ImageBytes == null)
            {
                MessageBox.Show("No Image has been captured.");
                return;
            }
            if (ViewModel.LastOcrResults == null)
            {
                MessageBox.Show("No OCR results were found. Paste an image and click on OCR button");
                return;
            }
            /*
             * Task class did not work with ShowDialog, but Thread works very well
            if (_tskResultsDialog == null || _tskResultsDialog.IsCanceled || _tskResultsDialog.IsCompleted)
            {
                _tskResultsDialog = new Task(LaunchResultsWindow);
                _tskResultsDialog.Start();
            }
            else
            {
                //Already existing window
                _winOcrResults.RenderImage(this.ViewModel.ImageBytes);
                _winOcrResults.Activate();

            }
            */
            if (_tResults == null || _tResults.ThreadState != System.Threading.ThreadState.Running || _winOcrResults == null)
            {
                _tResults = new Thread(new ThreadStart(this.LaunchResultsWindow));
                _tResults.SetApartmentState(ApartmentState.STA);
                _tResults.Start();
            }
            else
            {
                _winOcrResults.Dispatcher.Invoke(() =>
                {
                    _winOcrResults.RenderImageWithOverlay(this.ViewModel.ImageBytes, this.ViewModel.LastOcrResults);
                    _winOcrResults.WindowState = WindowState.Normal;
                    _winOcrResults.Activate();
                });
            }
        }
        OcrDemo.UI.Lib.OcrResults _winOcrResults = null;
        Thread _tResults;
        [STAThread]
        private void LaunchResultsWindow()
        {
            _winOcrResults = new UI.Lib.OcrResults();
            _winOcrResults.RenderImageWithOverlay(this.ViewModel.ImageBytes,this.ViewModel.LastOcrResults);
            _winOcrResults.ShowDialog();
        }
    }
}
