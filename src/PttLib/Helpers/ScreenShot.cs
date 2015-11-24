using System;
using System.IO;

namespace PttLib.Helpers
{
    public static class ScreenShot
    {
        private static string _ssFilePath = Path.GetFullPath(Path.Combine(Helper.AssemblyDirectory, @"..\..\screenshots\screenshot.png"));

        public static string ScreenShotDirectory
        {
            get
            {
                return Path.GetDirectoryName(_ssFilePath);
            }
        }

        public static void CreateScreenShotFolder()
        {
            if (Directory.Exists(ScreenShotDirectory)) return;
            Directory.CreateDirectory(ScreenShotDirectory);
        }

        public static string ScreenShotFilePath
        {
            get
            {
                CreateScreenShotFolder();
                return Path.GetFullPath(_ssFilePath.Replace("screenshot.png", "ss_" + Guid.NewGuid() + ".png"));
            }
        }
    }
}