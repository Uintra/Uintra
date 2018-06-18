using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Infrastructure;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class CopyFile : IPackageAction
    {
        public string Alias()
        {
            return nameof(CopyFile);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var sourceFile = new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("sourceFile")));
            var targetFile = new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetFile")));
            var fileCopier = new FileCopier(sourceFile, targetFile);
            return fileCopier.Copy(ex => LogHelper.Error<CopyFile>("An error occurred during file copying.", ex));
            //return new FileCopier(new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("sourceFile"))), new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetFile")))).Copy((Action<Exception>)(ex => Log.Add((LogTypes)18, -1, ex.FormatForLogging())));
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceFile=\"~/somefile.txt\" targetFile=\"~/someotherfile.txt\" />", (object)this.Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            throw new NotImplementedException();
        }
    }
}
