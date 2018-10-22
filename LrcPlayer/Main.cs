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

        public string[][] PlayList;

        [System.Runtime.InteropServices.DllImport("winmm.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int MciSendString(String command,StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        private string aliesName = "Media File";

        public Main()
        {
            InitializeComponent();
            List.List_check();
        }
    }
}
