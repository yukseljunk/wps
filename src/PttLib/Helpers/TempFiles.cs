using System;
using System.IO;

namespace PttLib.Helpers
{
    public static class TempFiles
    {
        private static string _tempFilePath = Path.GetFullPath(Path.Combine(Helper.AssemblyDirectory, @"..\..\temp\file.tmp"));

        public static string TempFilesDirectory
        {
            get
            {
                return Path.GetDirectoryName(_tempFilePath);
            }
        }

        public static void CreateTempFolder()
        {
            if (Directory.Exists(TempFilesDirectory)) return;
            Directory.CreateDirectory(TempFilesDirectory);
        }

        public static string TempFilePath(string extension)
        {
            CreateTempFolder();
            return Path.GetFullPath(_tempFilePath.Replace("file.tmp", "tmp_" + Guid.NewGuid() + "."+extension));

        }
    }
}