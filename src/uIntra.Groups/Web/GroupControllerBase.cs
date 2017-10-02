using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups.Constants;
using uIntra.Groups.Extentions;
using uIntra.Groups.Sql;
using Umbraco.Web.Mvc;

namespace uIntra.Groups.Web
{
    public abstract class GroupControllerBase : SurfaceController
    {

        protected virtual string OverviewPath => "~/App_Plugins/Groups/List/Overview.cshtml";
        protected virtual string ListViewPath => "~/App_Plugins/Groups/List/List.cshtml";
        protected virtual string DisabledGroupViewPath => "~/App_Plugins/Groups/DisabledGroupView/DisabledGroup.cshtml";

        protected virtual string CreateViewPath => "~/App_Plugins/Groups/Create/CreateGroup.cshtml";
        protected virtual string EditViewPath => "~/App_Plugins/Groups/Edit/EditGroup.cshtml";

        protected virtual string SubscribeView => "~/App_Plugins/Groups/Room/Info/Subscribe.cshtml";
        protected virtual string MembersViewPath => "~/App_Plugins/Groups/Room/Members/Members.cshtml";
        protected virtual string InfoViewPath => "~/App_Plugins/Groups/Room/Info/Info.cshtml";


        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupHelper _groupHelper;
        private readonly IIntranetUserService<IGroupMember> _userService;
        private readonly IGroupMediaService _groupMediaService;
        private readonly IProfileLinkProvider _profileLinkProvider;

        protected int ItemsPerPage = 10;

        protected GroupControllerBase(IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupHelper groupHelper,
            IGroupMediaService groupMediaService,
            IIntranetUserService<IGroupMember> userService,
            IProfileLinkProvider profileLinkProvider)
        {
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _mediaHelper = mediaHelper;
            _groupHelper = groupHelper;
            _groupMediaService = groupMediaService;
            _userService = userService;
            _profileLinkProvider = profileLinkProvider;
        }

        public virtual ActionResult Overview()
        {
            return PartialView(OverviewPath, GetOverViewModel(false));
        }

        public virtual ActionResult DisabledView(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            if (!group.IsHidden)
            {
                var deactivatedGroupPage = _groupHelper.GetGroupRoomPage();
                Response.Redirect(deactivatedGroupPage.UrlWithGroupId(groupId), true);
            }

            return PartialView(DisabledGroupViewPath);
        }

        public ActionResult MyGroupsOverview()
        {
            return PartialView(OverviewPath, GetOverViewModel(true));
        }

        [DisabledGroupActionFilter]
        public ActionResult Edit(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            if (!_groupService.CanEdit(group, _userService.GetCurrentUser()))
            {
                HttpContext.Response.Redirect(_groupHelper.GetGroupRoomPage().UrlWithGroupId(groupId));
            }

            var model = group.Map<GroupEditModel>();
            var mediaSettings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent.ToInt());
            model.MediaRootId = mediaSettings.MediaRootId;
            model.AllowedMediaExtentions = mediaSettings.AllowedMediaExtentions;

            return PartialView(EditViewPath, model);
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
            return RedirectToUmbracoPage(_groupHelper.GetGroupRoomPage(),
                new NameValueCollection { { GroupConstants.GroupIdQueryParam, group.Id.ToString() } });
        }

        public ActionResult Create()
        {
            var createGroupModel = new GroupCreateModel();
            var mediaSettings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent.ToInt(), true);

            createGroupModel.MediaRootId = mediaSettings.MediaRootId;
            createGroupModel.CreatorId = _userService.GetCurrentUserId();
            createGroupModel.AllowedMediaExtentions = mediaSettings.AllowedMediaExtentions;

