using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Uintra.Installer.Extensions;
using Uintra.Installer.Infrastructure;
using Umbraco.Core.Logging;
using System.Collections.Generic;
using Uintra.Installer.Helpers;

namespace Uintra.Installer.PackageActions
{
    public class MoveDirectory : IPackageAction
    {
        public string Alias()
        {
            return nameof(MoveDirectory);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var sourceDirectory = new DirectoryInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("sourceDirectory")));
            var targetDirectory = new DirectoryInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetDirectory")));
            var directoryMover = new DirectoryMover(sourceDirectory, targetDirectory, true);

            var attributes = new Dictionary<string, string>() {
                { "sourceDirectory", sourceDirectory.ToString() },
                { "targetDirectory", targetDirectory.ToString() }
            };

            return directoryMover.Move((e, result) =>
            {
                var errorMessage = $"An error occurred during moving directory. Source: {sourceDirectory.FullName} || Target: {targetDirectory.FullName}";
                if (result)
                    XmlLogger.LogCompleteAction<MoveDirectory>(attributes);
                else
                {
                    LogHelper.Error<MoveDirectory>(errorMessage, e);
                    XmlLogger.LogFailedAction<MoveDirectory>(errorMessage, e, attributes);
                }
            });
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceDirectory=\"~/somedirectory\" targetDirectory=\"~/someotherdirectory\" />", this.Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            string attributeValueFromNode = xmlData.GetAttributeValueFromNode("targetDirectory");
            try
            {
                Directory.Delete(attributeValueFromNode);
            }
            catch (Exception ex)
            {
                LogHelper.Error<MoveDirectory>("An error occurred during moving directory (undo).", ex);
                return false;
            }
            return true;
        }
    }
}
