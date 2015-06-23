using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbnailChanger
{
    public partial class Form1 : Form
    {
        Image image_Form1;
        public Image Image_Form1
        {
            get { return image_Form1; }
            set { image_Form1 = value; }
        }

        public Form1()
        {
            InitializeComponent();
            btnCaptureThumbnail.Enabled = false;
            btnScrubView.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            try
            { 
                axWindowsMediaPlayer1.URL = openFileDialog1.FileName;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                btnScrubView.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private Graphics g = null;

        private void btnCaptureThumbnail_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();

            System.Drawing.Image ret = null;
            try
            {
                // take picture BEFORE saveFileDialog pops up!!
                Bitmap bitmap = new Bitmap(axWindowsMediaPlayer1.Width, axWindowsMediaPlayer1.Height);
                {
                    Graphics g = Graphics.FromImage(bitmap);
                    {
                        Graphics gg = axWindowsMediaPlayer1.CreateGraphics();
                        {
                            //timerTakePicFromVideo.Start();
                            this.BringToFront();
                            g.CopyFromScreen(
                                axWindowsMediaPlayer1.PointToScreen(
                                    new System.Drawing.Point()).X,
                                axWindowsMediaPlayer1.PointToScreen(
                                    new System.Drawing.Point()).Y,
                                0, 0,
                                new System.Drawing.Size(
                                    axWindowsMediaPlayer1.Width,
                                    axWindowsMediaPlayer1.Height -60)
                                );
                        }
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ret = System.Drawing.Image.FromStream(ms);
                        Form2 frmViewImage = new Form2(ret, ret.Height, ret.Width);
                        frmViewImage.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 3)
            {
                btnCaptureThumbnail.Enabled = true;
            }
        }

        private void btnScrub_Click(object sender, EventArgs e)
        {
            Form3 frmScrub = new Form3(axWindowsMediaPlayer1.URL);
            frmScrub.Form1 = this;
            this.Hide();
            frmScrub.Show();
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
