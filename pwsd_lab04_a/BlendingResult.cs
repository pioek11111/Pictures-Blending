using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace pwsd_lab04_a
{
    public partial class BlendingResult : Form
    {
        UserControl1 us;
        int imagesCounter;
        string path;
        public BlendingResult(UserControl1 u)
        {
            InitializeComponent();
            path = null;
            us = u;
            pictureBox1.MouseDown += menu;
            imagesCounter = 0;
        }

        public void menu(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
        }
        public Image ImageResult
        {
            set
            {
                pictureBox1.Image = value;
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "newImage" + imagesCounter.ToString();
            DialogResult result = saveFileDialog1.ShowDialog();
            path = saveFileDialog1.FileName;
            // TODO zmienic file na path
            if (result == DialogResult.OK) // Test result.
            {
                try
                {                    
                    pictureBox1.Image.Save(path);
                    imagesCounter++;
                }
                catch (IOException)
                {
                }
            }
        }
        private void BlendingResult_Load(object sender, EventArgs e)
        {

        }

        private void addToLiblaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("You need to save image");
                return;
            }
            //us.addImageToLibrary(pictureBox1.Image);
            us.addImageToLibrary(path);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
