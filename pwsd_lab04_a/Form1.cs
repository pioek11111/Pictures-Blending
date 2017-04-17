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
        bool bgWorker1;
        bool bgWorker2;
        int backgroundCounter;
        //int imagesCounter;
        public int alpha1;
        public int alpha2;
        public int highestPercentageReached1;
        public int highestPercentageReached2;
        
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += userControl11.UserControl1_KeyDown;
            button1.Enabled = false;
            p1 = false;
            p2 = false;
            bgWorker1 = false;
            bgWorker2 = false;
            backgroundCounter = 0;
            backgroundWorker1.DoWork += new DoWorkEventHandler(blending_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(blending_Completed);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker2.DoWork += new DoWorkEventHandler(blending_DoWork);
            backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(blending_Completed);
            backgroundWorker2.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(userControl11.ImageChecked != null)
            {
                pictureBox1.Image = userControl11.ImageChecked.Image;
                p1 = true;
                if (p1 && p2) button1.Enabled = true;
                return;
            }

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
            if (userControl11.ImageChecked != null)
            {
                pictureBox2.Image = userControl11.ImageChecked.Image;
                p2 = true;
                if (p1 && p2) button1.Enabled = true;
                return;
            }
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
            //c.u = userControl11;
            progressBar1.Visible = true;
            backgroundCounter++;
            if (!bgWorker1)
            {
                alpha1 = trackBar1.Value;
                highestPercentageReached1 = 0;
                backgroundWorker1.RunWorkerAsync();
                bgWorker1 = true;
                return;
            }
            if (!bgWorker2)
            {
                alpha2 = trackBar1.Value;
                highestPercentageReached2 = 0;
                progressBar2.Visible = true;
                backgroundWorker2.RunWorkerAsync();
                bgWorker2 = true;
            }
            if (backgroundCounter == 2) button1.Enabled = false;
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
        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == backgroundWorker1)
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
            if (worker == backgroundWorker2)
            {
                this.progressBar2.Value = e.ProgressPercentage;
            }
        }
        public void blending_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == backgroundWorker1)
            {
                backgroundCounter--;
                bgWorker1 = false;
                progressBar1.Visible = false;
            }
            if (worker == backgroundWorker2)
            {
                backgroundCounter--;
                bgWorker2 = false;
                progressBar2.Visible = false;
            }
            if (backgroundCounter < 2) button1.Enabled = true;
            BlendingResult bResult = new BlendingResult(userControl11);
            bResult.ImageResult = e.Result as Image;
            
            bResult.Show();
        }
        public void blending_DoWork(object sender, DoWorkEventArgs e)
        {
            int alpha;            
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == backgroundWorker1) alpha = alpha1;
            else alpha = alpha2;
            worker.WorkerReportsProgress = true;
            var blending = new Bitmap(Math.Min(pictureBox1.Image.Width, pictureBox2.Image.Width),
                                      Math.Min(pictureBox1.Image.Height, pictureBox2.Image.Height));
            int percentComplete;
            Bitmap bmp1 = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(pictureBox2.Image);
            Color c;
            for (int i = 0; i < blending.Height; i++)
            {
                for (int j = 0; j < blending.Width; j++)
                {
                    c = Color.FromArgb(bmp1.GetPixel(j, i).R * alpha / 100 + bmp2.GetPixel(j, i).R * (100 - alpha) / 100,
                                       bmp1.GetPixel(j, i).G * alpha / 100 + bmp2.GetPixel(j, i).G * (100 - alpha) / 100,
                                       bmp1.GetPixel(j, i).B * alpha / 100 + bmp2.GetPixel(j, i).B * (100 - alpha) / 100);
                    blending.SetPixel(j, i, c);
                    percentComplete = 100 * i * j / (blending.Height * blending.Width);
                    percentComplete++;
                    if (worker == backgroundWorker1 && percentComplete > highestPercentageReached1)
                    {
                        highestPercentageReached1 = percentComplete;
                        worker.ReportProgress(percentComplete);
                    }
                    if (worker == backgroundWorker2 && percentComplete > highestPercentageReached2)
                    {
                        highestPercentageReached2 = percentComplete;
                        worker.ReportProgress(percentComplete);
                    }
                }
            }
            e.Result = blending;
        }
    }
}
