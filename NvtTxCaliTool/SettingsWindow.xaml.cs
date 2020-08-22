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


namespace NvtTxCaliTool
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindowViewModel View { private set; get; }
        public SettingsWindow(string config)
        {
            InitializeComponent();
            this.View = new SettingsWindowViewModel();
            this.View.LoadConfig(config);
            this.DataContext = this.View;
        }


        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Load Config File";
            fileDialog.Filter = "Json File(*.json)|*.json";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                this.View.LoadConfig(file);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.Title = "Save Config File";
            fileDialog.Filter = "Json File(*.json)|*.json";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                this.View.SaveConfig(file);
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
