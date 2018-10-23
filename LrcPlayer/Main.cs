using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;

namespace LrcPlayer
{
    public partial class Main : Form
    {
        private int PlayingFlag = 0;
        int PastTime = 0;
        public string[][] PlayList;
        string cmd;
        int Time;
        public Timer Timer = new Timer();

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command,StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        private string aliasName = "MediaFile";
        
        public Main()
        {
            InitializeComponent();
            InitLoad();
            Start();
        }
        public void Start()
        {
            Timer.Interval = 1000;
            Timer.Tick += (TimerSender, TimerE) =>
            {
                PastTime += 1;
                Elapse(PastTime);
            };
        }
        private void InitLoad()
        {
            StringBuilder PlayTime = new StringBuilder();
            string FileName = @"M:\楽曲\ボカロ\キミノヨゾラ哨戒班.mp3";
            cmd = "open \"" + FileName + "\" type mpegvideo alias " + aliasName;
            Console.WriteLine(cmd);
            if (mciSendString(cmd, null, 0, IntPtr.Zero) != 0)
            {
                Console.WriteLine("なんか変");
                return;
            }
            cmd = "Status " + aliasName + " length";
            mciSendString(cmd, PlayTime, 256, IntPtr.Zero);
            Time = int.Parse(PlayTime.ToString());
            Time = Time - Time % 1000;
            Time = Time / 1000;
            int Sec = Time % 60;
            int Min = (Time - Sec) / 60;
            Console.WriteLine(Min + ":" + Sec);
            Console.WriteLine(Time);
        }
        private void Play()
        {
            cmd = "Play " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
            PlayingFlag = 1;
            Console.WriteLine("Flag:" + PlayingFlag);
            PlayAndStop.Text = "停止";
            Timer.Start();
        }
        private void Stop()
        {
            string cmd;
            cmd = "stop " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
            cmd = "seek " + aliasName + " to start";
            mciSendString(cmd, null, 0, IntPtr.Zero);
            PlayingFlag = 0;
            Console.WriteLine(PlayingFlag);
            PlayAndStop.Text = "再生";
            Timer.Stop();
            Timer.Dispose();
            PastTime = 0;
        }
        private void PlayAndStop_Click(object sender, EventArgs e)
        {
            if (PlayingFlag == 0)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
        private void Elapse(int PastTime)
        {
            if (Time < PastTime)
            {
                Stop();
                Play();
            }
            else
            {
                Gauge(Time, PastTime);
            }
        }
        private void Gauge(int PlayTime,int PastTime)
        {
            Console.WriteLine(PastTime + "/" + PlayTime);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = PlayTime;
            progressBar1.Value = PastTime;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            if (PlayingFlag == 1)
            {
                Console.WriteLine("一時停止するよー");
                cmd = "pause " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                Pause.Text = "再開";
                PlayingFlag = 2;
                Timer.Stop();
            }
            else if(PlayingFlag == 2)
            {
                Console.WriteLine("再開するよー");
                cmd = "resume " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                Pause.Text = "一時停止";
                PlayingFlag = 1;
                Timer.Start();
            }
        }
    }
}
