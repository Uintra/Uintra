using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class Clear : IPackageAction
    {
        private readonly string _defaultInstallerDirectory = "~/umbraco/uintra/";
        private readonly string _defaultInstallerFileName = "Uintra.Installer.dll";

        private DirectoryInfo InstallerDirectory { get; set; }
        private FileInfo Installer { get; set; }
        private FileInfo ViewsWebConfigTransformFile { get; set; }

        private void Initialize(XmlNode xmlData)
        {
            var virtualPath = xmlData.GetAttributeValue("installerDirectory") ?? _defaultInstallerDirectory;
            this.InstallerDirectory = new DirectoryInfo(HostingEnvironment.MapPath(virtualPath));
            var installerFileName = xmlData.GetAttributeValue("installerFileName") ?? _defaultInstallerFileName;
            this.Installer = new FileInfo(HostingEnvironment.MapPath($"~/bin/{installerFileName}"));
            this.ViewsWebConfigTransformFile = new FileInfo(HostingEnvironment.MapPath($"~/Views/Web.config.transform"));
        }

        public string Alias()
        {
            return nameof(Clear);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            try
            {
                Initialize(xmlData);
                if (Installer.Exists)
                    Installer.Delete();
                if (InstallerDirectory.Exists)
                    InstallerDirectory.Delete(true);
                if (ViewsWebConfigTransformFile.Exists)
                    ViewsWebConfigTransformFile.Delete();
            }
            catch (Exception e) { LogHelper.Error<Clear>("Could not delete some files during Clear action", e); }
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode($"<Action runat=\"install\" undo=\"false\" alias=\"{Alias()}\" installerDirectory=\"~umbraco/uintra\" installerName=\"Uintra.Installer.dll\" />");
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
