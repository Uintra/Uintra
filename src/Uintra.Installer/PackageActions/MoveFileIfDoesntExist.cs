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
    public class MoveFileIfDoesntExist : IPackageAction
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
                var errorMessage = "An error occurred during moving file (if file does not exists).";
                if (result)
                    XmlLogger.LogCompleteAction<MoveFileIfDoesntExist>(attributes);
                else
                {
                    LogHelper.Error<MoveFileIfDoesntExist>(errorMessage, e);
                    XmlLogger.LogFailedAction<MoveFileIfDoesntExist>(errorMessage, e, attributes);
                }
            });
        }

        public string Alias()
        {
            return nameof(MoveFileIfDoesntExist);
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
