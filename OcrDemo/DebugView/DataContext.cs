using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OcrDemo.Contracts.interfaces;

namespace OcrDemo.DebugView
{
    public class DataContext : INotifyPropertyChanged
    {
        public DataContext()
        {
            _fileSample = SettingsDevOnly.Default.file_samplepicture;
        }
        private string _fileSample;
        public IMainView Main { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string SampleImage
        {
            get { return _fileSample; }
            set { _fileSample = value; NotifyPropertyChanged(); }
        }
        internal void SaveSettings()
        {
            SettingsDevOnly.Default.file_samplepicture = this.SampleImage;
            SettingsDevOnly.Default.Save();
        }
    }
}
