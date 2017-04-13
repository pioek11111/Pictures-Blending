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
    public partial class BlendingResult : Form
    {
        public BlendingResult()
        {
            InitializeComponent();
        }

        public Image ImageResult
        {
            set
            {
                pictureBox1.Image = value;
            }
        }
        private void BlendingResult_Load(object sender, EventArgs e)
        {

        }
    }
}
