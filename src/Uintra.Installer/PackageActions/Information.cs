using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Uintra.Installer.Extensions;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.PackageActions
{
    public class Information : IPackageAction
    {
        public string Alias()
        {
            return nameof(Information);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var message = xmlData.GetAttributeValueFromNode("message");
            LogHelper.Info<Information>(() => string.IsNullOrWhiteSpace(message) ? "Default info message" : message);
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" message=\"Some test message\" />", Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
