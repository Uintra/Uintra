using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Enums;
using uCommunity.CentralFeed.Models;
using uCommunity.Core;
using uCommunity.Core.Activity;
using uCommunity.Core.Extentions;
using uCommunity.Core.Grid;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uCommunity.Core.CentralFeed
{
    public class CentralFeedContentHelper: ICentralFeedContentHelper
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IGridHelper _gridHelper;

        public CentralFeedContentHelper(
            UmbracoHelper umbracoHelper,
            ICentralFeedService centralFeedService,
            IGridHelper gridHelper)
        {
            _umbracoHelper = umbracoHelper;
            _centralFeedService = centralFeedService;
            _gridHelper = gridHelper;
        }

        public IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(HomePage.ModelTypeAlias));
        }

        public bool IsCentralFeedPage(IPublishedContent currentPage)
        {
            return GetOverviewPage().Id == currentPage.Id || GetContents().Any(c => c.IsAncestorOrSelf(currentPage));
        }

        public IEnumerable<CentralFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
            var overviewPage = GetOverviewPage();
            yield return new CentralFeedTabModel
            {
                Content = overviewPage,
                Type = GetTabType(overviewPage),
                IsActive = overviewPage.Id == currentPage.Id
            };

            foreach (var content in GetContents())
            {
                var tabType = GetTabType(content);
                var activityType = tabType.GetHashCode().ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null)
                {
                    continue;
                }

                var settings = _centralFeedService.GetSettings(activityType.Value);
                yield return new CentralFeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    HasSubscribersFilter = settings.HasSubscribersFitler,
                    CreateUrl = content.Children.SingleOrDefault(n => n.DocumentTypeAlias.In(NewsCreatePage.ModelTypeAlias, EventsCreatePage.ModelTypeAlias))?.Url,
                    IsActive = content.IsAncestorOrSelf(currentPage)
                };
            }
        }

        public CentralFeedTypeEnum GetTabType(IPublishedContent content)
        {
            var value = _gridHelper.GetValue(content, "custom.CentralFeed");

            if (value == null || value.tabType == null)
            {
                return default(CentralFeedTypeEnum);
            }

            int tabType;
            if (int.TryParse(value.tabType.ToString(), out tabType))
            {
                return (CentralFeedTypeEnum)tabType;
            }
            return default(CentralFeedTypeEnum);
        }

        private IEnumerable<IPublishedContent> GetContents()
        {
            return GetOverviewPage().Children.Where(c => c.DocumentTypeAlias.In(NewsOverviewPage.ModelTypeAlias, EventsOverviewPage.ModelTypeAlias));
        }
    }
    public static class ObjectExtensions
    {
        public static bool In<T>(this T value, params T[] items)
        {
            if (items == null || (object)value == null)
                return false;
            return ((IEnumerable<T>)items).Contains<T>(value);
        }
    }
}