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
    public class ApplyTransformations : IPackageAction
    {
        private readonly string _defaultInstallerDirectory = "~/umbraco/uintra/";

        private Config[] ConfigurationFiles { get; set; }
        private DirectoryInfo InstallerDirectory { get; set; }

        private void Initialize(XmlNode xmlData)
        {
            this.InstallerDirectory = new DirectoryInfo(HostingEnvironment.MapPath(_defaultInstallerDirectory));
            this.ConfigurationFiles = xmlData.ProjectValueFromChildren(
                nameof(Config), i =>
                {
                    var config = new Config()
                    {
                        File = new FileInfo(HostingEnvironment.MapPath(i.GetAttributeValueFromNode("path"))),
                        SearchPattern = i.GetAttributeValue("pattern")
                    };
                    config.XdtFiles = InstallerDirectory.EnumerateFiles(
                        config.SearchPattern ?? $"{config.File.Name}.install.xdt", SearchOption.AllDirectories);
                    return config;
                });
        }

        public string Alias()
        {
            return nameof(ApplyTransformations);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            bool flag = true;
            var attributes = new Dictionary<string, string>();
            try
            {
                this.Initialize(xmlData);
            }
            catch (Exception e)
            {
                XmlLogger.LogFailedAction<ApplyTransformations>(
                    "An error occurred during action initialization.", e, attributes);
                return false;
            }
            foreach (var config in ConfigurationFiles)
            {
                try
                {
                    if (attributes.ContainsKey("targetConfig"))
                    {
                        attributes.Remove("targetConfig");
                        attributes.Add("targetConfig", config.File.FullName);
                    }
                    using (XmlTransformableDocument transformableDocument = new XmlTransformableDocument())
                    {
                        transformableDocument.PreserveWhitespace = true;
                        transformableDocument.Load(config.File.FullName);
                        foreach (var xdtFile in config.XdtFiles)
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
                                XmlLogger.LogFailedAction<ApplyTransformations>(
                                    $"An error occurred during xdt files merge. Target config: {config.File.FullName} || XdtFile: {xdtFile.FullName} || Transformations count: {config.XdtFiles.Count()}"
                                    , e, attributes);
                            }
                        }
                        transformableDocument.Save(config.File.FullName);
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    XmlLogger.LogFailedAction<ApplyTransformations>(
                        $"An error occurred before xdt files merge for target config: {config.File.FullName}",
                        e, attributes);
                }

            }
            return flag;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode($"<Action runat=\"install\" undo=\"false\" alias=\"{Alias()}\"></Action>");
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }

        private class Config
        {
            public FileInfo File { get; set; }
            public string SearchPattern { get; set; }
            public IEnumerable<FileInfo> XdtFiles { get; set; }
        }
    }
}
