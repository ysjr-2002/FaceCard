using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DYW.ImageReview.Core
{
    public static class FileManager
    {
        private static string root = "photo";

        static FileManager()
        {
            root = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, root);
            if (Directory.Exists(root) == false)
                Directory.CreateDirectory(root);
        }

        public static string SaveFile(int cardNo, string imagebase64)
        {
            if (!imagebase64.IsEmpty())
            {
                var folder = GetFolder();
                var filename = string.Concat(cardNo, "-", DateTime.Now.ToString("HHmmss"), ".jpg");
                var filepath = Path.Combine(folder, filename);
                File.WriteAllBytes(filepath, imagebase64.Base64ToByte());
                return filepath;
            }
            else
                return string.Empty;
        }

        public static string ReadFile(string filepath)
        {
            if (File.Exists(filepath))
                return File.ReadAllText(filepath);
            else
                return string.Empty;
        }

        public static string GetFolder()
        {
            var day = DateTime.Now.ToString("yyyyMMdd");
            var folder = Path.Combine(root, day);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }
    }
}
