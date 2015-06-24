using System.Diagnostics;
using System.IO;
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
using ImageFilesProcessor.Classes;
using LocalSearchEngine.FileManager;
using Microsoft.Win32;
using Image = System.Drawing.Image;

namespace LocalSearchEngineGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private String _workingFolder;
        private FileAgent _fileAgent;
        private readonly MediaObjectsHasher _imageHasher;
        private string _fileSelected;

        public static string[] ExtensionImagesContentFile = { ".docx", ".odt", ".pptx" };
        public MainWindow()
        {
            InitializeComponent();

            _imageHasher = new MediaObjectsHasher();
            _fileAgent = new FileAgent();
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
            TxtFileSize.Text = String.Format("{0} Kb", (fileInfo.Length/1024));
            TxtFileSign.Text = String.Format("x{0}",0000000);
            
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
            //TODO ONLY TEST PERCENTAGE
            ResultView.ItemsSource = _imageHasher.GetImageSimilarities(_fileSelected, 90, MediaObjectsHasher.HashMethod.DctMethod);
        }
    }
}
