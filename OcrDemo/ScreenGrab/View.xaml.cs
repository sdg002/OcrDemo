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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OcrDemo.ScreenGrab
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        System.Windows.Forms.PictureBox ctlPicBox;
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

        private void BtnOCR_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OCR - to be done");
        }
    }
}
