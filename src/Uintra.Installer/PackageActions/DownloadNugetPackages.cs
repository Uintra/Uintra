using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Helpers;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class DownloadNugetPackages : IPackageAction
    {
        private readonly string _defaultNugetUri = "https://www.nuget.org/api/v2/package/{0}/{1}";
        private readonly string _defaultTargetDirectory = "~/umbraco/uintra/nupkgs/";

        private string NugetUri { get; set; }
        private DirectoryInfo TargetDirectory { get; set; }
        private Package[] Packages { get; set; }


        private void Initialize(XmlNode xmlData)
        {
            this.NugetUri = (xmlData.GetAttributeValue("uri") ?? _defaultNugetUri) + "{0}/{1}";
            var relativePath = xmlData.GetAttributeValue("path") ?? _defaultTargetDirectory;
            var directory = new DirectoryInfo(HostingEnvironment.MapPath(relativePath));
            if (directory.Exists)
                directory.Delete(true);
            directory.Create();
            this.TargetDirectory = directory;
            this.Packages = xmlData.ProjectValueFromChildren(
                nameof(Package), i => new Package()
                {
                    Name = i.GetAttributeValueFromNode("name"),
                    Version = i.GetAttributeValueFromNode("version")
                });
        }

        public string Alias()
        {
            return nameof(DownloadNugetPackages);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            bool flag = true;
            var attributes = new Dictionary<string, string>();
            try
            {
                this.Initialize(xmlData);
                string targetFile = TargetDirectory.FullName + "{0}.zip";
                foreach (var package in this.Packages)
                {
                    if (attributes.ContainsKey("name"))
                    {
                        attributes.Remove("name");
                        attributes.Add("name", package.Name);
                    }
                    try
                    {
                        new WebClient().DownloadFile(
                            string.Format(NugetUri, package.Name, package.Version),
                            string.Format(targetFile, package.Name));
                    }
                    catch (Exception e)
                    {
                        flag = false;
                        XmlLogger.LogFailedAction<DownloadNugetPackages>(
                            $"An error occurred during nuget package downloading. Name: [{package.Name}]/[{package.Version}] ",
                            e, attributes);
                    }
                }
            }
            catch (Exception e)
            {
                flag = false;
                XmlLogger.LogFailedAction<DownloadNugetPackages>(
                    $"An error occurred before nuget packages downloading.", e, attributes);
            }
            return flag;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode($"<Action runat=\"install\" undo=\"false\" alias=\"{this.Alias()}\" source=\"~/umbraco/uintra/bin/\" targetFramework=\"4.5\" />");
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }

    public class Package
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
