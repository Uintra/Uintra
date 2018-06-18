using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public class DownloadNugetPackage : IPackageAction
    {
        private string _nugetUri = "https://www.nuget.org/api/v2/package/{0}/{1}";
        private string _targetFile = "~/umbraco/uintra/bin/{0}.zip";

        public string Alias()
        {
            return nameof(DownloadNugetPackage);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var name =  xmlData.GetAttributeValueFromNode("name");
            var version = xmlData.GetAttributeValueFromNode("version");
            var attributes = new Dictionary<string, string>() { { "name", name } };
            var file = new FileInfo(HostingEnvironment.MapPath(string.Format(_targetFile, name)));

            try
            {
                if (file.Exists)
                { file.Delete(); }
                if (!file.Directory.Exists)
                { file.Directory.Create(); }

                new WebClient().DownloadFile(string.Format(_nugetUri, name, version), file.FullName);
                XmlLogger.LogCompleteAction<DownloadNugetPackage>(attributes);
                return true;
            }
            catch (Exception e)
            {
                var errorText = $"An error occurred during nuget package downloading. Name: [{name}]/[{version}] || Target file: {file.FullName}";
                LogHelper.Error<DownloadNugetPackage>(errorText, e);
                XmlLogger.LogFailedAction<DownloadNugetPackage>(errorText, e, attributes);
                return false;    
            }
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode($"<Action runat=\"install\" undo=\"false\" alias=\"{Alias()}\" name=\"Compent.Uintra\" version=\"0.3.0\" />");
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
