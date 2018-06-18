using System;
using System.IO;

namespace Uintra.Installer.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static void CopyTo(this DirectoryInfo targetDir, DirectoryInfo destinationDir, bool overwrite)
        {
            PerformAction(targetDir, destinationDir, overwrite, false);
        }

        public static void MoveTo(this DirectoryInfo targetDir, DirectoryInfo destinationDir, bool overwrite)
        {
            PerformAction(targetDir, destinationDir, overwrite, true);
        }

        private static void PerformAction(DirectoryInfo targetDir, DirectoryInfo destinationDir, bool overwrite, bool deleteTarget)
        {
            var files = targetDir.EnumerateFiles();
            if (!destinationDir.Exists) destinationDir.Create();
            foreach (var file in files)
            {
                try
                {
                    file.CopyTo(destinationDir.FullName + "\\" + file.Name, overwrite);
                }
                catch (Exception) { }
                if (deleteTarget)
                    file.Delete();
            }
            var dirs = targetDir.EnumerateDirectories();
            foreach (var dir in dirs)
            {
                PerformAction(dir, new DirectoryInfo(destinationDir.FullName + "\\" + dir.Name), overwrite, deleteTarget);
            }

            if (deleteTarget)
                try
                {
                    targetDir.Delete(true);
                }
                catch (Exception) { }
        }
    }
}
