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

            //label1.Text = "状態\nスキャン結果\n誤認識";
            SetUI(new Debug());
        }

        private void StatusForm_Load(object sender, EventArgs e)
        {

        }

        public void SetUI(Debug debug)
        {
            string StateConv;
            if (debug.isRunning)
                StateConv = "実行しています";
            else
                StateConv = "停止しています";
            label1.Font = new Font("メイリオ", 9);
            label1.Text = $"状態: {StateConv}\n" +
                $"スキャン結果: {debug.ScanResult}\n" +
                $"誤認識: {debug.isMissedScan}\n" +
                $"インターバル: {debug.Interval}ミリ秒\n" +
                $"高精度の学習ファイルを使用する: {debug.isUseHighTrainData}\n" +
                $"入力時にミスをする: {debug.isDummyMissing}";
        }
    }
}
