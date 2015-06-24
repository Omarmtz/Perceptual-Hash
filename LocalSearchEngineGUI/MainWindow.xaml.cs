using FirstFloor.ModernUI.Windows.Controls;
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
using LocalSearchEngineGUI.Classes;

namespace LocalSearchEngineGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            List<ImageInfo> items = new List<ImageInfo>();
            items.Add(new ImageInfo()
            {
                FileHashValue = "0",
                FileName = "Loco",
                FilePath = @"E:\2.jpg",
                FileSign = "ABB#B$%"
            });


            ResultView.ItemsSource = items;

        }
    }
}
