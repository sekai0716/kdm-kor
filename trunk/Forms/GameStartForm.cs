using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KingsDamageMeter.Forms
{
    public partial class GameStartForm : Form
    {
        Timer Clock;
        int cnt = 9;

        public GameStartForm()
        {
            InitializeComponent();

            Clock = new Timer();
            Clock.Interval = 1000;
            Clock.Tick += new EventHandler(Timer_Tick);
            Clock.Start();
        }

        public void Timer_Tick(object sender, EventArgs eArgs)
        {
            if (sender == Clock)
            {
                string temp = string.Format("이 창은 {0}초 뒤에 자동으로 종료됩니다.", cnt);
                label2.Text = temp;
                if (cnt-- == 0)
                {
                    Clock.Stop();
                    Clock.Dispose();
                    this.Dispose();
                }
            }
        }


        private void GameStartForm_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://aion.plaync.co.kr");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.InvokeScript("gameStart");
        }
    }
}
