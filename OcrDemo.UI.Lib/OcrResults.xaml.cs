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
using OcrDemo.Contracts.entity;

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
        public void RenderImageWithOverlay(byte[] raw, Contracts.entity.TextExtractionResults lastOcrResults)
        {
            RenderImage(raw, lastOcrResults);
            RenderJson(lastOcrResults);
        }

        private void RenderJson(TextExtractionResults lastOcrResults)
        {
            try
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                };
                string text=Newtonsoft.Json.JsonConvert.SerializeObject(lastOcrResults, settings);
                ctlJsonViewer.Text = "";
                ctlJsonViewer.Text = text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RenderImage(byte[] raw, TextExtractionResults lastOcrResults)
        {
            Contracts.entity.TextExtractionResults ocr = null;
            if (lastOcrResults == null)
            {
                ctlStatusPanel0.Text = $"Found {lastOcrResults.Results.Length} text objects";
                //Create an empty object if none was specified
                ocr = new Contracts.entity.TextExtractionResults
                {
                    Results = new Contracts.entity.TextResult[]
                    {
                    }
                };
            }
            else
            {
                ocr = lastOcrResults;
            }
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(
                            System.Drawing.Color.FromArgb(100, System.Drawing.Color.Yellow));
            using (var memStm = new System.IO.MemoryStream(raw))
            {
                var imge = System.Drawing.Image.FromStream(memStm);
                _picBox.Image = imge;
                using (var g = System.Drawing.Graphics.FromImage(imge))
                {
                    foreach (var box in ocr.Results)
                    {
                        //var pts = new System.Drawing.Point[]
                        //{
                        //    new System.Drawing.Point((int)box.X1,(int)box.Y1),
                        //    new System.Drawing.Point((int)box.X2,(int)box.Y1),
                        //    new System.Drawing.Point((int)box.X2,(int)box.Y2),
                        //    new System.Drawing.Point((int)box.X1,(int)box.Y2),
                        //    new System.Drawing.Point((int)box.X1,(int)box.Y1)
                        //};
                        //g.DrawLines(pen,pts);
                        int xUpperLeft = (int)Math.Min(box.X1, box.X2);
                        int yUpperLeft = (int)Math.Min(box.Y1, box.Y2);
                        float width = (float)Math.Abs(box.X1 - box.X2);
                        float ht = (float)Math.Abs(box.Y1 - box.Y2);
                        g.DrawRectangle(pen, xUpperLeft, yUpperLeft, width, ht);
                        g.FillRectangle(b, xUpperLeft, yUpperLeft, width, ht);
                    }
                }
            }
        }
    }
}
