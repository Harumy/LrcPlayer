using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LrcPlayer
{
    class List
    {
        static public void List_check()
        {
            string[] Playlist_File=new string[0];
            StreamReader sr = new StreamReader("TextFile.txt");
            int i = 0;
            while (sr.Peek() != -1)
            {
                Playlist_File[0] = sr.ReadLine();
            }
            sr.Close();

        }
        public void List_shuffle(string[][] PlayList)
        {
            
        }
    }
}
