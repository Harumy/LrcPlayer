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
            string[] Playlist_File = new string[0];
            StreamReader sr = new StreamReader("M:\\List.txt");
            int i = 0;
            while (sr.Peek() != -1)
            {
                int a = Playlist_File.Length + 1;
                Array.Resize(ref Playlist_File, a);
                Playlist_File[i] = sr.ReadLine();
                Console.WriteLine(Playlist_File[i]);
                i += 1;
            }
            sr.Close();
            List_shuffle(Playlist_File);
            Console.WriteLine("lengthOfPlayList:" + Playlist_File.Length);
            Main.PlayList = Playlist_File;
        }
        static public string[] List_shuffle(string[] PlayList)
        {
            string[] List = PlayList.OrderBy(i => Guid.NewGuid()).ToArray();

            System.Random rng = new System.Random();
            int n = PlayList.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string tmp = PlayList[k];
                PlayList[k] = PlayList[n];
                PlayList[n] = tmp;
            }
            List = PlayList;

            return List;
        }
    }
}
