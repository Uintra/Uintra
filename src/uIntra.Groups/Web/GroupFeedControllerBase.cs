using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.Attributes;
using uIntra.Core.Extensions;
using uIntra.Core.Feed;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.Groups.Web
{
    public abstract class GroupFeedControllerBase : FeedControllerBase
    {
        private readonly IGroupFeedService _groupFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;
        private readonly IGroupFeedContentService _groupFeedContentContentService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IFeedFilterStateService _feedFilterStateService;
        private readonly IGroupFeedLinkService _groupFeedLinkService;
        private readonly IActivityTypeProvider _activityTypeProvider;

        private bool IsCurrentUserGroupMember { get; set; }

        protected override string OverviewViewPath => "~/App_Plugins/Groups/Room/Feed/Overview.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/Groups/Room/Feed/Details.cshtml";
        protected override string CreateViewPath => "~/App_Plugins/Groups/Room/Feed/Create.cshtml";
        protected override string EditViewPath => "~/App_Plugins/Groups/Room/Feed/Edit.cshtml";
        protected override string ListViewPath => "~/App_Plugins/Groups/Room/Feed/List.cshtml";

        protected GroupFeedControllerBase(
            ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentProvider intranetUserContentProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IGroupMember> intranetUserService,
            IGroupFeedContentService groupFeedContentContentService,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupFeedLinkService groupFeedLinkService,
            IGroupMemberService groupMemberService,
            IFeedFilterStateService feedFilterStateService,
            IActivityTypeProvider activityTypeProvider)
            : base(subscribeService,
                groupFeedService,
                intranetUserService,
                feedFilterStateService,
                centralFeedTypeProvider)
        {
            _groupFeedService = groupFeedService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _intranetUserService = intranetUserService;
            _groupFeedContentContentService = groupFeedContentContentService;
            _groupFeedLinkService = groupFeedLinkService;
            _groupMemberService = groupMemberService;
            _feedFilterStateService = feedFilterStateService;
            _activityTypeProvider = activityTypeProvider;
        }

        #region Actions

        [HttpGet]
        public ActionResult Overview(Guid groupId)
        {
            var model = GetOverviewModel(groupId);
            return PartialView(OverviewViewPath, model);
        }

        [HttpGet]
        [NotFoundActivity]
        public virtual ActionResult Details(Guid id, Guid groupId)
        {
            var viewModel = GetDetailsViewModel(id, groupId);
            return PartialView(DetailsViewPath, viewModel);
        }

        [HttpGet]
        public ActionResult Create(Guid groupId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            if (!_groupMemberService.IsGroupMember(groupId, currentUser))
                return new EmptyResult();

            var activityType = _groupFeedContentContentService.GetCreateActivityType(CurrentPage);
            var viewModel = GetCreateViewModel(activityType, groupId);
            return PartialView(CreateViewPath, viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(Guid id, Guid groupId)
        {
            var viewModel = GetEditViewModel(id, groupId);
            return PartialView(EditViewPath, viewModel);
        }

        public ActionResult List(GroupFeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider[model.TypeId];
            var items = GetGroupFeedItems(centralFeedType, model.GroupId).ToList();
            var tabSettings = _groupFeedService.GetSettings(centralFeedType);

            if (IsEmptyFilters(model.FilterState, _feedFilterStateService.CentralFeedCookieExists()))
            {
                model.FilterState = GetFilterStateModel();
            }

            var filteredItems = ApplyFilters(items, model.FilterState, tabSettings).ToList();
            var currentVersion = _groupFeedService.GetFeedVersion(filteredItems);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var centralFeedModel = GetFeedListViewModel(model, filteredItems, centralFeedType);
            var filterState = MapToFilterState(centralFeedModel.FilterState);
            _feedFilterStateService.SaveFiltersState(filterState);

            return PartialView(ListViewPath, centralFeedModel);
        }
        #endregion

        protected virtual IEnumerable<IFeedItem> GetGroupFeedItems(Enum type, Guid groupId)
        {
            return type is CentralFeedTypeEnum.All
                ? _groupFeedService.GetFeed(groupId).OrderByDescending(item => item.PublishDate)
                : _groupFeedService.GetFeed(type, groupId);
        }

        protected virtual FeedListViewModel GetFeedListViewModel(GroupFeedListModel model, List<IFeedItem> filteredItems, Enum centralFeedType)
        {
            var take = model.Page * ItemsPerPage;
            var pagedItemsList = SortForFeed(filteredItems, centralFeedType).Take(take).ToList();

            var settings = _groupFeedService.GetAllSettings().ToList();
            var tabSettings = settings
                .Single(s => s.Type.ToInt() == model.TypeId)
                .Map<FeedTabSettings>();

            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            IsCurrentUserGroupMember = _groupMemberService.IsGroupMember(model.GroupId, currentUserId); // I know that state is not the nice idea, but I cant find another way to remove logic duplication

            return new FeedListViewModel
            {
                Version = _groupFeedService.GetFeedVersion(filteredItems),
                Feed = GetFeedItems(pagedItemsList, settings),
                TabSettings = tabSettings,
                Type = centralFeedType,
                BlockScrolling = filteredItems.Count < take,
                FilterState = MapToFilterStateViewModel(model.FilterState)
            };
        }

        protected override ActivityFeedOptions GetActivityFeedOptions(Guid id)
        {
            return new ActivityFeedOptions
            {
                Links = _groupFeedLinkService.GetLinks(id),
                IsReadOnly = !IsCurrentUserGroupMember
            };
        }

        protected virtual GroupFeedOverviewModel GetOverviewModel(Guid groupId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var tabType = _groupFeedContentContentService.GetFeedTabType(CurrentPage);

            var tabs = _groupFeedContentContentService.GetActivityTabs(CurrentPage, currentUser, groupId);
            var activityTabs = tabs.Where(t => t.Type != null).Map<List<ActivityFeedTabViewModel>>();

            var model = new GroupFeedOverviewModel
            {
                Tabs = activityTabs,
                TabsWithCreateUrl = GetTabsWithCreateUrl(activityTabs),
                CurrentType = tabType,
                GroupId = groupId,
                IsGroupMember = _groupMemberService.IsGroupMember(groupId, currentUser)
            };
            return model;
        }

        protected virtual CreateViewModel GetCreateViewModel(Enum activityType, Guid groupId)
        {
            var links = _groupFeedLinkService.GetCreateLinks(activityType, groupId);
            var settings = _groupFeedService.GetSettings(activityType);

            return new CreateViewModel
            {
                Links = links,
                Settings = settings
            };
        }

        protected virtual EditViewModel GetEditViewModel(Guid id, Guid groupId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = _groupFeedLinkService.GetLinks(id);
            var settings = _groupFeedService.GetSettings(service.ActivityType);

            var viewModel = new EditViewModel
            {
                Id = id,
                Links = links,
                Settings = settings
            };
            return viewModel;
        }

        protected virtual DetailsViewModel GetDetailsViewModel(Guid id, Guid groupId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            IsCurrentUserGroupMember = _groupMemberService.IsGroupMember(groupId, currentUserId);
            var options = GetActivityFeedOptions(id);
            var settings = _groupFeedService.GetSettings(service.ActivityType);

            var viewModel = new DetailsViewModel
            {
                Id = id,
                Options = options,
                Settings = settings
            };
            return viewModel;
        }
    }
}