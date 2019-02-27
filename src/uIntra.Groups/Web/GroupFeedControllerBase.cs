using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Uintra.CentralFeed;
using Uintra.CentralFeed.Web;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Attributes;
using Uintra.Core.Context;
using Uintra.Core.Extensions;
using Uintra.Core.Feed;
using Uintra.Core.Permissions;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Groups.Attributes;
using Uintra.Subscribe;

namespace Uintra.Groups.Web
{
    public abstract class GroupFeedControllerBase : FeedControllerBase
    {
        private readonly IGroupFeedService _groupFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IIntranetMemberService<IGroupMember> _intranetMemberService;
        private readonly IGroupFeedContentService _groupFeedContentContentService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IFeedFilterStateService<FeedFiltersState> _feedFilterStateService;
        private readonly IPermissionsService _permissionsService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IFeedFilterService _feedFilterService;


        private bool IsCurrentMemberInGroup { get; set; }

        protected override string OverviewViewPath => "~/App_Plugins/Groups/Room/Feed/Overview.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/Groups/Room/Feed/Details.cshtml";
        protected override string CreateViewPath => "~/App_Plugins/Groups/Room/Feed/Create.cshtml";
        protected override string EditViewPath => "~/App_Plugins/Groups/Room/Feed/Edit.cshtml";
        protected override string ListViewPath => "~/App_Plugins/Groups/Room/Feed/List.cshtml";

        public override ContextType ControllerContextType { get; } = ContextType.GroupFeed;

        protected GroupFeedControllerBase(
            ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentProvider intranetUserContentProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetMemberService<IGroupMember> intranetMemberService,
            IGroupFeedContentService groupFeedContentContentService,
            IGroupFeedLinkProvider groupFeedLinkProvider,
            IGroupMemberService groupMemberService,
            IFeedFilterStateService<FeedFiltersState> feedFilterStateService,
            IPermissionsService permissionsService,
            IContextTypeProvider contextTypeProvider,
            IFeedLinkService feedLinkService,
            IFeedFilterService feedFilterService)
            : base(
                  subscribeService,
                  groupFeedService,
                  intranetMemberService,
                  feedFilterStateService,
                  centralFeedTypeProvider,
                  contextTypeProvider)
        {
            _groupFeedService = groupFeedService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _intranetMemberService = intranetMemberService;
            _groupFeedContentContentService = groupFeedContentContentService;
            _groupMemberService = groupMemberService;
            _feedFilterStateService = feedFilterStateService;
            _permissionsService = permissionsService;
            _feedLinkService = feedLinkService;
            _feedFilterService = feedFilterService;
        }

        #region Actions

        [HttpGet]
        [NotFoundGroup]
        public ActionResult Overview(Guid groupId)
        {
            var model = GetOverviewModel(groupId);
            return PartialView(OverviewViewPath, model);
        }

        [HttpGet]
        [NotFoundActivity]
        [NotFoundGroup]
        public virtual ActionResult Details(Guid id, Guid groupId)
        {
            var viewModel = GetDetailsViewModel(id, groupId);
            return PartialView(DetailsViewPath, viewModel);
        }

        [HttpGet]
        [NotFoundGroup]
        public ActionResult Create(Guid groupId)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            if (!_groupMemberService.IsGroupMember(groupId, currentMember))
                return new EmptyResult();

            var activityType = _groupFeedContentContentService.GetCreateActivityType(CurrentPage);
            var viewModel = GetCreateViewModel(activityType, groupId);
            return PartialView(CreateViewPath, viewModel);
        }

        [HttpGet]
        [NotFoundActivity]
        [NotFoundGroup]
        public virtual ActionResult Edit(Guid id, Guid groupId)
        {
            var viewModel = GetEditViewModel(id, groupId);
            return PartialView(EditViewPath, viewModel);
        }

        [NotFoundGroup]
        public ActionResult List(GroupFeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider[model.TypeId];
            var items = GetGroupFeedItems(centralFeedType, model.GroupId).ToList();
            var tabSettings = _groupFeedService.GetSettings(centralFeedType);

            if (IsEmptyFilters(model.FilterState, _feedFilterStateService.CentralFeedCookieExists()))
            {
                model.FilterState = GetFilterStateModel();
            }

            var filteredItems = _feedFilterService.ApplyFilters(items, model.FilterState, tabSettings).ToList();
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

            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            IsCurrentMemberInGroup = _groupMemberService.IsGroupMember(model.GroupId, currentMemberId); // I know that state is not the nice idea, but I cant find another way to remove logic duplication

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
                Links = _feedLinkService.GetLinks(id),
                IsReadOnly = !IsCurrentMemberInGroup
            };
        }

        protected virtual GroupFeedOverviewModel GetOverviewModel(Guid groupId)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var tabType = _groupFeedContentContentService.GetFeedTabType(CurrentPage);

            var tabs = _groupFeedContentContentService.GetActivityTabs(CurrentPage, currentMember, groupId);
            var activityTabs = tabs.Where(t => t.Type != null).Map<List<ActivityFeedTabViewModel>>();

            var model = new GroupFeedOverviewModel
            {
                Tabs = activityTabs,
                TabsWithCreateUrl = GetTabsWithCreateUrl(activityTabs).Where(tab => _permissionsService.Check(ToPermissionActivityType(tab.Type), PermissionActionEnum.Create)),
                CurrentType = tabType,
                GroupId = groupId,
                IsGroupMember = _groupMemberService.IsGroupMember(groupId, currentMember),
                CanCreateBulletin = _permissionsService.Check(PermissionActivityTypeEnum.Bulletins, PermissionActionEnum.Create)
            };
            return model;
        }

        protected virtual CreateViewModel GetCreateViewModel(Enum activityType, Guid groupId)
        {
            var links = _feedLinkService.GetCreateLinks(activityType, groupId);
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
            var links = _feedLinkService.GetLinks(id);
            var settings = _groupFeedService.GetSettings(service.Type);

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
            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            IsCurrentMemberInGroup = _groupMemberService.IsGroupMember(groupId, currentMemberId);
            var options = GetActivityFeedOptions(id);
            var settings = _groupFeedService.GetSettings(service.Type);

            var viewModel = new DetailsViewModel
            {
                Id = id,
                Options = options,
                Settings = settings
            };
            return viewModel;
        }

        private PermissionActivityTypeEnum ToPermissionActivityType(Enum type) => (PermissionActivityTypeEnum)type.ToInt();
    }
}