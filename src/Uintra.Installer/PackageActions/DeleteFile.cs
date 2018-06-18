using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Helpers;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class DeleteFile : IPackageAction
    {
        public bool Execute(string packageName, XmlNode xmlData)
        {
            string path = HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("file"));
            var attributes = new Dictionary<string, string>() {
                { "file", path }
            };
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                XmlLogger.LogCompleteAction<DeleteFile>(attributes);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occurred during file deleting. File name is : {path}";
                LogHelper.Error<DeleteFile>(errorMessage, e);
                XmlLogger.LogFailedAction<DeleteFile>(errorMessage, e, attributes);
                return false;
            }
            return true;
        }

        public string Alias()
        {
            return nameof(DeleteFile);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            throw new NotImplementedException();
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" file=\"~/somefile.txt\" />", this.Alias()));
        }
    }
}
