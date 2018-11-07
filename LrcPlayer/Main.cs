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
using System.IO;

namespace LrcPlayer
{
    public partial class Main : Form
    {
        private int PlayingFlag = 0;
        int PlayingTrack = 0;
        int PastTime = 0;
        static public string[] PlayList;
        string cmd;
        int Time;
        public Timer Timer = new Timer();
        string TrackLength;
        int[] LrcTime = new int[0];
        string[] LrcStr = new string[0];
        int Disp_Lrc=0;
        bool LrcFlag = false;

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command,StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        private string aliasName = "MediaFile";
        
        public Main()
        {
            InitializeComponent();
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
            List.List_check();
        }
        private void InitLoad()
        {
            for (int i = 0; i < 4; i += 1) {
                int tmp = PlayingTrack + i;
                if (PlayList.Length <= tmp)
                {
                    tmp = tmp - PlayList.Length;
                }
                Control[] cs = Controls.Find("Music" + (i+1) + "_Title", true);
                if (cs.Length > 0)
                {
                    ((Label)cs[0]).Text = Path.GetFileNameWithoutExtension(PlayList[tmp]);
                }
                Console.WriteLine(tmp.ToString()+":"+Path.GetFileNameWithoutExtension(PlayList[tmp]));
            }

            StringBuilder PlayTime = new StringBuilder();
            string FileName = PlayList[PlayingTrack];
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
            Time = Time / 1000;
            String aaa = StrTime(Time);
            TrackLength = aaa;
            string LrcName = PlayList[PlayingTrack].Replace(".mp3", ".lrc");
            if (System.IO.File.Exists(LrcName))
            {
                LrcFlag = true;
                StreamReader sr = new StreamReader(LrcName);
                while (sr.Peek() != -1)
                {
                    string reading = sr.ReadLine();
                    string[] read = reading.Split(']');
                    Array.Resize(ref LrcStr, LrcStr.Length + 1);
                    LrcStr[LrcStr.Length - 1] = read[1];
                    read[0].Replace("[","");
                    string[] readTime_Tmp = read[0].Split(':');
                    int readTime = new int();
                    readTime = int.Parse(readTime_Tmp[0])*60*1000;
                    if (readTime_Tmp[1].IndexOf(".") != -1)
                    {
                        
                    }
                    else
                    {
                        readTime = readTime + int.Parse(readTime_Tmp[1]);
                    }
                    readTime = readTime / 1000;
                }
            }
        }
        private void Play()
        {
            InitLoad();
            Console.WriteLine(PlayingTrack.ToString());
            cmd = "Play " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
            PlayingFlag = 1;
            Console.WriteLine("Flag:" + PlayingFlag);
            PlayAndStop.Text = "停止";
            Timer.Start();
            Track.Text = (PlayingTrack+1).ToString() + "/" + PlayList.Length.ToString();
        }
        private void Stop()
        {
            LrcDesp1.Text = "";
            LrcDesp2.Text = "";
            LrcDesp3.Text = "";
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
            cmd = "close " + aliasName;
            mciSendString(cmd, null, 0, IntPtr.Zero);
        }
        private void Next()
        {
            PlayingTrack += 1;
            if (PlayList.Length <= PlayingTrack)
            {
                PlayingTrack = 0;
            }
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
            if (Time <= PastTime)
            {
                Stop();
                Play();
            }
            else
            {
                Gauge(Time, PastTime);
                if (LrcFlag)
                {
                    if (PastTime < LrcTime[Disp_Lrc])
                    {
                        LrcDesp1.Text = LrcStr[Disp_Lrc];
                        LrcDesp2.Text = LrcStr[Disp_Lrc + 1];
                        LrcDesp3.Text = LrcStr[Disp_Lrc + 2];
                    }
                    else
                    {
                        Disp_Lrc++;
                        LrcDesp1.Text = LrcStr[Disp_Lrc];
                        LrcDesp2.Text = LrcStr[Disp_Lrc + 1];
                        LrcDesp3.Text = LrcStr[Disp_Lrc + 2];

                    }
                }
                else
                {
                    LrcDesp1.Text = "歌詞ファイルがありません。";
                }
            }
        }
        private void Gauge(int PlayTime,int PastTime)
        {
            Console.WriteLine(PastTime + "/" + PlayTime);
            string tmp = StrTime(PastTime);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = PlayTime;
            progressBar1.Value = PastTime;
            Length.Text = tmp + "/" + TrackLength;
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

        private void TrackNext_Click(object sender, EventArgs e)
        {
            Stop();
            Next();
            Play();
        }
        public string StrTime(int I)
        {
            int Sec = I % 60;
            int Min = (I - Sec) / 60;
            string SecTmp;
            if (Sec < 10)
            {
                SecTmp = "0" + Sec;
            }
            else
            {
                SecTmp = Sec.ToString();
            }
            string S = Min + ":" + SecTmp;
            return S;
        }

        private void ReadList_Click(object sender, EventArgs e)
        {
            List.List_check();
        }
    }
}
