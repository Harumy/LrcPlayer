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
            string[] Playlist_File = new string[100];
            StreamReader sr = new StreamReader("M:\\List.txt");
            int i = 0;
            while (sr.Peek() != -1)
            {
                Playlist_File[i] = sr.ReadLine();
                Console.WriteLine(Playlist_File[i]);
                i += 1;
            }
            sr.Close();

        }
        public void List_shuffle(string[][] PlayList)
        {
            
        }
    }
}
