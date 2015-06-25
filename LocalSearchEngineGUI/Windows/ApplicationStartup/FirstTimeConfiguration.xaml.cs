using System.Windows.Forms;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;


namespace LocalSearchEngineGUI.Windows.ApplicationStartup
{
    /// <summary>
    /// Interaction logic for FirstTimeConfiguration.xaml
    /// </summary>
    public partial class FirstTimeConfiguration : ModernDialog
    {
        public FirstTimeConfiguration()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.CancelButton };
            this.CancelButton.Content = "Salir";
            this.CancelButton.Click += (sender, args) => Application.Current.Shutdown();
            
        }


        private void ButtonSelectFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (!String.IsNullOrEmpty(fbd.SelectedPath))
            {
                Properties.Settings.Default.WorkingDirectory = fbd.SelectedPath;
                Properties.Settings.Default.FirstTimeRun = false;
                Properties.Settings.Default.Save();
                this.DialogResult = true;
                return;
                
            }
            this.DialogResult = false;
        }
    }
}
