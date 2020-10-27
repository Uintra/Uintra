using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Core
{
    public abstract class TabModelBase
    {
        public IPublishedContent Content { get; set; }
        public bool IsActive { get; set; }
    }
}