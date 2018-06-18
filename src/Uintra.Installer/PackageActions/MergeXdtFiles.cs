using Microsoft.Web.XmlTransform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class MergeXdtFiles : IPackageAction
    {
        private readonly string _defaulInstallerTempDirectory = "~/umbraco/uintra/";
        private FileInfo TargetConfig { get; set; }
        private IEnumerable<FileInfo> XdtFiles { get; set; }
        private DirectoryInfo InstallerTempDirectory { get; set; }

        private void Initialize(XmlNode XmlData)
        {
            var pattern = XmlData.GetAttributeValue("pattern");
            var relativePath = XmlData.GetAttributeValueFromNode("targetConfig");
            this.TargetConfig = new FileInfo(HostingEnvironment.MapPath(relativePath));
            this.InstallerTempDirectory = new DirectoryInfo(HostingEnvironment.MapPath(_defaulInstallerTempDirectory));
            this.XdtFiles = InstallerTempDirectory.EnumerateFiles(pattern ?? $"{TargetConfig.Name}.install.xdt", SearchOption.AllDirectories);
        }

        public string Alias()
        {
            return nameof(MergeXdtFiles);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            this.Initialize(xmlData);
            var attributes = new Dictionary<string, string>() {
                { "targetConfig", TargetConfig.FullName }
            };

            XmlTransformableDocument transformableDocument = new XmlTransformableDocument();
            transformableDocument.PreserveWhitespace = true;
            transformableDocument.Load(TargetConfig.FullName);
            bool flag = true;
            foreach (var xdtFile in XdtFiles)
            {
                if (attributes.ContainsKey("xdtFile"))
                {
                    attributes.Remove("xdtFile");
                    attributes.Add("xdtFile", xdtFile.FullName);
                }
                try
                {
                    using (XmlTransformation xmlTransformation = new XmlTransformation(xdtFile.FullName))
                    {
                        xmlTransformation.Apply(transformableDocument);
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    var errorText = $"An error occurred during xdt files merge. Target config: {TargetConfig.FullName} || XdtFile: {xdtFile.FullName} || Transformations count: {XdtFiles.Count()}";
                    LogHelper.Error<MergeXdtFiles>(errorText, e);
                    XmlLogger.LogFailedAction<MergeXdtFiles>(errorText, e, attributes);
                }
            }
            transformableDocument.Save(TargetConfig.FullName);
            return flag;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode($"<Action runat=\"install\" undo=\"false\" alias=\"{Alias()}\" targetConfig=\"~/config/Lang/da-DK.user.xml\" />");
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
