using Umbraco.Core.Models;

namespace Uintra.CentralFeed
{
    public abstract class TabModelBase
    {
        public IPublishedContent Content { get; set; }
        public bool IsActive { get; set; }
    }
}