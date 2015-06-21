using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbnailChanger
{
    public partial class Form2 : Form
    {
        Image picQr;
        int iHeight;
        int iWidth;
        public Form2(Image thumbnailImage, int height, int width)
        {
            InitializeComponent();
            picQr = thumbnailImage;
            iHeight = height;
            iWidth = width;
        }

        internal System.Drawing.Image ImageParameter { get; private set; }


        private void Form2_Load(object sender, EventArgs e)
        {
            Form1 Form1Object = new Form1();
            picThumbnail.Image = picQr;
            this.Height = iHeight;
            this.Width = iWidth;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
