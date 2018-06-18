using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Uintra.Installer.Infrastructure
{
    public class XmlFileMerger
    {
        private bool fileMoved;
        private string targetFilePath;
        private string sourceFilePath;
        private XDocument targetFile;
        private XDocument sourceFile;

        private string TargetFilePath
        {
            get
            {
                return this.targetFilePath;
            }
            set
            {
                this.targetFilePath = value;
                this.targetFile = (XDocument)null;
                if (File.Exists(this.targetFilePath))
                    return;
                if (this.CreateIfTargetFileNotExists)
                {
                    if (!this.EmbeddedResource)
                    {
                        File.Move(HostingEnvironment.MapPath(this.SourceFilePath), this.targetFilePath);
                    }
                    else
                    {
                        using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.sourceFilePath))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(this.targetFilePath));
                            using (FileStream fileStream = new FileStream(this.targetFilePath, FileMode.OpenOrCreate))
                                manifestResourceStream.CopyTo((Stream)fileStream);
                        }
                    }
                    this.fileMoved = true;
                }
                else
                    this.targetFilePath = (string)null;
            }
        }

        private string SourceFilePath
        {
            get
            {
                return this.sourceFilePath;
            }
            set
            {
                this.sourceFilePath = value;
                this.sourceFile = (XDocument)null;
            }
        }

        public bool DeleteTargetFileOnUndo { get; set; }

        public bool CreateIfTargetFileNotExists { get; set; }

        public bool OverwriteValues { get; set; }

        public bool EmbeddedResource { get; set; }

        protected XDocument TargetFile
        {
            get
            {
                if (this.targetFile == null && !string.IsNullOrEmpty(this.targetFilePath))
                    this.targetFile = XDocument.Load(this.targetFilePath);
                return this.targetFile;
            }
        }

        protected XDocument SourceFile
        {
            get
            {
                if (this.sourceFile == null)
                    this.sourceFile = !this.EmbeddedResource ? XDocument.Load(HostingEnvironment.MapPath(this.SourceFilePath)) : XDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(this.SourceFilePath));
                return this.sourceFile;
            }
        }

        public XmlFileMerger(string relativeTransformFilePath, string relativeWebConfigFilePath)
        {
            this.CreateIfTargetFileNotExists = true;

            this.EmbeddedResource = false;
            this.DeleteTargetFileOnUndo = false;
            this.OverwriteValues = false;
            this.SetFiles(relativeTransformFilePath, relativeWebConfigFilePath);
        }

        protected void SetFiles(string sourceFile, string targetFile)
        {
            this.SourceFilePath = sourceFile;
            this.TargetFilePath = HostingEnvironment.MapPath(targetFile);
        }

        private void MergeElement(XElement sourceElement, XElement targetElement)
        {
            foreach (XAttribute attribute in sourceElement.Attributes())
            {
                XAttribute sourceAttribute = attribute;
                if (targetElement.Attributes().FirstOrDefault<XAttribute>((Func<XAttribute, bool>)(i => i.Name.Equals((object)sourceAttribute.Name))) == null || this.OverwriteValues)
                    targetElement.SetAttributeValue(sourceAttribute.Name, (object)sourceAttribute.Value);
            }
            foreach (XElement element in sourceElement.Elements())
            {
                XElement targetElement1 = this.CheckElementExists(element, targetElement);
                if (!targetElement1.HasElements && this.OverwriteValues && !string.IsNullOrEmpty(targetElement1.Value))
                    targetElement1.Value = element.Value;
                this.MergeElement(element, targetElement1);
            }
        }

        private XElement CheckElementExists(XElement sourceElement, XElement parentElement)
        {
            XElement xelement = this.FindElement(parentElement, sourceElement);
            if (xelement == null)
            {
                xelement = new XElement(sourceElement.Name);
                if (!sourceElement.HasElements)
                    xelement.Value = sourceElement.Value;
                parentElement.Add((object)xelement);
            }
            return xelement;
        }

        protected virtual XElement FindElement(XElement parentElement, XElement sourceElement)
        {
            return parentElement.Elements().FirstOrDefault<XElement>((Func<XElement, bool>)(i =>
            {
                if (!i.Name.Equals((object)sourceElement.Name))
                    return false;
                if (i.Attribute((XName)"alias") == null)
                    return true;
                return i.Attribute((XName)"alias").Value.Equals(sourceElement.Attribute((XName)"alias").Value);
            }));
        }

        protected virtual void CleanFile()
        {
        }

        public void Install()
        {
            if (this.TargetFile == null)
                return;
            XElement targetElement = this.TargetFile.Root;
            if (targetElement == null)
            {
                targetElement = new XElement(this.SourceFile.Root.Name);
                this.TargetFile.Add((object)targetElement);
            }
            if (this.fileMoved)
                return;
            this.MergeElement(this.SourceFile.Root, targetElement);
            this.TargetFile.Save(this.TargetFilePath, SaveOptions.None);
            if (this.EmbeddedResource)
                return;
            File.Delete(HostingEnvironment.MapPath(this.SourceFilePath));
        }

        public void Uninstall()
        {
            if (this.TargetFile == null)
                return;
            if (!this.DeleteTargetFileOnUndo)
            {
                this.CleanFile();
                this.TargetFile.Save(this.TargetFilePath, SaveOptions.None);
            }
            else
                File.Delete(this.TargetFilePath);
        }
    }
}
