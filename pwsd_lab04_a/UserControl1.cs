using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace pwsd_lab04_a
{
    public partial class UserControl1 : UserControl
    {
        Dictionary<PictureBox,string> pictureBoxes;
        const int xMargin = 20;
        int yMargin;
        const int size = 150;
        int columns;
        public UserControl1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            pictureBoxes = new Dictionary<PictureBox, string>();
            this.Resize += resize;
            columns = 1;
            yMargin = 20;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
            

            if (File.Exists("Library.xml"))
            { 
                List<string> tmp = Deserialize();
                foreach (var path in tmp)
                    addImageToLibrary(path, false);
            }
        }
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            foreach (string file in files)
            {
                if (file.EndsWith(".jpg") || file.EndsWith(".bmp") || file.EndsWith(".png"))
                {
                    if(!pictureBoxes.ContainsValue(file)) addImageToLibrary(file);
                }
            }
            
        }

        private PictureBox imageChecked;
        public PictureBox ImageChecked
        {
            get { return imageChecked; }
        }

        public void resize(object sender, EventArgs e)
        {
            columns = (this.Size.Width - xMargin) / (size + xMargin);
            if (columns == 0) return;
            int counter = 0;
            int x,y;
            //foreach(var p in listOfPictureBox)
            foreach(PictureBox p in pictureBoxes.Keys)
            {
                x = xMargin + (counter % columns) * (size + xMargin);
                y = xMargin + (counter / columns) * (size + xMargin);
                p.Location = new Point(x, y);
                counter++;
            }
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        public void addImageToLibrary(string path, bool serialize = true)
        {
            PictureBox p = new PictureBox();
            pictureBoxes.Add(p, path);
            p.Location = new Point(xMargin, yMargin);
            yMargin += size + xMargin;
            p.Size = new Size(size, size);
            p.Image = Image.FromFile(path);
         
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            p.Visible = true;
            p.BorderStyle = BorderStyle.FixedSingle;
            p.Padding = new Padding(5, 5, 5, 5);
            p.MouseDown += menu;
            p.MouseDoubleClick += doubleClick;
            p.BackColor = Color.White;
            resize(this, null);
            this.Controls.Add(p);
            if(serialize) SerializeObject(pictureBoxes.Values.ToList());
        }
        public void menu(object sender, MouseEventArgs e)
        {
            
            PictureBox p = sender as PictureBox;
            if (e.Button == MouseButtons.Left)
            {
                if (p.BackColor == Color.White)
                {
                    if (imageChecked != null) imageChecked.BackColor = Color.White;
                    p.BackColor = Color.Orange;
                    imageChecked = sender as PictureBox;
                }
                else
                {
                    p.BackColor = Color.White;
                    imageChecked = null;
                }
            }
        }
        public void doubleClick(object sender, MouseEventArgs e)
        {
            return;
        }

        public void UserControl1_KeyDown(object sender, KeyEventArgs e)
        {  
            if (e.KeyCode == Keys.Delete)
            {
                if (imageChecked != null)
                {
                    this.Controls.Remove(imageChecked);
                    delete(pictureBoxes[imageChecked]);
                    pictureBoxes.Remove(imageChecked);                    
                    imageChecked = null;
                    resize(this, null);
                    
                   // SerializeObject(pictureBoxes.Values.ToList());
                }
            }
        }
        public void delete(string p)
        {
            XDocument doc = XDocument.Load(@"Library.xml");
            doc.Descendants("ArrayOfString")
            .Elements("string")
            .Where(x => x.Value == p)
            .Remove();
            doc.Save("Library.xml");
        }
        public static void SerializeObject(List<string> list, string fileName = "Library.xml")
        {
            var serializer = new XmlSerializer(typeof(List<string>));
            using (var stream = File.OpenWrite(fileName))
            {
                serializer.Serialize(stream, list);
            }
        }
        public static List<string> Deserialize( string fileName = "Library.xml")
        {
            List<string> list = new List<string>();
            var serializer = new XmlSerializer(typeof(List<string>));
            using (var stream = File.OpenRead(fileName))
            {
                var other = (List<string>)(serializer.Deserialize(stream));
                list.Clear();
                list.AddRange(other);
            }
            return list;
        }
    }
}