using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LrcPlayer
{
    public partial class Main : Form
    {
        private int PlayingFlag = 0;
        public string[][] PlayList;

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern int mciSendString(String command,StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        private string aliasName = "MediaFile";

        public Main()
        {
            InitializeComponent();
            List.List_check();
        }

        private void PlayAndStop_Click(object sender, EventArgs e)
        {
            if (PlayingFlag == 0)
            {
                string FileName = @"M:\楽曲\ボカロ\キミノヨゾラ哨戒班.mp3";
                string cmd;
                cmd = "open \"" + FileName + "\" type mpegvideo alias " + aliasName;
                Console.WriteLine(cmd);
                if (mciSendString(cmd, null, 0, IntPtr.Zero) != 0)
                {
                    Console.WriteLine("なんか変");
                    return;
                }
                cmd = "Play " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                PlayingFlag = 1;
                Console.WriteLine(PlayingFlag);
            }
            else
            {
                string cmd;
                cmd = "stop " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                cmd = "close " + aliasName;
                mciSendString(cmd, null, 0, IntPtr.Zero);
                PlayingFlag = 0;
                Console.WriteLine(PlayingFlag);
            }
        }
    }
}
