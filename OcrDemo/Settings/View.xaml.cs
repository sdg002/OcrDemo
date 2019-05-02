using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Diagnostics;
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
using OcrDemo.Contracts.interfaces;

namespace OcrDemo.Settings
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    [Export(typeof(OcrDemo.Contracts.interfaces.IPluginTabbedView))]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGINNAME, "settings")]
    [ExportMetadata(OcrDemo.Contracts.Constants.MEF_ATTRIBUTE_PLUGIN_TAB_TITLE, "Settings")]
    public partial class View : UserControl, OcrDemo.Contracts.interfaces.IPluginTabbedView
    {
        public View()
        {
            InitializeComponent();
        }

        public IMainView Main { get ; set ; }

        public void OnActivate()
        {
            Main.SetStatus(0, "");
        }

        public void OnInit()
        {
            //This approach of populating the property grid did not work. You have to use SelectedObject
            //foreach (SettingsProperty prop in Properties.Settings.Default.Properties)
            //{
            //    //object value = Properties.Settings.Default[prop.Name];
            //    //Trace.WriteLine($"Prop name={prop.Name}  Value={value}  Property Type={prop.PropertyType.Name}");
            //    Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition defn = new Xceed.Wpf.Toolkit.PropertyGrid.PropertyDefinition();
            //    defn.DisplayName = prop.Name;
            //    defn.TargetProperties = new string[] { prop.Name};
            //    defn.IsExpandable = false;
            //    ctlPropGrid.PropertyDefinitions.Add(defn);
            //}

            //This approach of pre=populating the Settings did not work either
            //var propNew = new SettingsProperty("cooldynamic");
            //propNew.PropertyType = typeof(string);
            //Properties.Settings.Default.Properties.Add(propNew);

            //I was trying to forcibly set a Category - did not work!
            //int index = 0;
            //foreach(var prop in ctlPropGrid.PropertyDefinitions)
            //{
            //    prop.Category = "BLAH" + index.ToString();
            //}
            //ctlPropGrid.Update();

            ctlPropGrid.SelectedObject = Properties.Settings.Default;
            ctlPropGrid.AutoGenerateProperties = true;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            Main.SetStatus(0, "Settings saved");
        }
    }
}
