using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Grid;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.CentralFeed
{
    public class CentralFeedContentHelper : ICentralFeedContentHelper
    {
        private const string CentralFeedFiltersStateCookieName = "centralFeedFiltersState";
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IGridHelper _gridHelper;
        private readonly ICookieProvider _cookieProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly ICentralFeedLinkService _centralFeedLinkService;

        public CentralFeedContentHelper(
            UmbracoHelper umbracoHelper,
            ICentralFeedService centralFeedService,
            IGridHelper gridHelper,
            ICookieProvider cookieProvider,
            IActivityTypeProvider activityTypeProvider,
            IFeedTypeProvider feedTypeProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            ICentralFeedLinkService centralFeedLinkService)
        {
            _umbracoHelper = umbracoHelper;
            _centralFeedService = centralFeedService;
            _gridHelper = gridHelper;
            _cookieProvider = cookieProvider;
            _activityTypeProvider = activityTypeProvider;
            _feedTypeProvider = feedTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _centralFeedLinkService = centralFeedLinkService;
        }

        public IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage()));
        }

        public bool IsCentralFeedPage(IPublishedContent currentPage)
        {
            return GetOverviewPage().Id == currentPage.Id || GetContents().Any(c => c.IsAncestorOrSelf(currentPage));
        }

        public IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
            var overviewPage = GetOverviewPage();
            var type = GetCentralFeedTabType(overviewPage);
            yield return new ActivityFeedTabModel
            {
                Content = overviewPage,
                Type = type,
                IsActive = overviewPage.Id == currentPage.Id,
                Links = _centralFeedLinkService.GetCreateLinks(type)
            };

            foreach (var content in GetContents())
            {
                var tabType = GetCentralFeedTabType(content);
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null)
                {
                    continue;
                }
                var settings = _centralFeedService.GetSettings(tabType);
                yield return new ActivityFeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    HasSubscribersFilter = settings.HasSubscribersFilter,
                    HasPinnedFilter = settings.HasPinnedFilter,
                    IsActive = content.IsAncestorOrSelf(currentPage),
                    Links = _centralFeedLinkService.GetCreateLinks(tabType),
                };
            }
        }

        public void SaveFiltersState(FeedFiltersState stateModel)
        {
            var cookie = _cookieProvider.Get(CentralFeedFiltersStateCookieName);
            cookie.Value = stateModel.ToJson();
            _cookieProvider.Save(cookie);
        }

        public TStateServer GetFiltersState<TStateServer>()
        {
            var cookie = _cookieProvider.Get(CentralFeedFiltersStateCookieName);
            if (string.IsNullOrEmpty(cookie?.Value))
            {
                cookie = new HttpCookie(CentralFeedFiltersStateCookieName)
                {
                    Expires = DateTime.Now.AddDays(7),
                    Value = GetDefaultCentralFeedFiltersState().ToJson()
                };
                _cookieProvider.Save(cookie);
            }
            return cookie.Value.Deserialize<TStateServer>();
        }

        public bool CentralFeedCookieExists()
        {
            return _cookieProvider.Exists(CentralFeedFiltersStateCookieName);
        }

        public IIntranetType GetCentralFeedTabType(IPublishedContent content)
        {
            return GetActivityTypeFromPlugin(content, CentralFeedConstants.CentralFeedPluginAlias);
        }

        public IIntranetType GetCreateActivityType(IPublishedContent content)
        {
            return GetActivityTypeFromPlugin(content, CentralFeedConstants.ActivityCreatePluginAlias);
        }

        public IIntranetType GetActivityTypeFromPlugin(IPublishedContent content, string gridPluginAlias)
        {
            var values = _gridHelper.GetValues(content, gridPluginAlias);
            var value = values.FirstOrDefault(v => v.tabType != null);

            if (value == null)
                return _feedTypeProvider.Get(default(CentralFeedTypeEnum).ToInt());

            var tabTypeId = int.TryParse(value.tabType.ToString(), out int result)
                ? result
                : default(CentralFeedTypeEnum).ToInt();
            return _feedTypeProvider.Get(tabTypeId);
        }

        private IEnumerable<IPublishedContent> GetContents()
        {
            var activityTypes = _activityTypeProvider.GetAll();
            var activitiesList = activityTypes.Select(_documentTypeAliasProvider.GetOverviewPage).ToArray();

            return GetOverviewPage().Children.Where(c => c.DocumentTypeAlias.In(activitiesList));
        }

        private FeedFiltersState GetDefaultCentralFeedFiltersState()
        {
            return new FeedFiltersState()
            {
                BulletinFilterSelected = true
            };
        }
    }
}