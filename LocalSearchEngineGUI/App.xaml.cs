using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LocalSearchEngineGUI.Windows.ApplicationStartup;
using LocalSearchEngineGUI.Windows.Configuration;
using LocalSearchEngineGUI.Windows.Main;

namespace LocalSearchEngineGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.Visibility = Visibility.Hidden;
            
            if (LocalSearchEngineGUI.Properties.Settings.Default.FirstTimeRun)
            {
                // Create the startup window
                var wnd = new FirstTimeConfiguration();
                wnd.Owner = mainWindow;
                
                var showDialog = wnd.ShowDialog();
                wnd.Close();
                if (showDialog != null && showDialog.Value)
                {
                    mainWindow.Visibility= Visibility.Visible;
                    mainWindow.ProcessIndexation();
                }
                
            }
            else
            {
                mainWindow.Visibility = Visibility.Visible;
            }
           
        }
    }
}
