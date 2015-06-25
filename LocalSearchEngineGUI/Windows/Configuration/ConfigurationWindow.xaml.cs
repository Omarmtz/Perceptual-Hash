using System.IO;
using System.Threading;
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
using ImageFilesProcessor;
using Path = System.IO.Path;

namespace LocalSearchEngineGUI.Windows.Configuration
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : ModernWindow
    {
        private int _similarityValue;
        private MediaObjectsHasher.HashMethod _searchMethod;
        public ConfigurationWindow()
        {
            InitializeComponent();
            _similarityValue = (int)Properties.Settings.Default.Similarity;
            _searchMethod = (MediaObjectsHasher.HashMethod) Enum.Parse(typeof (MediaObjectsHasher.HashMethod), Properties.Settings.Default.SearchMethod);
            
            SetSearchMethod(_searchMethod);
            SliderSimilarity.Value = _similarityValue;
        }

        private void SetSearchMethod(MediaObjectsHasher.HashMethod searchMethod)
        {
            switch (searchMethod)
            {
                case MediaObjectsHasher.HashMethod.BlockMeanMethod1UnOverlapped:
                    RadioButtonBlck1.IsChecked = true;
                    break;
                case MediaObjectsHasher.HashMethod.BlockMeanMethod2Overlapped:
                    RadioButtonBlck2.IsChecked = true;
                    break;
                case MediaObjectsHasher.HashMethod.BlockMeanMethod3UnOverlapped:
                    RadioButtonBlck3.IsChecked = true;
                    break;
                case MediaObjectsHasher.HashMethod.BlockMeanMethod4Overlapped:
                    RadioButtonBlck4.IsChecked = true;
                    break;
                case MediaObjectsHasher.HashMethod.DctMethod:
                    RadioButtonDct.IsChecked = true;
                    break;
            }
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelSliderValue != null)
            {
                this.LabelSliderValue.Content = String.Format("{0}%",(int)e.NewValue);
                Properties.Settings.Default.Similarity = (int) e.NewValue;
            }
            
        }

        private void RadioButtonDct_OnChecked(object sender, RoutedEventArgs e)
        {
            _searchMethod = MediaObjectsHasher.HashMethod.DctMethod;
            Properties.Settings.Default.SearchMethod = _searchMethod.ToString();
        }

        private void RadioButtonBlck1_OnChecked(object sender, RoutedEventArgs e)
        {
            _searchMethod = MediaObjectsHasher.HashMethod.BlockMeanMethod1UnOverlapped;
            Properties.Settings.Default.SearchMethod = _searchMethod.ToString();
        }

        private void RadioButtonBlck2_OnChecked(object sender, RoutedEventArgs e)
        {
            _searchMethod = MediaObjectsHasher.HashMethod.BlockMeanMethod2Overlapped;
            Properties.Settings.Default.SearchMethod = _searchMethod.ToString();
        }

        private void RadioButtonBlck3_OnChecked(object sender, RoutedEventArgs e)
        {
            _searchMethod = MediaObjectsHasher.HashMethod.BlockMeanMethod3UnOverlapped;
            Properties.Settings.Default.SearchMethod = _searchMethod.ToString();
        }

        private void RadioButtonBlck4_OnChecked(object sender, RoutedEventArgs e)
        {
            _searchMethod = MediaObjectsHasher.HashMethod.BlockMeanMethod4Overlapped;
            Properties.Settings.Default.SearchMethod = _searchMethod.ToString();
        }
        
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonRestart_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.WorkingDirectory = String.Empty;
            Properties.Settings.Default.FirstTimeRun = true;
            Properties.Settings.Default.Similarity = 90;
            Properties.Settings.Default.SearchMethod = MediaObjectsHasher.HashMethod.DctMethod.ToString();

            Properties.Settings.Default.Save();

            Directory.Delete(System.IO.Path.Combine(Path.GetTempPath(), "ImageSearchingTempFiles"),true);

            var message = new ModernDialog();
            message.Owner = this;
            message.Title = "Reiniciando el sistema";
            message.Content = "Vuelva a iniciar la aplicacion para que los cambios tengan efecto.";
            message.ShowDialog();

            Application.Current.Shutdown();
        }
    }
}
