using System;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using Uintra.Installer.Infrastructure;
using Uintra.Installer.Extensions;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using Umbraco.Core.Logging;
using System.Collections.Generic;
using Uintra.Installer.Helpers;

namespace Uintra.Installer.PackageActions
{
    public class MergeConfig : IPackageAction
    {
        private FileInfo _targetConfig;
        private string _configurationVirtualPath;
        public Transformation[] _transformations;

        public void Initialize(XmlNode xmlData)
        {
            this._configurationVirtualPath = xmlData.GetAttributeValueFromNode("targetConfig");
            this._targetConfig = new FileInfo(HostingEnvironment.MapPath(xmlData.GetAttributeValueFromNode("targetConfig")));
            this._transformations = xmlData.ProjectValueFromChildren<Transformation>("Transformation", (Func<XmlNode, Transformation>)(n => new Transformation(n.GetAttributeValueFromNode("virtualPath"), n.GetAttributeFlagFromNode("integrated"))));
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            this.Initialize(xmlData);
            var attributes = new Dictionary<string, string>() {
                { "targetConfig", _targetConfig.ToString() }
            };

            //TODO add transformation merge error to xml log file
            try
            {
                using (ConfigurationTransformer configurationTransformer = new ConfigurationTransformer(this._targetConfig))
                {
                    foreach (Transformation transformation in this._transformations)
                        configurationTransformer.Transform(new FileInfo(HostingEnvironment.MapPath(transformation.VirtualPath)), transformation.OnlyIfIisIntegrated,
                            (ex => LogHelper.Error<Transformation>($"An error occurred during transformation merge. Transformation virtual path: [{transformation.VirtualPath}]", ex)));
                }
                XmlLogger.LogCompleteAction<MergeConfig>(attributes);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occurred during file merge. Target config: {_targetConfig.FullName} || Transformations count: {_transformations.Length}";
                LogHelper.Error<MergeConfig>(errorMessage, e);
                XmlLogger.LogFailedAction<MergeConfig>(errorMessage, e, attributes);
                return false;
            }
            return true;
        }

        public string Alias()
        {
            return nameof(MergeConfig);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" targetConfig=\"~/web.config\" sourceConfig=\"~/umbraco/uintra/install/uintra.config\" sourceConfigIntegratedMode=\"~/umbraco/uintra/install/uintra.IIS7.config\"/>", this.Alias()));
        }
    }
}
