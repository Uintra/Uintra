using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using Uintra.Installer.Extensions;
using Uintra.Installer.Infrastructure;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class CreateXmlLogFile : IPackageAction
    {
        private XDocument Document { get; set; }

        public string Alias()
        {
            return nameof(CreateXmlLogFile);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var virtualPath = "~/App_Data/TEMP/Uintra/InstallPackageLog.xml";
            var path = HostingEnvironment.MapPath(virtualPath);
            var packageVersion = xmlData.GetAttributeValueFromNode("version");

            CreateFile(path);
            LogMachineInfo();
            LogInstallationInfo(packageVersion);
            Document?.Save(path);
            return true;
        }

        private void CreateFile(string path)
        {
            try
            {
                var fileInfo = new FileInfo(path);
                if (!fileInfo.Directory.Exists)
                    fileInfo.Directory.Create();
                var document = XDocument.Parse(Template);
                document.Save(fileInfo.FullName);
                Document = document;
            }
            catch (Exception e)
            {
                LogHelper.Error<CreateXmlLogFile>("Could not create xml log file", e);
            }
        }

        private void LogMachineInfo()
        {
            var info = new MachineInfo();
            var machineElem = Document.Root.Element("machineInfo");
            machineElem.SetElementValue("cpuId", info.CpuId);
            machineElem.SetElementValue("hddSerial", info.HddSerial);
            machineElem.SetElementValue("ip", info.Ip);
            machineElem.SetElementValue("macAddress", info.MacAddress);
            machineElem.SetElementValue("mbSerial", info.MbSerial);
        }

        private void LogInstallationInfo(string version)
        {
            var installationElem = Document.Root.Element("installationInfo");
            installationElem.SetElementValue("startTime", DateTime.UtcNow);
            installationElem.SetElementValue("status", true);
            installationElem.SetElementValue("umbracoVersion", ConfigurationManager.AppSettings["umbracoConfigurationStatus"]);
            installationElem.SetElementValue("packageVersion", version);
        }

        private string Template
        {
            get
            {
                return @"<log><installationInfo><startTime></startTime><endTime></endTime><status></status></installationInfo><machineInfo></machineInfo><actions></actions></log>";
            }
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" version=\"0.3.0\" />", Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            throw new NotImplementedException();
        }
    }
}
