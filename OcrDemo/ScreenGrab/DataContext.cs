using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OcrDemo.ScreenGrab
{
    public class DataContext : INotifyPropertyChanged
    {
        ObservableCollection<OcrDemo.Contracts.interfaces.IPluginMetaData> _engines = new ObservableCollection<OcrDemo.Contracts.interfaces.IPluginMetaData>();
        public OcrDemo.Contracts.interfaces.IMainView  Main { get; set; }
        public ObservableCollection<OcrDemo.Contracts.interfaces.IPluginMetaData> OcrEngines { get => _engines;  }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Bound to UI
        /// </summary>
        public OcrDemo.Contracts.interfaces.IPluginMetaData SelectedOcrEngine { get; set; }
        internal void DoOcr()
        {
            if (this.SelectedOcrEngine == null)
            {
                throw new InvalidOperationException("OCR engine has not been selected");
            }
            //Thread.Sleep(3000);
            var engine = Main.OcrEngines.FirstOrDefault(ocr => ocr.Metadata.Name == this.SelectedOcrEngine.Name);
            if (engine == null) throw new InvalidOperationException($"OCR engine was not found. {this.SelectedOcrEngine.Name}");
            this.LasOcrResults=engine.Value.DoOcr(this.ImageBytes);
        }
        public Contracts.entity.TextExtractionResults LasOcrResults { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
