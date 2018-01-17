using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups.Permissions;
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
        protected virtual string LeftNavigationPath => "~/App_Plugins/Groups/GroupLeftNavigation.cshtml";

        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetUserService<IGroupMember> _userService;
        private readonly IGroupMediaService _groupMediaService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IImageHelper _imageHelper;

        protected int ItemsPerPage = 10;

        protected GroupControllerBase(
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupMediaService groupMediaService,
            IIntranetUserService<IGroupMember> userService,
            IProfileLinkProvider profileLinkProvider,
            IGroupLinkProvider groupLinkProvider,
            IImageHelper imageHelper)
        {
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _mediaHelper = mediaHelper;
            _groupMediaService = groupMediaService;
            _userService = userService;
            _profileLinkProvider = profileLinkProvider;
            _groupLinkProvider = groupLinkProvider;
            _imageHelper = imageHelper;
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
                Response.Redirect(_groupLinkProvider.GetGroupLink(groupId));
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
            GroupEditModel model = GetEditModel(groupId);
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
            return Redirect(_groupLinkProvider.GetGroupLink(group.Id));
        }

        [GroupRestrictedAction(IntranetActivityActionEnum.Create)]
        public ActionResult Create()
        {
            var createGroupModel = new GroupCreateModel();
            var mediaSettings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent.ToInt(), true);

            createGroupModel.MediaRootId = mediaSettings.MediaRootId;
            createGroupModel.CreatorId = _userService.GetCurrentUserId();
            createGroupModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;

            return PartialView(CreateViewPath, createGroupModel);
        }

        [HttpPost]
        public ActionResult Create(GroupCreateModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToCurrentUmbracoPage(Request.QueryString);
            }

            var group = createModel.Map<GroupModel>();
            group.GroupTypeId = GroupTypeEnum.Open.ToInt();
            var createdMedias = _mediaHelper.CreateMedia(createModel).ToList();
            group.ImageId = createdMedias.Any() ? (int?)createdMedias.First() : null;

            Guid groupId = _groupService.Create(group);
            _groupMediaService.GroupTitleChanged(groupId, group.Title);

            _groupMemberService.Add(groupId, createModel.CreatorId);

            return Redirect(_groupLinkProvider.GetGroupLink(groupId));
        }

        public abstract ActionResult LeftNavigation();

        public virtual ActionResult Index(bool isMyGroupsPage = false, int page = 1)
        {
            GroupsListModel model = GetListModel(isMyGroupsPage, page);
            return PartialView(ListViewPath, model);
        }

        [DisabledGroupActionFilter]
        public ActionResult Info(Guid groupId)
        {
            GroupInfoViewModel groupInfo = GetInfoViewModel(groupId);
            return PartialView(InfoViewPath, groupInfo);
        }

        protected virtual GroupInfoViewModel GetInfoViewModel(Guid groupId)
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
                groupInfo.GroupImageUrl = _imageHelper.GetImageWithPreset(Umbraco.TypedMedia(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupInfo;
        }

        [HttpPost]
        public virtual ActionResult Hide(Guid id)
        {
            _groupService.Hide(id);
            return Json(_groupLinkProvider.GetGroupsOverviewLink());
        }

        [DisabledGroupActionFilter]
        public virtual PartialViewResult GroupSubscribe(Guid groupId)
        {
            GroupSubscribeViewModel subscribeModel = GetSubscribeViewModel(groupId);
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
            var model = GetGroupMembersViewModel(groupId);
            return PartialView(MembersViewPath, model);
        }

        private GroupSubscribeViewModel GetSubscribeViewModel(Guid groupId)
        {
            var currentUser = _userService.GetCurrentUser();

            var subscribeModel = new GroupSubscribeViewModel
            {
                GroupId = groupId,
                IsMember = _groupMemberService.IsGroupMember(groupId, currentUser.Id),
                UserId = currentUser.Id,
                MembersCount = _groupMemberService.GetMembersCount(groupId)
            };
            return subscribeModel;
        }

        private GroupsListModel GetListModel(bool isMyGroupsPage, int page)
        {
            var take = page * ItemsPerPage;
            var currentUser = _userService.GetCurrentUser();

            var allGroups = isMyGroupsPage
                ? _groupService.GetMany(currentUser.GroupIds).ToList()
                : _groupService.GetAllNotHidden().ToList();

            bool IsCurrentUserMember(GroupModel g) => isMyGroupsPage || _groupMemberService.IsGroupMember(g.Id, currentUser.Id);

            var groups = allGroups
                .Select(g => MapGroupViewModel(g, IsCurrentUserMember(g)))
                .OrderByDescending(g => g.Creator.Id == currentUser.Id)
                .ThenByDescending(s => s.IsMember)
                .ThenBy(g => g.Title);

            var model = new GroupsListModel
            {
                Groups = groups.Take(take),
                BlockScrolling = allGroups.Count <= take
            };
            return model;
        }

        private GroupMemberOverviewViewModel GetGroupMembersViewModel(Guid groupId)
        {
            var groupMembers = _groupMemberService.GetGroupMemberByGroup(groupId);
            var membersIdsList = groupMembers.Select(s => s.MemberId).ToList();
            var groupUsers = _userService.GetMany(membersIdsList);
            var group = _groupService.Get(groupId);
            var currentUserId = _userService.GetCurrentUserId();

            var groupMembersViewModel = groupUsers
                .Select(m => MapToMemberViewModel(m, group, currentUserId))
                .OrderByDescending(s => s.IsGroupAdmin)
                .ThenBy(s => s.GroupMember.DisplayedName)
                .ToList();

            var model = new GroupMemberOverviewViewModel
            {
                Members = groupMembersViewModel,
                CanExcludeFromGroup = IsGroupCreator(currentUserId, group)
            };
            return model;
        }

        protected virtual GroupEditModel GetEditModel(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            if (!_groupService.CanEdit(group, _userService.GetCurrentUser()))
            {
                HttpContext.Response.Redirect(_groupLinkProvider.GetGroupLink(groupId));
            }

            var model = group.Map<GroupEditModel>();
            var mediaSettings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.GroupsContent.ToInt());
            model.MediaRootId = mediaSettings.MediaRootId;
            model.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
            return model;
        }

        protected virtual GroupsOverviewModel GetOverViewModel(bool isMyGroupsPage)
        {
            var groupsOverviewModel = new GroupsOverviewModel { IsMyGroupsPage = isMyGroupsPage };
            return groupsOverviewModel;
        }

        private static GroupMemberViewModel MapToMemberViewModel(IGroupMember m, GroupModel groupModel, Guid currentUserId)
        {
            var viewModel = m.Map<GroupMemberViewModel>();
            viewModel.IsGroupAdmin = IsGroupCreator(m.Id, groupModel);
            viewModel.CanUnsubscribe = viewModel.GroupMember.Id == currentUserId && currentUserId != groupModel.CreatorId;
            return viewModel;
        }

        private static bool IsGroupCreator(Guid userId, GroupModel groupModel)
        {
            return userId == groupModel.CreatorId;
        }

        private GroupViewModel MapGroupViewModel(GroupModel group, bool isCurrentUserMember)
        {
            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = isCurrentUserMember;
            groupModel.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupModel.Creator = _userService.Get(group.CreatorId);
            groupModel.GroupUrl = _groupLinkProvider.GetGroupLink(group.Id);
            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = _imageHelper.GetImageWithPreset(Umbraco.TypedMedia(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupModel;
        }
    }
}
