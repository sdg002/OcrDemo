using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace OcrDemo.ScreenGrab
{
    [Export(typeof(ICommand))]
    [ExportMetadata(Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME, "cmdfileopen")]
    [ExportMetadata(Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_DESCRIPTION, "Open a picture file")]
    public class FileOpenCommandHandler : ICommand
    {
        [Import]
        public Contracts.interfaces.IMainView MainView { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Multiselect = false;
            d.Filter = "PNG|*.png|JPG|*.jpg|All files|*.*";
            var r = d.ShowDialog();
            if (r == DialogResult.Cancel) return;
            var lazy = this.MainView.
                        Plugins.TabbedViews.
                        Where(lz => (string)lz.Metadata[Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME] == "screengrab").
                        FirstOrDefault();
            this.MainView.ActivateTab("screengrab");
            View tabScreenGrab = lazy.Value as View;
            tabScreenGrab.LoadPicture(d.FileName);
            //TODO What next? How do you communicate with the main UI
        }
    }
}
