using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Helpers;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class ExtractNugetPackages : IPackageAction
    {
        private readonly string _defaultSourceDirectory = "~/umbraco/uintra/nupkgs/";
        private readonly string _defaultTempDirectory = "~/umbraco/uintra/temp/";
        private readonly string _defaultTargetFramework = "net45";
        private readonly string _defaultUintraFileName = "Compent.Uintra.dll";
        private readonly string _defaultInstallerDirectory = "~/umbraco/uintra/";

        private DirectoryInfo SourceDirectory { get; set; }
        private string TargetFramework { get; set; }
        private DirectoryInfo TempDirectory { get; set; }
        private DirectoryInfo ApplicationRootDirectory { get; set; }
        private FileInfo UintraFile { get; set; }
        private DirectoryInfo InstallerDirectory { get; set; }

        private void Initialize(XmlNode xmlData)
        {
            this.TargetFramework = xmlData.GetAttributeValue("targetFramework") ?? _defaultTargetFramework;
            var relativePath = xmlData.GetAttributeValue("sourceDirectory") ?? _defaultSourceDirectory;
            var directory = new DirectoryInfo(HostingEnvironment.MapPath(relativePath));
            if (!directory.Exists) directory.Create();
            this.SourceDirectory = directory;
            this.TempDirectory = new DirectoryInfo(HostingEnvironment.MapPath(_defaultTempDirectory));
            if (TempDirectory.Exists) TempDirectory.Delete(true);
            this.ApplicationRootDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/"));
            this.InstallerDirectory = new DirectoryInfo(HostingEnvironment.MapPath(
                xmlData.GetAttributeValue("installerDirectory") ?? _defaultInstallerDirectory));
            var uintraFileName = xmlData.GetAttributeValue("uintraFileName") ?? _defaultUintraFileName;
            this.UintraFile = this.InstallerDirectory.EnumerateFiles(uintraFileName).FirstOrDefault();
        }

        public string Alias()
        {
            return nameof(ExtractNugetPackages);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            bool flag = true;
            var attributes = new Dictionary<string, string>();
            try
            {
                this.Initialize(xmlData);
                var zippedNugetFiles = this.SourceDirectory.GetFiles("*.zip");
                var binDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/bin/"));
                var appPluginDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Plugins/"));
                foreach (var file in zippedNugetFiles)
                {
                    var extractedPackageTempDirectory = new DirectoryInfo(TempDirectory.FullName + file.Name);
                    try
                    {
                        if (attributes.ContainsKey("name"))
                        {
                            attributes.Remove("name");
                            attributes.Add("name", file.FullName);
                        }
                        ZipFile.ExtractToDirectory(file.FullName, extractedPackageTempDirectory.FullName);

                        if (file.Name.ToLowerInvariant().Contains("uintra.zip"))
                        {
                            var contentDirectory = new DirectoryInfo(extractedPackageTempDirectory.FullName + "/content");
                            var files = contentDirectory.EnumerateFiles().Where(i =>
                                i.Extension.ToLowerInvariant().ContainsAny(new[] { "ico", "png", "html" }));
                            foreach (var item in files)
                            {
                                var destinationFile = new FileInfo(ApplicationRootDirectory.FullName + item.Name);
                                if (destinationFile.Exists) destinationFile.Delete();
                                item.MoveTo(destinationFile.FullName);
                            }
                            var directories = contentDirectory.EnumerateDirectories().Where(i =>
                                i.Name.ToLowerInvariant().ContainsAny(new[] { "build", "views", "content", "app_plugins" }));
                            foreach (var item in directories)
                            {
                                var destinationDirectory = new DirectoryInfo(ApplicationRootDirectory.FullName + item.Name);
                                item.MoveTo(destinationDirectory, true);
                            }
                            continue;
                        }

                        //var dlls = extractedPackageTempDirectory.EnumerateFiles("*.dll", SearchOption.AllDirectories).ToList();
                        //var targetSpecificDlls = dlls.Where(i => i.FullName.ToLowerInvariant().Contains(TargetFramework)).ToList();
                        //foreach (var dll in targetSpecificDlls)
                        //{
                        //    var destinationFile = new FileInfo(binDirectory.FullName + dll.Name);
                        //    if (destinationFile.Exists) destinationFile.Delete();
                        //    dll.MoveTo(destinationFile.FullName);
                        //}
                        //if (!targetSpecificDlls.Any())
                        //{
                        //    var dll = dlls.FirstOrDefault(); //will be used the lowest target framework version (for example net30 or net40)
                        //    if (dll != null)
                        //    {
                        //        var destinationFile = new FileInfo(binDirectory.FullName + dll.Name);
                        //        if (destinationFile.Exists) destinationFile.Delete();
                        //        dll.MoveTo(destinationFile.FullName);
                        //    }
                        //}
                        var libDirectory = extractedPackageTempDirectory.EnumerateDirectories("lib").FirstOrDefault();
                        if (libDirectory != null)
                        {
                            var topLevelDlls = libDirectory.EnumerateFiles("*.dll", SearchOption.TopDirectoryOnly);
                            if (topLevelDlls.Any())
                            {
                                foreach (var dll in topLevelDlls)
                                {
                                    var destinationFile = new FileInfo(binDirectory.FullName + dll.Name);
                                    if (destinationFile.Exists) destinationFile.Delete();
                                    dll.MoveTo(destinationFile.FullName);
                                }
                            }
                            else
                            {
                                var pattern = new Regex(@"^net(\d{2,3})$", RegexOptions.IgnoreCase);
                                var targetFrameworkVersion = int.Parse(TargetFramework.Count() < 3 ? TargetFramework + "0" : TargetFramework);
                                var frameworkDirectories = extractedPackageTempDirectory.EnumerateDirectories("net*", SearchOption.AllDirectories);
                                var targetFrameworkDirectory = frameworkDirectories.Select(i =>
                                    {
                                        var match = pattern.Match(i.Name);
                                        if (!match.Success)
                                            return null;
                                        var value = match.Groups[1].Value.Replace(".", "");
                                        var version = int.Parse(value.Count() < 3 ? value + "0" : value);
                                        return new { Directory = i, Version = version };
                                    }).WhereNotNull().OrderByDescending(i => i.Version)
                                    .FirstOrDefault(i => i.Version <= targetFrameworkVersion);
                                foreach (var dll in targetFrameworkDirectory?.Directory.EnumerateFiles("*.dll"))
                                {
                                    var destinationFile = new FileInfo(binDirectory.FullName + dll.Name);
                                    if (destinationFile.Exists) destinationFile.Delete();
                                    dll.MoveTo(destinationFile.FullName);
                                }
                            }
                        }

                        var appFolders = extractedPackageTempDirectory.EnumerateDirectories("App_Plugins", SearchOption.AllDirectories);
                        foreach (var directory in appFolders)
                        {
                            var pluginsDirectories = directory.EnumerateDirectories(); //EmailWorker.Web contains two app_plugins foleder (include archetype)
                            foreach (var pluginDir in pluginsDirectories)
                            {
                                var destinationDirectory = new DirectoryInfo(appPluginDirectory.FullName + pluginDir.Name);
                                if (destinationDirectory.Exists) destinationDirectory.Delete(true);
                                pluginDir.MoveTo(destinationDirectory, true);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        flag = false;
                        XmlLogger.LogFailedAction<ExtractNugetPackages>(
                            $"An error occurred during extraction/moving downloaded nuget files. Source archive: {file.FullName} ", e, attributes);
                    }
                }
                this.UintraFile.MoveTo(binDirectory.FullName + this.UintraFile.Name);
            }
            catch (Exception e)
            {
                flag = false;
                XmlLogger.LogFailedAction<ExtractNugetPackages>(
                    $"An error occurred before extraction/moving downloaded nuget files or during moving [{UintraFile?.FullName}]",
                    e, attributes);
            }
            return flag;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode($"<Action runat=\"install\" undo=\"false\" alias=\"{Alias()}\" sourceDirectory=\"~/umbraco/uintra/nupkgs/\" targetFramework=\"net45\" installerDirectory=\"~/umbraco/intra/\" uintraFileName=\"Compent.Uintra.dll\" />");
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
