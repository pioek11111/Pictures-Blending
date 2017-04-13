using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pwsd_lab04_a
{
    public partial class Form1 : Form
    {
        bool p1;
        bool p2;
        //int alpha;
        //int highestPercentageReached;
        Class1 c;
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            p1 = false;
            p2 = false;
            c = new Class1();
            c.progressBar1 = progressBar1;
            c.pictureBox1 = pictureBox1;
            c.pictureBox2 = pictureBox2;
            backgroundWorker1.DoWork += new DoWorkEventHandler(c.blending_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(c.blending_Completed);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(c.backgroundWorker1_ProgressChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                p1 = true;
                if (p1 && p2) button1.Enabled = true;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                p2 = true;
                if (p1 && p2) button1.Enabled = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                if (false == p1)
                {
                    screan(pictureBox1);
                    p1 = true;
                    return;
                }
                else
                {
                    screan(pictureBox2);
                    p2 = true;
                }
            }
            if (p1 && p2) button1.Enabled = true;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            c.alpha = trackBar1.Value;
            c.highestPercentageReached = 0;
            progressBar1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }
        void screan(PictureBox p)
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                               Screen.PrimaryScreen.Bounds.Height/*,
                               PixelFormat.Format32bppArgb*/);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            p.Image = bmpScreenshot;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

    }
}
