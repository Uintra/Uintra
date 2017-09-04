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
using uIntra.Core.User.Permissions;
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
        private readonly IPermissionsService _permissionsService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public CentralFeedContentHelper(
            UmbracoHelper umbracoHelper,
            ICentralFeedService centralFeedService,
            IGridHelper gridHelper,
            ICookieProvider cookieProvider,
            IPermissionsService permissionsService, IActivityTypeProvider activityTypeProvider,
            ICentralFeedTypeProvider centralFeedTypeProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _umbracoHelper = umbracoHelper;
            _centralFeedService = centralFeedService;
            _gridHelper = gridHelper;
            _cookieProvider = cookieProvider;
            _permissionsService = permissionsService;
            _activityTypeProvider = activityTypeProvider;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage()));
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
                var activityType = tabType.Id.ToEnum<IntranetActivityTypeEnum>();

                if (activityType == null)
                {
                    continue;
                }

                var canCreate = _permissionsService.IsCurrentUserHasAccess(tabType, IntranetActivityActionEnum.Create);

                var settings = _centralFeedService.GetSettings(tabType);
                yield return new CentralFeedTabModel
                {
                    Content = content,
                    Type = tabType,
                    HasSubscribersFilter = settings.HasSubscribersFilter,
                    HasPinnedFilter = settings.HasPinnedFilter,
                    CreateUrl = canCreate ? content.Children.SingleOrDefault(n => n.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetCreatePage(tabType)))?.Url : null,
                    IsActive = content.IsAncestorOrSelf(currentPage)
                };
            }
        }

        public void SaveFiltersState(CentralFeedFiltersStateModel stateModel)
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

        public IIntranetType GetTabType(IPublishedContent content)
        {
            var value = _gridHelper.GetValue(content, "custom.CentralFeed");

            if (value == null || value.tabType == null)
            {
                return _centralFeedTypeProvider.Get(default(CentralFeedTypeEnum).ToInt());
            }

            int tabType;
            if (int.TryParse(value.tabType.ToString(), out tabType))
            {
                return _centralFeedTypeProvider.Get(tabType);
            }
            return _centralFeedTypeProvider.Get(default(CentralFeedTypeEnum).ToInt());
        }

        private IEnumerable<IPublishedContent> GetContents()
        {
            var activityTypes = _activityTypeProvider.GetAll();
            var activitiesList = activityTypes.Select(_documentTypeAliasProvider.GetOverviewPage).ToArray();

            return GetOverviewPage().Children.Where(c => c.DocumentTypeAlias.In(activitiesList));
        }

        private CentralFeedFiltersStateModel GetDefaultCentralFeedFiltersState()
        {
            return new CentralFeedFiltersStateModel()
            {
                BulletinFilterSelected = true
            };
        }
    }
}