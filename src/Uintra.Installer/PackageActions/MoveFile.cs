using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Helpers;
using Uintra.Installer.Infrastructure;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class MoveFile : IPackageAction
    {
        public bool Execute(string packageName, XmlNode xmlData)
        {
            var sourceFile = new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("sourceFile")));
            var targetFile = new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetFile")));
            var fileMover = new FileMover(sourceFile, targetFile);

            var attributes = new Dictionary<string, string>() {
                { "sourceFile", sourceFile.ToString() },
                { "targetFile", targetFile.ToString() }
            };

            return fileMover.MoveIfDoesntExist((e, result) =>
            {
                var errorMessage = $"An error occurred during moving file. Source: {sourceFile.FullName} || Target: {targetFile.FullName}";
                if (result)
                    XmlLogger.LogCompleteAction<MoveFile>(attributes);
                else
                {
                    LogHelper.Error<MoveFile>(errorMessage, e);
                    XmlLogger.LogFailedAction<MoveFile>(errorMessage, e, attributes);
                }
            });
        }

        public string Alias()
        {
            return nameof(MoveFile);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            File.Delete(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetFile")));
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceFile=\"~/somefile.txt\" targetFile=\"~/someotherfile.txt\" />", this.Alias()));
        }
    }
}
