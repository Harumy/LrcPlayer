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
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("winmm.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int mciSendString(String command,StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        private string aliesName = "Media File";

        public Form1()
        {
            InitializeComponent();
        }
    }
}
