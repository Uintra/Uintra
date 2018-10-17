using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Web.XmlTransform;
using System;

namespace Uintra.Tasks
{
    public class RemoveInvalidAssemblyBindings : Task
    {
        private string xdtFilePath;
        private string webConfigPath;

        [Required]
        public string XdtFilePath
        {
            get
            {
                return xdtFilePath;
            }
            set
            {
                xdtFilePath = value;
            }
        }

        [Required]
        public string WebConfigPath
        {
            get
            {
                return webConfigPath;
            }
            set
            {
                webConfigPath = value;
            }
        }

        public override bool Execute()
        {
            try
            {
                XmlTransformableDocument transformableDocument = new XmlTransformableDocument();
                transformableDocument.PreserveWhitespace = true;
                transformableDocument.Load(WebConfigPath);

                using (XmlTransformation xmlTransformation = new XmlTransformation(XdtFilePath))
                    xmlTransformation.Apply(transformableDocument);

                transformableDocument.Save(WebConfigPath);
                transformableDocument.Dispose();
            }
            catch (Exception)
            {
                return false;
            }
            //Log.LogMessage(MessageImportance.High, "Removed invalid assembly dependency bindings! " + this.XdtFilePath + "\r\n" + this.WebConfigPath);
            return true;
        }
    }
}
