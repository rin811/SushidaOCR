using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Ocr;
using Windows.Graphics.Imaging;
using Tesseract;

namespace Sushida
{
    public partial class Form1 : Form
    {
        bool isStarted = false;//実行中か
        bool isAutoMiss = false;//あえてミスする

        private Point mousePoint;

        public Form1()
        {
            InitializeComponent();

            if (isStarted)
            {
                this.Text = this.Text + " - F1で停止";
            }
            else
            {
                this.Text = this.Text + " - F1で開始";
            }
        }

        public string GetOCR(Bitmap img)
        {
            var tesseract = new Tesseract.TesseractEngine("TrainData", "eng");
            Tesseract.Page page = tesseract.Process(img);

            Text = page.GetText();
            return page.GetText();
        }

        public Bitmap GetScr()
        {
            
            Bitmap ScrBmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(ScrBmp);
            g.CopyFromScreen(0, 0, 0, 0, ScrBmp.Size);
            g.Dispose();

            Bitmap WinShot = ScrBmp.Clone(new Rectangle(PointToScreen(new Point(0,0)).X, PointToScreen(new Point(0, 0)).Y,
                ClientSize.Width, ClientSize.Height),ScrBmp.PixelFormat);

            ScrBmp.Dispose();

            //WinShot.Save("SS.png", System.Drawing.Imaging.ImageFormat.Png);
            return WinShot;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = GetOCR(GetScr());

            if (isStarted)
            {
                this.Text = this.Text + " - F1で停止";
            }
            else
            {
                this.Text = this.Text + " - F1で開始";
            }

            //大文字だったら誤認識判定
            bool Err = false;
            foreach(char c in str)
            {
                if (char.IsUpper(c))
                    Err = true;
            }

            try
            {
                if (Err == false)
                {
                    if (isAutoMiss)
                    {
                        SendKeys.Send("a"+str+"a");
                    }
                    else
                    {
                        SendKeys.Send(str);
                    }
                }

            }
            catch { }


        }

        private void Ctrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                isStarted = !isStarted;

                if (isStarted)
                {
                    timer1.Enabled = true;
                    timer1_Tick(null,null);
                }
                else
                {
                    timer1.Enabled = false;
                }


            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);
            }
        }
    }
}
