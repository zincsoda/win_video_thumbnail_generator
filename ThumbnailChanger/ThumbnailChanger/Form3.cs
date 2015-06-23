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
    public partial class Form3 : Form
    {
        public Form Form1 { get; set; }

        public Form3(String url)
        {
            InitializeComponent();
            axWindowsMediaPlayer1.URL = url;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Form1.Show();
            this.Close();
            
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState.ToString() != "wmppsPaused")
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }

            btnPrevious.Enabled = true;

            if (axWindowsMediaPlayer1.Ctlcontrols.currentPosition - 0.2 > 0.0)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = axWindowsMediaPlayer1.Ctlcontrols.currentPosition - 0.2;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
            else
            {
                btnPrevious.Enabled = false;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState.ToString() != "wmppsPaused")
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }

            btnPrevious.Enabled = true;

            if (axWindowsMediaPlayer1.Ctlcontrols.currentPosition + 0.2 < axWindowsMediaPlayer1.currentMedia.duration)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = axWindowsMediaPlayer1.Ctlcontrols.currentPosition + 0.2;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.pause();

            }
            else
            {
                btnNext.Enabled = false;
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 3)
            {
                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                btnCaptureThumbnail.Enabled = true;
                trkFrames.Enabled = true;
                trkFrames.Visible = true;
                trkFrames.Minimum = 0;
                trkFrames.Maximum = (int)Math.Round(axWindowsMediaPlayer1.currentMedia.duration / 0.2);
                trkFrames.LargeChange = (int)Math.Round(axWindowsMediaPlayer1.currentMedia.duration / 0.2) / 10;
                trkFrames.SmallChange = 1;
                //trkFrames.TickFrequency = (int)Math.Round(axWindowsMediaPlayer1.currentMedia.duration / 0.2);
            }
        }

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
                                    axWindowsMediaPlayer1.Height - 60)
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

        private void Form3_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private DateTime lastScroll = DateTime.Now;
        private void trkFrames_Scroll(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState.ToString() != "wmppsPaused")
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }

            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0.2 * trkFrames.Value;

            if (DateTime.Now > lastScroll.AddSeconds(0.5))
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                lastScroll = DateTime.Now;
                lblTimeDispaly.Text = "Current Time Select: " + 0.2 * trkFrames.Value;
            }
        }
    }
}