            return PartialView(CreateViewPath, createGroupModel);
        }

        [HttpPost]
        public ActionResult Create(GroupCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var group = createModel.Map<Group>();
            group.GroupTypeId = GroupTypeEnum.Open.ToInt();
            var createdMedias = _mediaHelper.CreateMedia(createModel).ToList();
            group.ImageId = createdMedias.Any() ? (int?)createdMedias.First() : null;

            _groupService.Create(group);
            _groupMediaService.GroupTitleChanged(group.Id, group.Title);

            _groupMemberService.Add(group.Id, createModel.CreatorId);

            return RedirectToUmbracoPage(_groupHelper.GetGroupRoomPage(),
                new NameValueCollection { { GroupConstants.GroupIdQueryParam, group.Id.ToString() } });
        }

        public virtual ActionResult Index(bool isMyGroupsPage = false, int page = 1)
        {
            var take = page * ItemsPerPage;
            var currentUser = _userService.GetCurrentUser();
            List<Group> allGroups;

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
                return MapGroupViewModel(g,
                    () => isMyGroupsPage || _groupMemberService.IsGroupMember(g.Id, currentUser.Id));

            })
                .OrderByDescending(g => g.Creator.Id == currentUser.Id)
                .ThenByDescending(s => s.IsMember)
                .ThenBy(g => g.Title);

            return PartialView(ListViewPath, new GroupsListModel()
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
            var currentUser = _userService.GetCurrentUser();
            groupInfo.IsMember = _groupMemberService.IsGroupMember(group.Id, currentUser.Id);
            groupInfo.CanUnsubscribe = group.CreatorId != currentUser.Id;

            groupInfo.Creator = _userService.Get(group.CreatorId);
            groupInfo.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupInfo.CreatorProfileUrl = _profileLinkProvider.GetProfileLink(group.CreatorId);

            if (group.ImageId.HasValue)
            {
                groupInfo.GroupImageUrl = Umbraco.TypedMedia(group.ImageId.Value).Url;
            }
            return PartialView(InfoViewPath, groupInfo);
        }

        [HttpPost]
        public virtual ActionResult Hide(Guid id)
        {
            _groupService.Hide(id);
            return Json(new { _groupHelper.GetOverviewPage().Url });
        }

        [DisabledGroupActionFilter]
        public virtual PartialViewResult GroupSubscribe(Guid groupId)
        {
            var currentUser = _userService.GetCurrentUser();

            var subscribeModel = new GroupSubscribeViewModel
            {
                GroupId = groupId,
                IsMember = _groupMemberService.IsGroupMember(groupId, currentUser.Id),
                UserId = currentUser.Id,
                MembersCount = _groupMemberService.GetMembersCount(groupId)
            };

            return PartialView(SubscribeView, subscribeModel);
        }

        [HttpPost]
        [DisabledGroupActionFilter]
        public virtual RedirectToUmbracoPageResult Subscribe(Guid groupId)
        {
            var currentUser = _userService.GetCurrentUser();
            if (_groupMemberService.IsGroupMember(groupId, currentUser.Id))
            {
                _groupMemberService.Remove(groupId, currentUser.Id);
            }
            else
            {
                _groupMemberService.Add(groupId, currentUser.Id);
            }

            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        [DisabledGroupActionFilter]
        public virtual ActionResult ExcludeMember(Guid groupId, Guid memberId)
        {
            _groupMemberService.Remove(groupId, memberId);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        [DisabledGroupActionFilter]
        public virtual ActionResult GroupMembers(Guid groupId)
        {
            var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
            var membersIdsList = groupMembers.Select(s => s.MemberId).ToList();
            var groupUsers = _userService.GetMany(membersIdsList);
            var group = _groupService.Get(groupId);
            var currentUserId = _userService.GetCurrentUserId();

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

            return PartialView(MembersViewPath, model);
        }

        private GroupViewModel MapGroupViewModel(Group group, Func<bool> fillIsMember)
        {
            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = fillIsMember();
            groupModel.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupModel.Creator = _userService.Get(group.CreatorId);
            groupModel.GroupUrl =
                _groupHelper.GetGroupRoomPage()
                    .Url.UrlWithQueryString(GroupConstants.GroupIdQueryParam, group.Id);
            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = Umbraco.TypedMedia(group.ImageId).Url;
            }

            return groupModel;
        }

        private GroupsOverviewModel GetOverViewModel(bool isMyGroupsPage)
        {
            var createGroupUrl = _groupHelper.GetCreateGroupPage().Url;
            var groupsOverviewModel = new GroupsOverviewModel { IsMyGroupsPage = isMyGroupsPage, CreatePageUrl = createGroupUrl };
            return groupsOverviewModel;
        }
    }
}
