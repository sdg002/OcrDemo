using System;
using System.Collections.Generic;
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
        public OcrDemo.Contracts.interfaces.IMainView  Main { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void DoOcr()
        {
            Thread.Sleep(3000);
        }
    }
}
