using System;
using System.IO;
using System.Web.Hosting;

namespace Uintra.Installer.Infrastructure
{
    public class XmlMerger
    {
        private readonly FileInfo _toBeTransformed;
        private readonly Transformation[] _transformations;

        public XmlMerger(string configurationVirtualPath, params Transformation[] transformations)
        {
            this._toBeTransformed = new FileInfo(HostingEnvironment.MapPath(configurationVirtualPath));
            this._transformations = transformations;
        }

        public void DoMerge()
        {
            using (ConfigurationTransformer configurationTransformer = new ConfigurationTransformer(this._toBeTransformed))
            {
                foreach (Transformation transformation in this._transformations)
                    configurationTransformer.Transform(new FileInfo(HostingEnvironment.MapPath(transformation.VirtualPath)), transformation.OnlyIfIisIntegrated, (Action<Exception>)(ex => { }));
            }
        }
    }
}
