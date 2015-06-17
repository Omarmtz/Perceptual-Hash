using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageFilesProcessor;
using LocalSearchEngine.FileManager;
using PHash;

namespace SearchEngineGUI
{
    public partial class Form1 : Form
    {
        private String _workingFolder;
        private FileAgent _fileAgent;
        private readonly MediaObjectsHasher _imageHasher;

        public static string[] ExtensionImagesContentFile = { ".docx", ".odt", ".pptx" };
        public Form1()
        {
            InitializeComponent();
            _imageHasher = new MediaObjectsHasher();
            this.AllowDrop = true;
            pictureBox1.AllowDrop = true;
            listView1.View = View.LargeIcon;
            _fileAgent = new FileAgent();
        }

        private void selectWorkDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            _workingFolder = fbd.SelectedPath;
            _fileAgent.SetInitialDirectory(_workingFolder);

        }

        private void rescanDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileAgent.UpdateIndexation();
            _imageHasher.ScanDatabaseSystem();
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            pictureBox1.Image = Image.FromFile(files.First());

            var results = _imageHasher.GetImageSimilarities(files.First(), (int)this.numericUpDown1.Value,MediaObjectsHasher.HashMethod.BlockMeanMethod4Overlapped);
            
            resultImageList.Images.Clear();

            int i = 0;

            
            listView1.Items.Clear();
            resultImageList.Images.Clear();
            listView1.LargeImageList = resultImageList;

            try
            {
                foreach (var documentImage1 in results)
                {
                    if (ExtensionImagesContentFile.Any(p=> p.Equals(Path.GetExtension(documentImage1))))
                    {
                        listView1.Items.Add(documentImage1, i++);
                    }
                    else
                    {
                        using (var fs = new FileStream(documentImage1, FileMode.Open, FileAccess.Read))
                        {
                            using (var img = Image.FromStream(fs, true, false))
                            {
                                resultImageList.Images.Add((Bitmap)img);
                                listView1.Items.Add(documentImage1, i++);
                            }
                        }
                        
                    }
                    
                }
            }
            catch (Exception)
            {
                
            }
           
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void directoryManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;

                ListViewItem lvItem = items[0];
                string what = lvItem.Text;
                Process.Start(what);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
     
        private void scanDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var task = Task.Factory.StartNew(_fileAgent.InitializeIndexation);

            MessageBox.Show("Proceso de indexacion automatica iniciado...");

            task.GetAwaiter().OnCompleted(() =>
            {
                MessageBox.Show("Terminado de procesar indexacion, Iniciando generacion hashing");
                _imageHasher.ScanDatabaseSystem();
                MessageBox.Show("Hashing Terminado");
            });
        }
    }
}
