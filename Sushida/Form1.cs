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
        PictureBox TransparentRect = new PictureBox();
        StatusForm StatusWindow = new StatusForm();
        Debug status = new Debug();
        
        string AppName = "SushidaOCR";

        bool isStarted = false;//実行中か
        bool isAutoMiss = false;//あえてミスする

        private Point mousePoint;

        public Form1()
        {
            InitializeComponent();

            if (isStarted)
            {
                this.Text = AppName + " - 実行中(F1で切り替え)";
            }
            else
            {
                this.Text = AppName + " - 停止中(F1で切り替え)";
            }

            status.isMissedScan = isAutoMiss;

            StatusWindow.Show();
            StatusWindow.Location = new Point(Location.X + Size.Width + 1, Location.Y);
        }

        public void SetScr()
        {
            StatusWindow.Location = new Point(Location.X + Size.Width + 1, Location.Y);
        }

        public string GetOCR(Bitmap img)
        {
            var tesseract = new Tesseract.TesseractEngine("TrainData", "eng");
            Tesseract.Page page = tesseract.Process(img);
            
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

        private async void timer1_Tick(object sender, EventArgs e)
        {
            Task<string> task = Task.Run(() => GetOCR(GetScr()));
            await Task.WhenAll(task);
            string str = task.Result;

            status.ScanResult = str;

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
                    status.isMissedScan = false;

                    if (isAutoMiss)
                    {
                        SendKeys.Send("a"+str+"a");
                    }
                    else
                    {
                        SendKeys.Send(str);
                    }
                }
                else
                {
                    status.isMissedScan = true;
                }
            }
            catch { }

            StatusWindow.SetUI(status);

        }

        private void Ctrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                isStarted = !isStarted;
                status.isRunning = isStarted;
                if (isStarted)
                {
                    this.Text = AppName + " - 実行中(F1で切り替え)";
                    timer1.Enabled = true;
                    timer1_Tick(null,null);
                }
                else
                {
                    this.Text = AppName + " - 停止中(F1で切り替え)";
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

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            SetScr();
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            SetScr();
        }
    }
}
