using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Uintra.Installer.Extensions;
using Uintra.Installer.Infrastructure;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace Uintra.Installer.PackageActions
{
    public class MergeXmlFiles : IPackageAction
    {
        public string Alias()
        {
            return nameof(MergeXmlFiles);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            var targetFilePath = xmlData.GetAttributeValueFromNode("tagetFile");
            var sourceFilePath = xmlData.GetAttributeValueFromNode("transformFile");

            var merger = new XmlFileMerger(sourceFilePath, targetFilePath);
            merger.Install();

            return true;

        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" tagetFile=\"~/web.config\" transformFile=\"~/app_plugins/demo/web.config.transform\"></Action>", (object)this.Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
