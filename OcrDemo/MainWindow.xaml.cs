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

namespace OcrDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, OcrDemo.Contracts.interfaces.IMainView
    {
        MEFHost _hostMef;
        public MainWindow()
        {
            InitializeComponent();
            InitMef();
            this.Loaded += MainWindow_Loaded;
            ctlTabMain.SelectionChanged += CtlTabMain_SelectionChanged;
        }

        private void CtlTabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            var tItem = tab.SelectedItem as Controls.TabItemWrapper;
            if (tItem.Plugin.IsValueCreated==false)
            {
                tItem.Content = tItem.Plugin.Value;
                tItem.Plugin.Value.Main = this;
            }
            tItem.Plugin.Value.OnActivate();
        }

        private void InitMef()
        {
            _hostMef = new MEFHost();

        }

        public bool IsBusy
        {
            get
            {
                if (ctlBusy.Visibility == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    ctlBusy.Visibility = Visibility.Visible;
                    ctlBusy.BringIntoView();
                }
                else
                {
                    ctlBusy.Visibility = Visibility.Hidden;
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            foreach(var plugin in _hostMef.Plugins)
            {
                var tItem = new OcrDemo.Controls.TabItemWrapper();
                tItem.Plugin = plugin;
                tItem.Header = plugin.Metadata[OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_TAB_TITLE];
                ctlTabMain.Items.Add(tItem);
                /*
                TabItem tItem = new TabItem();
                Type tyPlugin=Type.GetType(plugin.TypeName, false, false);
                Object ctl=Activator.CreateInstance(tyPlugin);
                if (ctl == null)
                {
                    MessageBox.Show($" Could not load plugin View:{tItem}");
                }
                tItem.Content = ctl;
                tItem.Header = plugin.TabTitle;
                ctlTabMain.Items.Add(tItem);
                */
            }
        }

        public void SetStatus(int panel, string text)
        {
            if (panel != 0) throw new ArgumentException($"The panel index should be 0");
            ctlStatusPanel0.Text = text;
        }

        public string GetStatus(int panel, string text)
        {
            if (panel != 0) throw new ArgumentException($"The panel index should be 0");
            return ctlStatusPanel0.Text;
        }
    }
    /// <summary>
    /// Represents a Tabbed view which will be added dynamically to the main Tabbed view
    /// </summary>
    class TabPlugin
    {
        public TabPlugin(string type,string title)
        {
            this.TypeName = type;
            this.TabTitle = title;
        }
        /// <summary>
        /// The class name of the User control
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Title of the Tab view
        /// </summary>
        public string TabTitle { get; set; }
        public override string ToString()
        {
            return $"Type={TypeName}    Title={TabTitle}";
        }
    }
}

//TODO Get user control from MEF demo for progrss bar
//TODO Create OCR account in AWS
//TODO Do a simple OCR and examine the output
