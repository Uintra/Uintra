﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.CentralFeed;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Subscribe;

namespace uIntra.Groups.Web
{
    public abstract class GroupFeedControllerBase : FeedControllerBase
    {
        private readonly ICentralFeedContentHelper _centralFeedContentHelper;
        private readonly IGroupFeedService _groupFeedService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IGroupContentHelper _groupContentHelper;
        private readonly IGroupService _groupService;
        private IGroupFeedLinkService _groupFeedLinkService;

        // TODO : remove redundancies in pathes
        protected override string OverviewViewPath => "~/App_Plugins/Groups/Feed/GroupFeedOverviewView.cshtml";
        protected override string DetailsViewPath => "~/App_Plugins/Groups/Feed/GroupFeedDetailsView.cshtml";
        protected override string CreateViewPath => "~/App_Plugins/Groups/Feed/GroupFeedCreateView.cshtml";
        protected override string EditViewPath => "~/App_Plugins/Groups/Feed/GroupFeedDetailsView.cshtml";

        protected override string ListViewPath => "~/App_Plugins/Groups/Feed/GroupFeedList.cshtml";
        protected override string NavigationViewPath => "-";
        protected override string LatestActivitiesViewPath => "-";

        protected GroupFeedControllerBase(
            ICentralFeedContentHelper centralFeedContentHelper,
            ISubscribeService subscribeService,
            IGroupFeedService groupFeedService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IFeedTypeProvider centralFeedTypeProvider,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IGroupContentHelper groupContentHelper,
            IGroupService groupService)
            : base(centralFeedContentHelper,
                  subscribeService,
                  groupFeedService,                 
                  intranetUserService)
        {
            _centralFeedContentHelper = centralFeedContentHelper;
            _groupFeedService = groupFeedService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _intranetUserService = intranetUserService;
            _groupContentHelper = groupContentHelper;
            _groupService = groupService;
        }

        internal interface IGroupFeedLinkService
        {
            ActivityLinks GetLinks(IFeedItem item, Guid groupId);
            ActivityLinks GetCreateLinks(Guid groupId);
        }

        #region Actions

        [HttpGet]
        public ActionResult Overview(Guid groupId)
        {
            var model = GetOverviewModel(groupId);
            return PartialView(OverviewViewPath, model);
        }

        [HttpGet]
        public virtual ActionResult Details(Guid id, Guid groupId)
        {
            var viewModel = GetDetailsViewModel(id, groupId);
            return PartialView(DetailsViewPath, viewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(Guid id, Guid groupId)
        {
            var viewModel = GetEditViewModel(id, groupId);
            return PartialView(EditViewPath, viewModel);
        }

        public ActionResult List(GroupFeedListModel model)
        {
            var centralFeedType = _centralFeedTypeProvider.Get(model.TypeId);
            var items = GetGroupFeedItems(centralFeedType, model.GroupId).ToList();
            var tabSettings = _groupFeedService.GetSettings(centralFeedType);

            if (IsEmptyFilters(model.FilterState, _centralFeedContentHelper.CentralFeedCookieExists()))
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
            _centralFeedContentHelper.SaveFiltersState(filterState);

            return PartialView(ListViewPath, centralFeedModel);
        }

        public override ActionResult Create(int typeId)
        {
            var groupId = _groupService.GetGroupIdFromQuery(Request.QueryString.ToString());

            if (!groupId.HasValue)
                throw new NotImplementedException();

            var activityType = _centralFeedTypeProvider.Get(typeId);
            var viewModel = GetCreateViewModel(activityType, groupId.Value);
            return PartialView(CreateViewPath, viewModel);
        }
        #endregion

        protected virtual IEnumerable<IFeedItem> GetGroupFeedItems(IIntranetType type, Guid groupId)
        {
            if (type.Id == CentralFeedTypeEnum.All.ToInt())
            {
                var items = _groupFeedService.GetFeed(groupId).OrderByDescending(item => item.PublishDate);
                return items;
            }

            return _groupFeedService.GetFeed(type, groupId);
        }

        protected virtual FeedListViewModel GetFeedListViewModel(GroupFeedListModel model, List<IFeedItem> filteredItems, IIntranetType centralFeedType)
        {
            var take = model.Page * ItemsPerPage;
            var pagedItemsList = Sort(filteredItems, centralFeedType).Take(take).ToList();

            var settings = _groupFeedService.GetAllSettings();
            var tabSettings = settings
                .Single(s => s.Type.Id == model.TypeId)
                .Map<FeedTabSettings>();

            return new FeedListViewModel
            {
                Version = _groupFeedService.GetFeedVersion(filteredItems),
                Feed = GetFeedItems(pagedItemsList, settings, model.GroupId),
                TabSettings = tabSettings,
                Type = centralFeedType,
                BlockScrolling = filteredItems.Count < take,
                FilterState = MapToFilterStateViewModel(model.FilterState)
            };
        }

        protected virtual IEnumerable<FeedItemViewModel> GetFeedItems(IEnumerable<IFeedItem> items, IEnumerable<FeedSettings> settings, Guid groupId)
        {
            var activitySettings = settings
                .ToDictionary(s => s.Type);

            var result = items
                .Select(i => new FeedItemViewModel()
                {
                    Item = i,
                    Links = _groupFeedLinkService.GetLinks(i, groupId),
                    ControllerName = activitySettings[i.Type].Controller
                });

            return result;
        }

        protected virtual GroupFeedOverviewModel GetOverviewModel(Guid groupId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var tabType = _groupContentHelper.GetTabType(CurrentPage);

            var tabs = _groupContentHelper.GetTabs(groupId, currentUser, CurrentPage).Select(t => MapFeedTabToViewModel(t, groupId));

            var model = new GroupFeedOverviewModel
            {
                Tabs = tabs,
                CurrentType = tabType,
                GroupId = groupId
            };
            return model;
        }

        private FeedTabViewModel MapFeedTabToViewModel(FeedTabModel tab, Guid groupId)
        {
            return new FeedTabViewModel()
            {
                Type = tab.Type,
                CreateUrl = tab.CreateUrl?.AddGroupId(groupId),
                Url = tab.Content.Url.AddGroupId(groupId),
                IsActive = tab.IsActive
            };
        }

        protected virtual CreateViewModel GetCreateViewModel(IIntranetType activityType, Guid groupId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(activityType.Id);
            var links = service.GetGroupFeedCreateLinks(groupId);

            var settings = _groupFeedService.GetSettings(activityType);

            return new CreateViewModel()
            {
                Links = links,
                Settings = settings
            };
        }

        protected virtual EditViewModel GetEditViewModel(Guid id, Guid groupId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService>(id);
            var links = service.GetGroupFeedLinks(id, groupId);

            var type = service.ActivityType;
            var settings = _groupFeedService.GetSettings(type);

            var viewModel = new EditViewModel()
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
            var links = service.GetGroupFeedLinks(id, groupId);

            var type = service.ActivityType;
            var settings = _groupFeedService.GetSettings(type);

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
