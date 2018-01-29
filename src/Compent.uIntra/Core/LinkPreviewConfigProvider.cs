using uIntra.Core;
using uIntra.Core.Utils;

namespace Compent.uIntra.Core
{
    public class LinkPreviewConfigProvider : ILinkPreviewConfigProvider
    {
        public FooBananaConfig Config
        {
            get
            {
                return FileSystemUtils.ReadXmlFile<FooBananaConfig>("~/config/LinkDetection.config");
            }
        }
    }
}