using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Helpers;
using Uintra.Installer.Infrastructure;
//using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class ExtractFileFromArchive : IPackageAction
    {
        private string SourceArchive { get; set; }

        private string SourceFilename { get; set; }

        private string TargetFilename { get; set; }

        private string TargetFramework { get; set; }

        private void Initialize(XmlNode xmlData)
        {
            this.SourceArchive = xmlData.GetAttributeValueFromNode("sourceArchive");
            this.SourceFilename = xmlData.GetAttributeValueFromNode("sourceFile");
            this.TargetFilename = xmlData.GetAttributeValueFromNode("targetFile");
            this.TargetFramework = xmlData.GetAttributeValueFromNode("targetFramework");
            this.SourceArchive = HostingEnvironment.MapPath(this.SourceArchive);
            this.TargetFilename = HostingEnvironment.MapPath(this.TargetFilename);
            if (!File.Exists(this.SourceArchive))
                throw new ArgumentException($"Not a valid archive. Archive full path: [{SourceArchive}].");
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var attributes = new Dictionary<string, string>() {
                { "sourceArchive", this.SourceArchive },
                { "sourceFile", this.SourceFilename },
                { "targetFile", this.TargetFilename }
            };
            try
            {
                this.Initialize(xmlData);
                if (File.Exists(this.TargetFilename))
                    File.Delete(this.TargetFilename);
                new ZipArchiveFileExtractor(this.SourceArchive)
                    .Extract(this.SourceFilename, this.TargetFilename, this.TargetFramework);
                XmlLogger.LogCompleteAction<ExtractFileFromArchive>(attributes);
                return true;
            }
            catch (Exception e)
            {
                var errorText = $"An error occurred during extraction downloaded file. Source archive: {SourceArchive} || Source filename: {SourceFilename} || Target file: {TargetFilename}";
                LogHelper.Error<ExtractFileFromArchive>(errorText, e);
                XmlLogger.LogFailedAction<ExtractFileFromArchive>(errorText, e, attributes);
                return false;
            }
        }

        public string Alias()
        {
            return nameof(ExtractFileFromArchive);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            //throw new NotImplementedException();
            return false;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceArchive=\"~/umbraco/uintra/bin/file.zip\" sourceFile=\"file.dll\" targetFile=\"~/bin/uintra/file.dll\"/>", (object)this.Alias()));
        }
    }
}