using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Configuration;
using System.IO.Compression;
using NET.Tools;

namespace Effect.FX.WPF.IO
{
    public static class History
    {
        private const int MAXHISTORY = 12;
        public static IList<FileInfo> FileList { get; private set; }

        static History()
        {
            FileList = new List<FileInfo>();
            Reload();
        }

        public static void Reload()
        {
            if (!File.Exists(Files.History))
                return;

            try
            {
                using (FileStream fin = new FileStream(Files.History, FileMode.Open))
                {
                    using (GZipStream zip = new GZipStream(fin, CompressionMode.Decompress))
                    {
                        int count = zip.ReadInteger();

                        for (int i = 0; i < count; i++)
                            FileList.Add(new FileInfo(zip.ReadString()));
                    }
                }
            }
            catch (Exception e)
            {
                FileList.Clear();
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public static void Save()
        {
            try
            {
                using (FileStream fin = new FileStream(Files.History, FileMode.Create))
                {
                    using (GZipStream zip = new GZipStream(fin, CompressionMode.Compress))
                    {
                        zip.WriteInteger(FileList.Count);

                        foreach (FileInfo fi in FileList)
                            zip.WriteString(fi.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public static bool Add(FileInfo file)
        {
            if (!History.FileList.Contains(fi => { return fi.FullName.Equals(file.FullName); }))
            {
                History.FileList.Insert(0, file);
                if (History.FileList.Count > MAXHISTORY)
                    History.FileList.RemoveAt(History.FileList.Count - 1);
                History.Save();

                return true;
            }

            return false;
        }
    }
}
