using System;
using System.Collections.Generic;
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

namespace OcrDemo.UI.Lib
{
    /// <summary>
    /// Interaction logic for OcrResults.xaml
    /// </summary>
    public partial class OcrResults : Window
    {
        System.Windows.Forms.PictureBox _picBox;
        public OcrResults()
        {
            InitializeComponent();
            _picBox = new System.Windows.Forms.PictureBox();
            ctlWindowsFormsHost.Child = _picBox;
            this.Loaded += OcrResults_Loaded;
        }

        private void OcrResults_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        public void RenderImage(byte[] raw, Contracts.entity.TextExtractionResults lastOcrResults)
        {
            if (lastOcrResults != null)
            {
                ctlStatusPanel0.Text = $"Found {lastOcrResults.Results.Length} text objects";
            }
            using (var memStm = new System.IO.MemoryStream(raw))
            {
                var imge = System.Drawing.Image.FromStream(memStm);
                _picBox.Image = imge;
            }
                
        }
    }
}
