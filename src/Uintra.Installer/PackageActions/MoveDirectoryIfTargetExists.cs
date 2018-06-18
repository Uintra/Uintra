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
    public class MoveDirectoryIfTargetExists : IPackageAction
    {
        public bool Execute(string packageName, XmlNode xmlData)
        {
            var sourceDirectory = new DirectoryInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("sourceDirectory")));
            var targetDirectory = new DirectoryInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetDirectory")));
            var directoryMover = new DirectoryMoverIfTargetExist(sourceDirectory, targetDirectory);

            var attributes = new Dictionary<string, string>() {
                { "sourceDirectory", sourceDirectory.ToString() },
                { "targetDirectory", targetDirectory.ToString() }
            };

            return directoryMover.Move((e, result) =>
            {
                var errorMessage = $"An error occurred during moving directory (if target exists). Source: {sourceDirectory.FullName} || Target: {targetDirectory.FullName}";
                if (result)
                    XmlLogger.LogCompleteAction<MoveDirectoryIfTargetExists>(attributes);
                else
                {
                    LogHelper.Error<MoveDirectoryIfTargetExists>(errorMessage, e);
                    XmlLogger.LogFailedAction<MoveDirectoryIfTargetExists>(errorMessage, e, attributes);
                }
            });

        }

        public string Alias()
        {
            return nameof(MoveDirectoryIfTargetExists);
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
                LogHelper.Error<MoveDirectoryIfTargetExists>("An error occurred during moving directory (undo, if target exists).", ex);
                return false;
            }
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceDirectory=\"~/somedirectory\" targetDirectory=\"~/someotherdirectory\" />", this.Alias()));
        }
    }
}
