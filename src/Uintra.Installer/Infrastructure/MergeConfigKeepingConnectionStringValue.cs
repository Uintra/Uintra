using System;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Uintra.Installer.Infrastructure
{
    public abstract class MergeConfigKeepingConnectionStringValue
    {
        private XAttribute ConnectionStringAttribute { get; set; }

        private string TargetDocumentPath { get; set; }

        public void InitializeTargetDocumentPath(string path)
        {
            this.TargetDocumentPath = path;
        }

        public void ReadConnectionStringAttribute()
        {
            this.ConnectionStringAttribute = this.GetUCommerceConnectionStringAttribute(this.GetTargetDocument());
        }

        public void SetConnectionStringAttribute()
        {
            if (this.ConnectionStringAttribute == null)
                return;
            XDocument targetDocument = this.GetTargetDocument();
            XAttribute connectionStringAttribute = this.GetUCommerceConnectionStringAttribute(targetDocument);
            if (connectionStringAttribute == null)
                return;
            connectionStringAttribute.Value = this.ConnectionStringAttribute.Value;
            targetDocument.Save(HostingEnvironment.MapPath(this.TargetDocumentPath));
        }

        private XAttribute GetUCommerceConnectionStringAttribute(XDocument appConfig)
        {
            XElement xelement = appConfig.Descendants().FirstOrDefault<XElement>((Func<XElement, bool>)(x =>
            {
                if (x.Name == (XName)"runtimeConfiguration" && x.Parent != null)
                    return x.Parent.Name == (XName)"commerce";
                return false;
            }));
            if (xelement == null)
                return (XAttribute)null;
            return xelement.Attribute((XName)"connectionString") ?? (XAttribute)null;
        }

        private XDocument GetTargetDocument()
        {
            return XDocument.Load(HostingEnvironment.MapPath(this.TargetDocumentPath));
        }
    }
}
