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
        private Contracts.entity.TextExtractionResults _ocrResults;
        private OcrDemo.Contracts.interfaces.IPluginMetaData _ocrEngineSelected;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        internal void DoOcr()
        {
            if (this.SelectedOcrEngine == null)
            {
                throw new InvalidOperationException("OCR engine has not been selected");
            }
            //Thread.Sleep(3000);
            var engine = Main.OcrEngines.FirstOrDefault(ocr => ocr.Metadata.Name == this.SelectedOcrEngine.Name);
            if (engine == null) throw new InvalidOperationException($"OCR engine was not found. {this.SelectedOcrEngine.Name}");
            this.LastOcrResults=engine.Value.DoOcr(this.ImageBytes);
        }
        /// <summary>
        /// HOlds a reference to the results obtained from the last successful OCR engine
        /// </summary>
        public Contracts.entity.TextExtractionResults LastOcrResults
        {
            get { return _ocrResults; }
            set { _ocrResults = value; NotifyPropertyChanged(); }
        }
        /// <summary>
        /// Brings the view model back to original state
        /// </summary>
        internal void Clear()
        {
            this.LastOcrResults = null;
            this.ImageBytes = null;
        }

        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// Bound to UI
        /// </summary>
        public OcrDemo.Contracts.interfaces.IPluginMetaData SelectedOcrEngine
        {
            get { return _ocrEngineSelected; }
            set { _ocrEngineSelected = value; NotifyPropertyChanged(); }
        }

    }
}
