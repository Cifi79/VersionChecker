using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace VersionChecker
{
    static class Function
    {
        public static string GetVersion(string pathproj, string FileName, string SearchString)
        {
            string Content = "";
            try
            {
                Content = File.ReadAllLines(Path.Combine(pathproj, FileName)).First(p => (p.Contains(SearchString) && !p.Contains("//")));
                Content = Content.Split('"')[1];
            }
            catch { }
            return Content;
        }

        public static List<string> IterateDirectories(string root, string filter)
        {
            List<string> count = new List<string>();
            iterateDirectories(root, ref count, filter);
            return count;
        }

        private static void iterateDirectories(string root, ref List<string> count, string filter)
        {
            try
            {
                foreach (string directory in Directory.EnumerateDirectories(root))
                    iterateDirectories(directory, ref count, filter);

                count.AddRange(Directory.GetFiles(root, filter));
            }
            catch {; }
        }

    }
}
