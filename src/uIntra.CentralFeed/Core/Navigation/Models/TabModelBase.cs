using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public abstract class TabModelBase
    {
        public IPublishedContent Content { get; set; }
        public bool IsActive { get; set; }
    }
}