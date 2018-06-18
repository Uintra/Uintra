using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using Uintra.Installer.Extensions;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class SendXmlLogFile : IPackageAction
    {
        public string Alias()
        {
            return nameof(SendXmlLogFile);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var file = new FileInfo(HostingEnvironment.MapPath("~/App_Data/TEMP/Uintra/InstallPackageLog.xml"));
            if (!file.Exists)
                return true;
            var uri = xmlData.GetAttributeValueFromNode("uri");
            using (var client = new HttpClient())
            {
                try
                {
                    var document = XDocument.Load(file.FullName);
                    document.Root.Element("installationInfo").SetElementValue("endTime", DateTime.UtcNow);

                    var content = new StringContent(document.ToString());
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/xml");

                    var task = client.PutAsync(uri, content);
                    task.Wait();
                }
                catch (Exception e)
                {
                    LogHelper.Error<SendXmlLogFile>("Sending logs failed!", e);
                }
            }
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" uri=\"http://someapi.com/report\" />", Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            throw new NotImplementedException();
        }
    }
}
