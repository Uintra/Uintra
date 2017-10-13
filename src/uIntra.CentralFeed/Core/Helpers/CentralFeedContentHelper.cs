using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.CentralFeed.Providers;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.CentralFeed
{
    public class CentralFeedContentHelper : FeedContentHelperBase, ICentralFeedContentHelper
    {
        private const string CentralFeedFiltersStateCookieName = "centralFeedFiltersState";
        private readonly ICentralFeedService _centralFeedService;

        private readonly ICookieProvider _cookieProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly ICentralFeedLinkService _centralFeedLinkService;
        private readonly ICentralFeedContentProvider _contentProvider;

        public CentralFeedContentHelper(
            ICentralFeedService centralFeedService,
            ICookieProvider cookieProvider,
            IActivityTypeProvider activityTypeProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            ICentralFeedLinkService centralFeedLinkService,
            ICentralFeedContentProvider contentProvider)
        {
            _centralFeedService = centralFeedService;
            _cookieProvider = cookieProvider;
            _activityTypeProvider = activityTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _centralFeedLinkService = centralFeedLinkService;
            _contentProvider = contentProvider;
        }

        public bool IsCentralFeedPage(IPublishedContent currentPage)
        {
            return _contentProvider.GetOverviewPage().Id == currentPage.Id || GetContents().Any(c => c.IsAncestorOrSelf(currentPage));
        }

        public IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
            var overviewPage = _contentProvider.GetOverviewPage();
            var type = GetFeedTabType(overviewPage);
            yield return new ActivityFeedTabModel
            {
                Content = overviewPage,
                Type = type,
                IsActive = overviewPage.Id == currentPage.Id,
                Links = _centralFeedLinkService.GetCreateLinks(type)
            };

            foreach (var content in GetContents())
            {
                var tabType = GetFeedTabType(content);
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

        private IEnumerable<IPublishedContent> GetContents()
        {
            var activityTypes = _activityTypeProvider.GetAll();
            var activitiesList = activityTypes.Select(_documentTypeAliasProvider.GetOverviewPage).ToArray();

            return _contentProvider.GetOverviewPage().Children.Where(c => c.DocumentTypeAlias.In(activitiesList));
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