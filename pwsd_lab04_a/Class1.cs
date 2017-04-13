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
    class Class1
    {
        public ProgressBar progressBar1;
        public PictureBox pictureBox1;
        public PictureBox pictureBox2;
        public int alpha;
        public int highestPercentageReached;
        public void backgroundWorker1_ProgressChanged(object sender,
           ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }
        public void blending_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            BlendingResult bResult = new BlendingResult();
            bResult.ImageResult = e.Result as Image;
            bResult.Show();
        }
        public void blending_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
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
                    if (percentComplete > highestPercentageReached)
                    {
                        highestPercentageReached = percentComplete;
                        worker.ReportProgress(percentComplete);
                    }
                }
            }
            e.Result = blending;
        }
    }
}
