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
    public partial class MainWindow : Window
    {
        List<TabPlugin> _plugins = new List<TabPlugin>();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            Plugins.Add(new TabPlugin("OcrDemo.ScreenGrab.View", "Simple Screen grab"));
            Plugins.Add(new TabPlugin("OcrDemo.Settings.View", "Settings"));
        }

        internal List<TabPlugin> Plugins { get => _plugins;  }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            foreach(var plugin in this.Plugins)
            {
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
            }
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
