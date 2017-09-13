using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.Groups.Web
{
    public abstract class GroupFeedControllerBase : FeedControllerBase
    {
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly ICentralFeedService _centralFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IGroupContentHelper _groupContentHelper;
        protected override string OverviewViewPath => "~/App_Plugins/Groups/Feed/GroupFeedOverviewView.cshtml";

        protected override string DetailsViewPath => "~/App_Plugins/Groups/Feed/GroupFeedDetailsView.cshtml";
        protected override string CreateViewPath => "~/App_Plugins/Groups/Feed/GroupFeedDetailsView.cshtml";
        protected override string EditViewPath => "~/App_Plugins/Groups/Feed/GroupFeedDetailsView.cshtml";

        protected override string ListViewPath => "~/App_Plugins/Groups/Feed/GroupFeedList.cshtml";
        protected override string NavigationViewPath => "-";
        protected override string LatestActivitiesViewPath => "-";

        public GroupFeedControllerBase(
            ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            ICentralFeedService centralFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            ICentralFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IGroupContentHelper groupContentHelper) 
            : base(centralFeedContentHelper,
                  subscribeService,
                  centralFeedService,
                  activitiesServiceFactory,
                  intranetUserContentHelper,
                  centralFeedTypeProvider,
                  intranetUserService)
        {
            _centralFeedContentHelper = centralFeedContentHelper;
            _centralFeedService = centralFeedService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _intranetUserService = intranetUserService;
            _groupContentHelper = groupContentHelper;
        }

        [NonAction]
        public override ActionResult Overview()
        {
            return base.Overview();
        }

        public ActionResult Overview(Guid groupId)
        {
            var model = GetOverviewModel(groupId);
            return PartialView(OverviewViewPath, model);
        }

        [NonAction]
        public override ActionResult List(CentralFeedListModel model)
        {
            return base.List(model);
        }

        public virtual ActionResult List(GroupFeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider.Get(model.TypeId);
            var items = GetCentralFeedItems(centralFeedType).ToList();

            //if (IsEmptyFilters(model.FilterState, _centralFeedContentHelper.CentralFeedCookieExists()))
            //{
            //    model.FilterState = GetFilterStateModel();
            //}

            var tabSettings = _centralFeedService.GetSettings(centralFeedType);

            var filteredItems = ApplyFilters(items, model.FilterState, tabSettings, model.GroupId).ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(filteredItems);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var centralFeedModel = GetCentralFeedListViewModel(model, filteredItems, centralFeedType);
            //var filterState = MapToFilterState(centralFeedModel.FilterState);
            //_centralFeedContentHelper.SaveFiltersState(filterState);

            return PartialView(ListViewPath, centralFeedModel);
        }

        protected virtual IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings, Guid groupId)
        {
            return base.ApplyFilters(items, filterState, settings)
                .Where(i => i is IGroupable && ((IGroupable) i).GroupIds.Contains(groupId));
        }

        // TODO : think how we can reduce duplication
        protected virtual GroupFeedOverviewModel GetOverviewModel(Guid groupId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var tabType = _centralFeedContentHelper.GetTabType(CurrentPage);

            var centralFeedState = _centralFeedContentHelper.GetFiltersState<FeedFiltersState>();
            var tabs = _groupContentHelper.GetTabs(groupId, currentUser, CurrentPage).Map<IEnumerable<CentralFeedTabViewModel>>();

            var model = new GroupFeedOverviewModel
            {
                Tabs = tabs,
                CurrentType = tabType,
                IsFiltersOpened = centralFeedState.IsFiltersOpened,
                GroupId = groupId
            };
            return model;
        }

        protected override CreateViewModel GetCreateViewModel(IIntranetType activityType)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(activityType.Id);
            var links = service.GetGroupFeedCreateLinks();

            var settings = _centralFeedService.GetSettings(activityType);

            return new CreateViewModel()
            {
                Links = links,
                Settings = settings
            };
        }

        protected override EditViewModel GetEditViewModel(Guid id)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = service.GetCentralFeedLinks(id);

            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);

            var viewModel = new EditViewModel()
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return viewModel;
        }

        protected override DetailsViewModel GetDetailsViewModel(Guid id)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = service.GetCentralFeedLinks(id);

            var type = service.ActivityType;
            var settings = _centralFeedService.GetSettings(type);

            var viewModel = new DetailsViewModel()
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return viewModel;
        }
    }
}
