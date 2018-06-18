using System.Linq;
using Uintra.CentralFeed.Providers;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Uintra.CentralFeed
{
    public class CentralFeedHelper : ICentralFeedHelper
    {
        private readonly ICentralFeedContentProvider _contentProvider;

        public CentralFeedHelper(ICentralFeedContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public bool IsCentralFeedPage(IPublishedContent page)
        {
            return IsHomePage(page) || IsSubPage(page);
        }
        private bool IsHomePage(IPublishedContent page) =>
            _contentProvider.GetOverviewPage().Id == page.Id;

        private bool IsSubPage(IPublishedContent page) => 
            _contentProvider.GetRelatedPages().Any(c => c.IsAncestorOrSelf(page));
    }
}