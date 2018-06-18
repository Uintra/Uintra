using Microsoft.Web.XmlTransform;
using System;
using System.IO;
using System.Xml;

namespace Uintra.Installer.Infrastructure
{
    public class ConfigurationTransformer : IDisposable
    {
        private readonly TextWriter _transformed;
        private readonly XmlTransformableDocument _target;
        private readonly bool _iisIntegratedConfig;
        private readonly string _targetPath;

        private ConfigurationTransformer()
        {
            XmlTransformableDocument transformableDocument = new XmlTransformableDocument();
            transformableDocument.PreserveWhitespace = true;
            this._target = transformableDocument;
        }

        public ConfigurationTransformer(FileInfo toBeTransformed)
          : this()
        {
            this._targetPath = toBeTransformed.FullName;
            this._target.Load(this._targetPath);
            this._iisIntegratedConfig = ConfigurationTransformer.IisIntegratedConfig(this._target);
        }

        public ConfigurationTransformer(TextReader toBeTransformed, TextWriter transformed)
          : this()
        {
            this._transformed = transformed;
            this._target.Load(toBeTransformed);
            this._iisIntegratedConfig = ConfigurationTransformer.IisIntegratedConfig(this._target);
        }

        public bool Transform(FileInfo transformation, bool onlyIfIisIntegrated, Action<Exception> logAction)
        {
            try
            {
                if (onlyIfIisIntegrated)
                {
                    if (!this._iisIntegratedConfig)
                        goto exit;
                }
                using (FileStream fileStream = transformation.OpenRead())
                    this.DoTransform((Stream)fileStream);
            }
            catch (Exception ex)
            {
                logAction(ex);
                return false;
            }
            exit:
            return true;
        }

        public void Transform(Stream transformation)
        {
            this.DoTransform(transformation);
        }

        private void DoTransform(Stream transform)
        {
            using (XmlTransformation xmlTransformation = new XmlTransformation(transform, (IXmlTransformationLogger)null))
                xmlTransformation.Apply((XmlDocument)this._target);
        }

        private static bool IisIntegratedConfig(XmlTransformableDocument doc)
        {
            bool flag = false;
            if (doc != null)
                flag = doc.SelectSingleNode("/configuration/system.webServer") != null;
            return flag;
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(this._targetPath) && this._transformed == null)
                this._target.Save(this._targetPath);
            else
                this._target.Save(this._transformed);
            this._target.Dispose();
        }
    }
}
