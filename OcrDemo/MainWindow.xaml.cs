using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using OcrDemo.Contracts.interfaces;

namespace OcrDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView, INotifyPropertyChanged
    {
        MEFHost _hostMef;
        public event PropertyChangedEventHandler PropertyChanged;
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
                tItem.Plugin.Value.OnInit();
            }
            tItem.Plugin.Value.OnActivate();
        }

        private void InitMef()
        {
            _hostMef = new MEFHost();
            _hostMef.MainView = this;
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
                    //ctlBusy.BringIntoView();//Was tyring to reveal the progress bar
                    //But when you have a Windows Forms Host, it is going to hide everything WPF
                    //https://stackoverflow.com/questions/9920480/windowsformshost-is-always-the-most-top-from-wpf-element
                }
                else
                {
                    ctlBusy.Visibility = Visibility.Hidden;
                }
                NotifyPropertyChanged();
            }
        }

        public Lazy<IPluginOcrEngine, IPluginMetaData>[] OcrEngines
        {
            get
            {
                return _hostMef.OcrEngines;
            }
        }

        public IPluginManager Plugins
        {
            get
            {
                return new entity.PluginManager(_hostMef);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTabbedPlugins();
            LoadOcrEnginePlugins();
        }

        private void LoadOcrEnginePlugins()
        {
            //var configbuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            //var configSourceMemory = new Microsoft.Extensions.Configuration.Memory.MemoryConfigurationSource();
            //configbuilder.Add(configSourceMemory);
            //Microsoft.Extensions.Configuration.IConfiguration config = configbuilder.Build();
            //foreach (System.Configuration.SettingsProperty prop in Properties.Settings.Default.Properties)
            //{
            //    config[prop.Name] = $"{Properties.Settings.Default[prop.Name]}"; ;
            //}
            //foreach (var plugin in _hostMef.OcrEngines)
            //{
            //    var ocr = plugin.Value;

            //    ocr.OnInit(config);
            //}
        }

        private void LoadTabbedPlugins()
        {
            foreach(var plugin in _hostMef.TabbedViews)
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
        public void ActivateTab(string tabpluginname)
        {
            OcrDemo.Controls.TabItemWrapper[] tabItems = ctlTabMain.
                                                        Items.
                                                        Cast<OcrDemo.Controls.TabItemWrapper>().ToArray();
            OcrDemo.Controls.TabItemWrapper tabMatch = tabItems.
                                                        FirstOrDefault(t => (string)t.Plugin.Metadata[OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME] == tabpluginname);
            if (tabMatch == null)
            {
                MessageBox.Show($"No tab with plugin name:{tabpluginname} was found");
                return;
            }
            ctlTabMain.SelectedItem=tabMatch;
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
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MnuFile_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnuItem = e.OriginalSource as MenuItem;
            string cmd = mnuItem.DataContext as string;
            if (string.IsNullOrEmpty(cmd))
            {
                MessageBox.Show("The Menu item has not been tagged to any menu handler");
                return;
            }
            var lazy = _hostMef.CommandHandlers.FirstOrDefault(lz => (string)lz.Metadata[OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME] == cmd);
            if (lazy == null)
            {
                MessageBox.Show($"No command handler found for the command:{cmd}");
                return;
            }
            lazy.Value.Execute(null);
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
//TODO Azure Computer Vision . 1)Write a plugin using sample code at , use HTTP post
//2)https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/quickstarts/csharp-print-text
//TODO Scroll bar on pasted Image window
//TODO Scroll bar on Results window
//TODO Enable OCR button only when engine is selected (look for Converters nuget package)
//TODO Show some progress - a modal dialog with spinner OR a thin progress indicator in the toolbar
//TODO Try Windows 7 theme from Extended toolkit
//TODO Investigate if it is possible to handle and event when MEF Lazy object is initialized (Try using a Func delegate , how to tie it up to an instance?)
//TODO AWS-Investigate why are coordinates negative , use 2 lines near the upper border



