namespace SearchEngineGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectWorkDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultImageList = new System.Windows.Forms.ImageList(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(357, 415);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragEnter);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.directoryManagementToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1151, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // directoryManagementToolStripMenuItem
            // 
            this.directoryManagementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectWorkDirectoryToolStripMenuItem,
            this.rescanDirectoryToolStripMenuItem,
            this.scanDirectoryToolStripMenuItem});
            this.directoryManagementToolStripMenuItem.Name = "directoryManagementToolStripMenuItem";
            this.directoryManagementToolStripMenuItem.Size = new System.Drawing.Size(141, 20);
            this.directoryManagementToolStripMenuItem.Text = "Directory Management";
            this.directoryManagementToolStripMenuItem.Click += new System.EventHandler(this.directoryManagementToolStripMenuItem_Click);
            // 
            // selectWorkDirectoryToolStripMenuItem
            // 
            this.selectWorkDirectoryToolStripMenuItem.Name = "selectWorkDirectoryToolStripMenuItem";
            this.selectWorkDirectoryToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.selectWorkDirectoryToolStripMenuItem.Text = "Select Work Directory";
            this.selectWorkDirectoryToolStripMenuItem.Click += new System.EventHandler(this.selectWorkDirectoryToolStripMenuItem_Click);
            // 
            // rescanDirectoryToolStripMenuItem
            // 
            this.rescanDirectoryToolStripMenuItem.Name = "rescanDirectoryToolStripMenuItem";
            this.rescanDirectoryToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.rescanDirectoryToolStripMenuItem.Text = "Rescan Directory";
            this.rescanDirectoryToolStripMenuItem.Click += new System.EventHandler(this.rescanDirectoryToolStripMenuItem_Click);
            // 
            // scanDirectoryToolStripMenuItem
            // 
            this.scanDirectoryToolStripMenuItem.Name = "scanDirectoryToolStripMenuItem";
            this.scanDirectoryToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.scanDirectoryToolStripMenuItem.Text = "Scan Directory";
            this.scanDirectoryToolStripMenuItem.Click += new System.EventHandler(this.scanDirectoryToolStripMenuItem_Click);
            // 
            // resultImageList
            // 
            this.resultImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.resultImageList.ImageSize = new System.Drawing.Size(240, 240);
            this.resultImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.resultImageList;
            this.listView1.Location = new System.Drawing.Point(357, 24);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(794, 415);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.numericUpDown1.Location = new System.Drawing.Point(357, 419);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(794, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Tag = "";
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(357, 406);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Image Similarity %";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 439);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Local File Search";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectWorkDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanDirectoryToolStripMenuItem;
        private System.Windows.Forms.ImageList resultImageList;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem scanDirectoryToolStripMenuItem;
    }
}

