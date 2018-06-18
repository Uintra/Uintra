using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Infrastructure;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;
using System.Collections.Generic;
using Uintra.Installer.Helpers;

namespace Uintra.Installer.PackageActions
{
    class ExtractArchiveToDirectory : IPackageAction
    {
        public string Alias()
        {
            return nameof(ExtractArchiveToDirectory);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var sourceArchiveName = HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("sourceArchive"));
            var targetDirectoryName = HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetDirectory"));
            var sourceArchive = new FileInfo(sourceArchiveName);
            var targetDirectory = new DirectoryInfo(targetDirectoryName);
            if (!File.Exists(sourceArchive.FullName))
                throw new ArgumentException($"Not a valid archive. Archive full path: [{sourceArchive.FullName}].");
            var attributes = new Dictionary<string, string>() {
                { "sourceArchive", sourceArchiveName },
                { "targetDirectory", targetDirectoryName }
            };
            try
            {
                if (targetDirectory.Exists)
                    Directory.Delete(targetDirectory.FullName, true);
                new ZipArchiveFileExtractor(sourceArchive.FullName)
                    .Extract(targetDirectory.FullName);
                XmlLogger.LogCompleteAction<ExtractArchiveToDirectory>(attributes);
                return true;
            }
            catch (Exception e)
            {
                var errorText = $"An error occurred during extraction downloaded file to directory. Source archive: {sourceArchive.FullName} || Target directory: {targetDirectory.FullName}";
                LogHelper.Error<ExtractArchiveToDirectory>(errorText, e);
                XmlLogger.LogFailedAction<ExtractArchiveToDirectory>(errorText, e, attributes);
                return false;
            }
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceArchive=\"~/umbraco/uintra/bin/file.zip\" targetDirectory=\"~/bin/uintra/someFolder\"/>", this.Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
