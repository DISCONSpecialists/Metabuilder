using System.Diagnostics;

namespace MetaBuilder.Core
{
    public class Launcher
    {
        public static void LaunchExplorerForFile(string filename)
        {
            if (filename.Length > 0)
                Process.Start(strings.GetPath(filename));
        }
        public static void OpenFile(string filename)
        {
            if (filename.Length > 0)
                Process.Start(filename);
        }
    }
}