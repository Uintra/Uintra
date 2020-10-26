using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core
{
    public abstract class TabModelBase
    {
        public IPublishedContent Content { get; set; }
        public bool IsActive { get; set; }
    }
}