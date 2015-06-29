using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FirstFloor.ModernUI.Windows.Controls;
using ImageFilesProcessor;
using ImageFilesProcessor.Classes;
using LocalSearchEngine.FileManager;
using LocalSearchEngineGUI.Windows.Configuration;
using Microsoft.Win32;
using Image = System.Drawing.Image;

namespace LocalSearchEngineGUI.Windows.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private String _workingFolder;
        private FileAgent _fileAgent;
        private MediaObjectsHasher _imageHasher;
        private string _fileSelected;

        private ModernDialog hashValues;

        public static string[] ExtensionImagesContentFile = { ".docx", ".odt", ".pptx" };
        public MainWindow()
        {
            InitializeComponent();
            _imageHasher = new MediaObjectsHasher();
            _fileAgent = new FileAgent();
        }

        public void ProcessIndexation()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.WorkingDirectory))
            {
                _fileAgent.SetInitialDirectory(Properties.Settings.Default.WorkingDirectory);
                var task = Task.Factory.StartNew(_fileAgent.InitializeIndexation);

                var modern = new ModernDialog();
                modern.Owner = this;
                modern.Title = "Indexando";
                modern.Content = "Proceso de indexacion automatica espere...";

                foreach (var button in modern.Buttons)
                {
                    button.Visibility = Visibility.Hidden;
                }

                task.GetAwaiter().OnCompleted(() => IndexationHashStart(modern));
                modern.ShowDialog();
            }
        }

        private void IndexationHashStart(ModernDialog modern)
        {
            modern.Close();

            hashValues = new ModernDialog();
            hashValues.Owner = this;

            hashValues.Title = "Calculando Valores Hash";

            foreach (var button in hashValues.Buttons)
            {
                button.Visibility = Visibility.Hidden;
            }

            var hashTask = Task.Factory.StartNew(() => _imageHasher.ScanDatabaseSystem(new Progress<Tuple<int, int>>(ReportProgress)));
            hashTask.GetAwaiter().OnCompleted(hashValues.Close);
            hashValues.ShowDialog();
        }

        private void ReportProgress(Tuple<int, int> reportProgress)
        {
            Dispatcher.Invoke(() =>
            {
                if (reportProgress.Item1 % 10 == 0)
                {
                    hashValues.Content = String.Format("{0:0.00}% COMPLETADO \n{1} DE {2} FIRMAS", (reportProgress.Item1 / (float)reportProgress.Item2) * 100,
                    reportProgress.Item1,
                    reportProgress.Item2);
                }   
            }, DispatcherPriority.Normal);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock)
            {
                var textControl = (sender as TextBlock);
                if (!String.IsNullOrEmpty(textControl.Text))
                {
                    Process.Start(textControl.Text);
                }
            }
        }

        private void ResultView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox)
            {
                var listBox = (sender as ListBox);
                if (listBox.SelectedItem != null)
                {
                    var imageInfo = (listBox.SelectedItem as ImageInfo);
                    if (imageInfo != null) Process.Start(imageInfo.FilePath);
                }
            }
        }

        private void ImagePictureBox_OnDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                _fileSelected = files.First();

                ImagePictureBox.Source = new BitmapImage(new Uri(_fileSelected, UriKind.RelativeOrAbsolute));
                SetImageInfoLabels(new FileInfo(_fileSelected));
            }
            catch (Exception)
            {
                MessageBox.Show("Archivo no es imagen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ResultView.ItemsSource = null;
        }

        private void ButtonLoadImage_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;.bmp;*.jpg;*.png;*.jpeg;*.tif;*.tiff;*.jif;*.jfif;*.jp2;*.jpx;*.j2k;*.j2c)|*.png;*.jpeg;.bmp;*.jpg;*.png;*.jpeg;*.tif;*.tiff;*.jif;*.jfif;*.jp2;*.jpx;*.j2k;*.j2c";
            if (openFileDialog.ShowDialog() == true)
            {
                _fileSelected = openFileDialog.FileName;
                ImagePictureBox.Source = new BitmapImage(new Uri(_fileSelected, UriKind.RelativeOrAbsolute));
                SetImageInfoLabels(new FileInfo(_fileSelected));
            }
            ResultView.ItemsSource = null;

        }

        private void SetImageInfoLabels(FileInfo fileInfo)
        {
            TxtFileName.Text = fileInfo.FullName;
            TxtChangeDate.Text = fileInfo.LastWriteTime.ToShortDateString();
            TxtFileSize.Text = String.Format("{0} Kb", (fileInfo.Length / 1024));


            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var img = Image.FromStream(fs, true, false))
                {
                    TxtFileDimensions.Text = String.Format("Ancho {0} Largo {1}", img.Width, img.Height);
                }
            }

        }

        private void ButtonSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var method = (MediaObjectsHasher.HashMethod)Enum.Parse(typeof(MediaObjectsHasher.HashMethod), Properties.Settings.Default.SearchMethod);

            ResultView.ItemsSource = _imageHasher.GetImageSimilarities(_fileSelected, Properties.Settings.Default.Similarity, method);
        }

        private void ButtonConfigurationSearch_OnClick(object sender, RoutedEventArgs e)
        {
            var configWindow = new ConfigurationWindow();
            configWindow.Owner = this;
            configWindow.ShowDialog();
        }

    }
}
