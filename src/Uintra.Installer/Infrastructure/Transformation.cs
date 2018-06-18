using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uintra.Installer.Infrastructure
{
    public class Transformation
    {
        public Transformation(string virtualPath, bool isIntegrated = false)
        {
            this.VirtualPath = virtualPath;
            this.OnlyIfIisIntegrated = isIntegrated;
        }

        public string VirtualPath { get; private set; }

        public bool OnlyIfIisIntegrated { get; set; }
    }
}
