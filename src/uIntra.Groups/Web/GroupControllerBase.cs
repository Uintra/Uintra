using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Examine;
using uIntra.CentralFeed;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups.Extentions;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Groups.Web
{
    public abstract class GroupControllerBase : SurfaceController
    {
        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupContentHelper _groupContentHelper;
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;
        private readonly IGroupMediaService _groupMediaService;
        public abstract string GroupOverviewPageTypeAlias { get; }

        private int ItemsPerPage = 10;

        public GroupControllerBase(IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupContentHelper groupContentHelper,
            IUserService userService,
            IGroupMediaService groupMediaService,
            IIntranetUserService<IGroupMember> intranetUserService)
        {
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _mediaHelper = mediaHelper;
            _groupContentHelper = groupContentHelper;
            _groupMediaService = groupMediaService;
            _intranetUserService = intranetUserService;
        }

        public ActionResult Overview()
        {
            return PartialView("~/App_Plugins/Groups/List/GroupsOverview.cshtml", new GroupsOverviewModel { IsMyGroupsPage = false });
        }

        public ActionResult DisabledView(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            if (!group.IsHidden)
            {
                var deactivatedGroupPage = _groupContentHelper.GetGroupRoomPage();
                Response.Redirect(deactivatedGroupPage.UrlWithGroupId(groupId), true);
            }

            return PartialView("~/App_Plugins/Groups/DisablerGroupView/DisabledGroup.cshtml");
        }

        public ActionResult MyGroupsOverview()
        {
            return PartialView("~/App_Plugins/Groups/List/GroupsOverview.cshtml", new GroupsOverviewModel() { IsMyGroupsPage = true });
        }

        [DisabledGroupActionFilter]
        public ActionResult CentralFeedOverview(Guid groupId)
        {
            var model = new GroupCentralFeedOverviewModel
            {
                CurrentType = _groupContentHelper.GetTabType(CurrentPage),
                GroupId = groupId,

            };

            var allTabs = _groupContentHelper.GetTabs(groupId, _intranetUserService.GetCurrentUser(), CurrentPage).ToList();
            var activityTabs = allTabs.FindAll(t => t.Type != null);

            model.ActivityTabs = activityTabs.Select(tab => new CentralFeedTabViewModel
            {
                Type = tab.Type,
                Url = tab.Content.UrlWithGroupId(groupId),
                IsActive = tab.IsActive
            });

            model.CreateTabs = activityTabs.Where(t => !StringExtentions.IsNullOrEmpty(t.CreateUrl)).Select(tab =>
            {
                var tabModel = tab.Map<GroupNavigationCreateTabViewModel>();
                tabModel.Url = tab.CreateUrl.UrlWithGroupId(groupId);
                return tabModel;
            });

            return PartialView("~/App_Plugins/Groups/CentralFeed/GroupCentralFeedOverview.cshtml", model);
        }

        [DisabledGroupActionFilter]
        public ActionResult Edit(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            if (!_groupService.CanEdit(group, _intranetUserService.GetCurrentUser()))
            {
                HttpContext.Response.Redirect(_groupContentHelper.GetGroupRoomPage().UrlWithGroupId(groupId));
            }

            var model = group.Map<GroupEditModel>();
            //var mediaRootAlias = GroupOverviewPageTypeAlias.GetModelPropertyType(s => s.MediaRootId).PropertyTypeAlias; TODO
            var overviewPage = _groupContentHelper.GetOverviewPage();
            //model.MediaRootId = overviewPage.GetPropertyValue<int?>(mediaRootAlias);
            model.AllowedMediaExtentions = overviewPage.GetPropertyValue<string>("allowedMediaExtensions", "");

            FillLinks();
            return PartialView("~/App_Plugins/Groups/Edit/EditGroupView.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(GroupEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var group = _groupService.Get(model.Id);
            group = Mapper.Map(model, group);
            group.ImageId = model.Media?.Split(',').First().ToNullableInt();
            var createdMedias = _mediaHelper.CreateMedia(model).ToList();
            if (createdMedias.Any())
            {
                group.ImageId = createdMedias.First();
            }
            _groupService.Edit(group);
            _groupMediaService.GroupTitleChanged(group.Id, group.Title);
            return RedirectToUmbracoPage(_groupContentHelper.GetGroupRoomPage(), new NameValueCollection { { "groupId", group.Id.ToString() } });
        }

        public ActionResult Create()
        {
            var createGroupModel = new GroupCreateModel();


            //var mediaRootAlias = GroupOverviewPageTypeAlias.GetModelPropertyType(s => s.MediaRootId).PropertyTypeAlias;
            var overviewPage = _groupContentHelper.GetOverviewPage();
            //createGroupModel.MediaRootId = overviewPage.GetPropertyValue<int?>(mediaRootAlias);
            createGroupModel.CreatorId = _intranetUserService.GetCurrentUser().Id;
            //createGroupModel.AllowedMediaExtentions = overviewPage.GetPropertyValue<string>("allowedMediaExtensions", "");

            FillLinks();
            return PartialView("~/App_Plugins/Groups/Create/CreateGroupView.cshtml", createGroupModel);
        }

        [HttpPost]
        public ActionResult Create(GroupCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            if (createModel.ParentActivityId.HasValue)
            {
                var existedGroup = _groupService.GetGroupByActivity(createModel.ParentActivityId.Value);
                if (existedGroup != null)
                {
                    return RedirectToCurrentUmbracoPage(Request.QueryString);
                }
            }

            var group = createModel.Map<Sql.Group>();
            group.GroupTypeId = GroupTypeEnum.Open.GetHashCode();
            var createdMedias = _mediaHelper.CreateMedia(createModel).ToList();
            group.ImageId = createdMedias.Any() ? (int?)createdMedias.First() : null;

            _groupService.Create(group);
            _groupMediaService.GroupTitleChanged(group.Id, group.Title);


                _groupMemberService.Add(group.Id, createModel.CreatorId);
                var creator = _intranetUserService.Get(createModel.CreatorId);
                creator.GroupIds = creator.GroupIds.Concat(Enumerable.Repeat(group.Id, 1));
                //_intranetUserService.UpdateCache(Enumerable.Repeat(creator, 1));


            return RedirectToUmbracoPage(_groupContentHelper.GetGroupRoomPage(), new NameValueCollection { { "groupId", group.Id.ToString() } });
        }

        public ActionResult Index(bool isMyGroupsPage = false, int page = 1)
        {
            var take = page * ItemsPerPage;
            var currentUser = _intranetUserService.GetCurrentUser();
            List<Sql.Group> allGroups;
            if (isMyGroupsPage)
            {
                allGroups = _groupService.GetMany(currentUser.GroupIds).ToList();
            }
            else
            {
                allGroups = _groupService.GetAllNotHidden().ToList();
            }

            var groups = allGroups.Select(g =>
            {
                return MapGroupViewModel(g, () => isMyGroupsPage || _groupMemberService.IsGroupMember(g.Id, currentUser.Id));

            }).OrderByDescending(g => g.Creator.Id == currentUser.Id).ThenByDescending(s => s.IsMember).ThenBy(g => g.Title);

            return PartialView("~/App_Plugins/Groups/List/GroupsList.cshtml", new GroupsListModel()
            {
                Groups = groups.Take(take),
                BlockScrolling = allGroups.Count <= take
            });
        }

        [DisabledGroupActionFilter]
        public ActionResult Info(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            var groupInfo = group.Map<GroupInfoViewModel>();
            groupInfo.Creator = _intranetUserService.Get(group.CreatorId);
            var currentUser = _intranetUserService.GetCurrentUser();
            groupInfo.IsMember = _groupMemberService.IsGroupMember(group.Id, currentUser.Id);
            groupInfo.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupInfo.CanUnsubscribe = group.CreatorId != currentUser.Id;
            if (group.ImageId.HasValue)
            {
                groupInfo.GroupImageUrl = Umbraco.TypedMedia(group.ImageId.Value).Url;
            }
            return PartialView("~/App_Plugins/Groups/Info/GroupInfoView.cshtml", groupInfo);
        }

        [HttpPost]
        public ActionResult Hide(Guid id)
        {
            _groupService.Hide(id);
            //(_eventsService as IIndexer).FillIndex();
            //(_newsService as IIndexer).FillIndex();
            return Json(new { _groupContentHelper.GetOverviewPage().Url });
        }

        [DisabledGroupActionFilter]
        public PartialViewResult GroupSubscribe(Guid groupId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var subscribeModel = new GroupSubscribeViewModel
            {
                GroupId = groupId,
                IsMember = _groupMemberService.IsGroupMember(groupId, currentUser.Id),
                UserId = currentUser.Id,
                MembersCount = _groupMemberService.GetMembersCount(groupId)
            };

            return PartialView("~/App_Plugins/Groups/Info/GroupSubscribeView.cshtml", subscribeModel);
        }

        [HttpPost]
        [DisabledGroupActionFilter]
        public RedirectToUmbracoPageResult Subscribe(Guid groupId)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            if (_groupMemberService.IsGroupMember(groupId, currentUser.Id))
            {
                _groupMemberService.Remove(groupId, currentUser.Id);

            }
            else
            {
                _groupMemberService.Add(groupId, currentUser.Id);
            }
            UpdateUserCache(currentUser.Id);

            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }


        [DisabledGroupActionFilter]
        public ActionResult Unsubscribe(Guid groupId, Guid memberId)
        {
            _groupMemberService.Remove(groupId, memberId);

            UpdateUserCache(memberId);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        private void UpdateUserCache(Guid memberId)
        {
            var user = _intranetUserService.Get(memberId);
            user.GroupIds = _groupMemberService.GetGroupMemberByMember(user.Id).Select(u => u.GroupId);
            //_intranetUserService.UpdateCache(Utils.EnumerableExtensions.ToEnumerableOfOne(user));
        }

        [DisabledGroupActionFilter]
        public ActionResult GroupMembers(Guid groupId)
        {
            var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
            var membersIdsList = groupMembers.Select(s => s.MemeberId).ToList();
            var groupUsers = _intranetUserService.GetMany(membersIdsList);
            var group = _groupService.Get(groupId);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            var model = groupUsers.Select(s =>
                {
                    var viewModel = s.Map<GroupMemberViewModel>();
                    viewModel.IsGroupAdmin = s.Id == group.CreatorId;
                    viewModel.CanUnsubscribe = viewModel.Id == currentUserId && currentUserId != group.CreatorId;
                    return viewModel;
                })
                .OrderByDescending(s => s.IsGroupAdmin)
                .ThenBy(s => s.Name)
                .ToList();

            return PartialView("~/App_Plugins/Groups/Members/GroupMembers.cshtml", model);
        }


        private void FillLinks()
        {
            //ViewData.SetActivityOverviewPageUrl(_ideasService.ActivityType.Id, _ideasService.GetOverviewPage().Url);
        }

        private GroupViewModel MapGroupViewModel(Sql.Group group, Func<bool> fillIsMember)
        {
            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = fillIsMember();
            groupModel.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupModel.Creator = _intranetUserService.Get(group.CreatorId);
            groupModel.GroupUrl = _groupContentHelper.GetGroupRoomPage().Url.UrlWithQueryString("groupId", group.Id);
            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = Umbraco.TypedMedia(group.ImageId).Url;
            }

            return groupModel;
        }
    }
}
