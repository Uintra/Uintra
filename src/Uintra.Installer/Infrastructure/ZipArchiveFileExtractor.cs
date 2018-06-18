//using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using Umbraco.Core.Logging;

namespace Uintra.Installer.Infrastructure
{
    public class ZipArchiveFileExtractor
    {
        private readonly string _archivePath;

        public ZipArchiveFileExtractor(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
                throw new ArgumentException(string.Format("'path' must point to valid file. '{0}'.", (object)path));
            this._archivePath = path;
        }

        public void Extract(string targetDirectory)
        {
            ZipFile.ExtractToDirectory(this._archivePath, targetDirectory);
            //FileStream fs = File.OpenRead(this._archivePath);
            //ZipFile file = new ZipFile(fs);
            //foreach (ZipEntry entry in file)
            //{
            //    if (entry.IsDirectory)
            //    {
            //        String entryFileName = entry.Name;
            //    }
            //}
        }

        public void Extract(string sourceFilename, string targetFilename, string targetFramework)
        {
            if (string.IsNullOrEmpty(sourceFilename))
                throw new ArgumentNullException(nameof(sourceFilename));
            if (string.IsNullOrEmpty(targetFilename))
                throw new ArgumentNullException(nameof(targetFilename));
            new FileInfo(targetFilename).Directory.Create();
            using (Package package = Package.Open(this._archivePath, FileMode.Open, FileAccess.Read))
            {
                foreach (PackagePart part in package.GetParts())
                {
                    var fullPath = part.Uri.ToString();
                    //if (sourceFilename.Equals("Castle.Core.dll") && fullPath.EndsWith(sourceFilename))
                    //{
                    //    LogHelper.Info<ZipArchiveFileExtractor>(() => $"----------------{part.ContentType}----------------{part.Uri.ToString()}----------");
                    //    LogHelper.Info<ZipArchiveFileExtractor>(() => $"--{targetFramework + "/" + sourceFilename}---");
                    //    //"/lib/net35/Castle.Core.dll"
                    //}
                    var loweredFullPath = fullPath.ToLowerInvariant();
                    var loweredSourceFileName = sourceFilename.ToLowerInvariant();
                    if ((string.IsNullOrWhiteSpace(targetFramework) && loweredFullPath.EndsWith(loweredSourceFileName)) ||
                        (!string.IsNullOrWhiteSpace(targetFramework) && loweredFullPath.EndsWith(targetFramework + "/" + loweredSourceFileName)))
                        {
                            this.WritePartToFile(targetFilename, part);
                            break;
                        }
                }
            }
        }

        private void WritePartToFile(string targetFilename, PackagePart part)
        {
            using (Stream stream1 = part.GetStream(FileMode.Open, FileAccess.Read))
            {
                using (Stream stream2 = (Stream)File.OpenWrite(targetFilename))
                {
                    byte[] buffer = new byte[4096];
                    int count;
                    while ((count = stream1.Read(buffer, 0, buffer.Length)) > 0)
                        stream2.Write(buffer, 0, count);
                    stream2.Close();
                }
            }
        }
    }
}
