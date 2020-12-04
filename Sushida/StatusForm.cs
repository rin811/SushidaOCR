using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sushida
{
    public partial class StatusForm : Form
    {
        public StatusForm()
        {
            InitializeComponent();
            this.TopMost = true;

            label1.Text = "スキャン結果\n誤認識";
        }

        private void StatusForm_Load(object sender, EventArgs e)
        {

        }

        public void SetUI(Debug debug)
        {
            label1.Text = $"スキャン結果: {debug.ScanResult}\n誤認識: {debug.isMissedScan}";
        }
    }
}
