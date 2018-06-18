using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Helpers;
//using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class DownloadFileWeb : IPackageAction
    {
        private string SourceWebResource { get; set; }

        private string TargetFilename { get; set; }

        private void Initialize(XmlNode xmlData)
        {
            this.SourceWebResource = xmlData.GetAttributeValueFromNode("webResource");
            this.TargetFilename = xmlData.GetAttributeValueFromNode("targetFile");
            this.TargetFilename = HostingEnvironment.MapPath(this.TargetFilename);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var attributes = new Dictionary<string, string>() { { "webResource", this.SourceWebResource } };
            try
            {
                this.Initialize(xmlData);
                if (System.IO.File.Exists(this.TargetFilename))
                {
                    File.Delete(this.TargetFilename);
                    //return true;
                }
                var file = new FileInfo(this.TargetFilename);
                if (!file.Directory.Exists)
                    file.Directory.Create();
                new WebClient().DownloadFile(this.SourceWebResource, this.TargetFilename);
                XmlLogger.LogCompleteAction<DownloadFileWeb>(attributes);
                return true;
            }
            catch (Exception e)
            {
                var errorText = $"An error occurred during web file downloading. Url: {SourceWebResource} || Target file: {TargetFilename}";
                LogHelper.Error<DownloadFileWeb>(errorText, e);
                XmlLogger.LogFailedAction<DownloadFileWeb>(errorText, e, attributes);
                return false;
            }
        }

        public string Alias()
        {
            return "DownloadFile";
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            //throw new NotImplementedException();
            return false;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" webResource=\"https://www.uintra.dk/file.zip\" targetFile=\"~/umbraco/uintra/bin/file.zip\"/>", this.Alias()));
        }
    }
}
