using Microsoft.Web.XmlTransform;
using System.Web;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace Uintra.Installer.PackageActions
{
    public class TransformConfig : IPackageAction
    {
        public string Alias()
        {
            return nameof(TransformConfig);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            string absolute1 = VirtualPathUtility.ToAbsolute(xmlData.Attributes.GetNamedItem("file").Value);
            string absolute2 = VirtualPathUtility.ToAbsolute(xmlData.Attributes.GetNamedItem("xdtfile").Value);
            using (XmlTransformableDocument transformableDocument = new XmlTransformableDocument())
            {
                transformableDocument.PreserveWhitespace = true;
                transformableDocument.Load(HttpContext.Current.Server.MapPath(absolute1));
                using (XmlTransformation xmlTransformation = new XmlTransformation(HttpContext.Current.Server.MapPath(absolute2)))
                {
                    if (xmlTransformation.Apply((XmlDocument)transformableDocument))
                        transformableDocument.Save(HttpContext.Current.Server.MapPath(absolute1));
                }
            }
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" file=\"~/web.config\" xdtfile=\"~/app_plugins/demo/web.config.xdt\"></Action>", (object)this.Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }
    }
}
