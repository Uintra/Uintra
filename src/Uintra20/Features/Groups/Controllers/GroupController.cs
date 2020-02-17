using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Compent.CommandBus;
using UBaseline.Core.Controllers;
using UBaseline.Core.Media;
using UBaseline.Core.Node;
using Uintra20.Attributes;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.CommandBus.Commands;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Controllers
{
    [ValidateModel]
    public class GroupController : UBaselineApiController
    {
        private const int ItemsPerPage = 10;

        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IGroupMediaService _groupMediaService;
        private readonly IImageHelper _imageHelper;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IMediaModelService _mediaModelService;
        private readonly INodeModelService _nodeModelService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IPermissionsService _permissionsService;

        public GroupController(
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupMediaService groupMediaService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IImageHelper imageHelper,
            ICommandPublisher commandPublisher,
            IMediaModelService mediaModelService,
            INodeModelService nodeModelService,
            IGroupLinkProvider groupLinkProvider,
            IPermissionsService permissionsService)
        {
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _mediaHelper = mediaHelper;
            _memberService = intranetMemberService;
            _groupMediaService = groupMediaService;
            _imageHelper = imageHelper;
            _commandPublisher = commandPublisher;
            _mediaModelService = mediaModelService;
            _nodeModelService = nodeModelService;
            _groupLinkProvider = groupLinkProvider;
            _permissionsService = permissionsService;
        }

        [HttpGet]
        public GroupLeftNavigationMenuViewModel LeftNavigation()
        {
            var rootGroupPage = _nodeModelService.AsEnumerable().OfType<UintraGroupsPageModel>().First();

            var groupPageChildren = _nodeModelService.AsEnumerable().Where(x =>
                x is IGroupNavigationComposition navigation && navigation.GroupNavigation.ShowInMenu &&
                x.ParentId == rootGroupPage.Id);

            groupPageChildren = groupPageChildren.Where(x =>
            {
                if (x is UintraGroupsCreatePageModel)
                {
                    return _permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Create,
                        PermissionResourceTypeEnum.Groups));
                }

                return true;
            });

            var menuItems = groupPageChildren.OrderBy(x => ((IGroupNavigationComposition)x).GroupNavigation.SortOrder.Value).Select(x => new GroupLeftNavigationItemViewModel
            {
                Title = ((IGroupNavigationComposition)x).GroupNavigation.NavigationTitle,
                Link = x.Url.ToLinkModel()
            });

            var result = new GroupLeftNavigationMenuViewModel
            {
                Items = menuItems,
                GroupPageItem = new GroupLeftNavigationItemViewModel
                {
                    Link = rootGroupPage.Url.ToLinkModel(),
                    Title = ((IGroupNavigationComposition)rootGroupPage).GroupNavigation.NavigationTitle
                }
            };

            return result;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Edit(GroupEditModel model)
        {
            var group = _groupService.Get(model.Id);

            if (group == null || group.IsHidden)
            {
                return NotFound();
            }

            if (!_permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Edit,
                PermissionResourceTypeEnum.Groups)))
            {
                return Ok(_groupLinkProvider.GetGroupRoomLink(model.Id));
            }

            group = Mapper.Map(model, group);
            group.ImageId = model.Media?.Split(',').First().ToNullableInt();
            var createdMedias = _mediaHelper.CreateMedia(model, MediaFolderTypeEnum.GroupsContent).ToList();
            if (createdMedias.Any())
            {
                group.ImageId = createdMedias.First();
            }
            await _groupService.EditAsync(group);
            await _groupMediaService.GroupTitleChangedAsync(group.Id, group.Title);

            return Ok(_groupLinkProvider.GetGroupRoomLink(group.Id));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(GroupCreateModel createModel)
        {
            if (!_permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Create,
                PermissionResourceTypeEnum.Groups)))
            {
                return Ok(_groupLinkProvider.GetGroupsOverviewLink());
            }

            var currentMemberId = await _memberService.GetCurrentMemberIdAsync();

            var groupId = await _groupMemberService.CreateAsync(createModel, new GroupMemberSubscriptionModel
            {
                IsAdmin = true,
                MemberId = currentMemberId
            });

            return Ok(_groupLinkProvider.GetGroupRoomLink(groupId));
        }

        [HttpGet]
        public virtual async Task<IEnumerable<GroupViewModel>> List(bool isMyGroupsPage = false, int page = 1)
        {
            var take = page * ItemsPerPage;
            var skip = (page - 1) * ItemsPerPage;
            var currentMember = await _memberService.GetCurrentMemberAsync();

            var allGroups = await (isMyGroupsPage
                ? _groupService.GetManyAsync(currentMember.GroupIds)
                : _groupService.GetAllNotHiddenAsync());

            async Task<bool> IsCurrentMemberInGroupAsync(GroupModel g) => isMyGroupsPage || await _groupMemberService.IsGroupMemberAsync(g.Id, currentMember.Id);

            var groups = (await allGroups
                .SelectAsync(g => MapGroupViewModelAsync(g, IsCurrentMemberInGroupAsync(g))))
                .OrderByDescending(g => g.Creator.Id == currentMember.Id)
                .ThenByDescending(s => s.IsMember)
                .ThenBy(g => g.Title)
                .Skip(skip)
                .Take(take);

            return groups;
        }

        [HttpGet]
        public async Task<GroupHeaderViewModel> Header(Guid groupId)
        {
            var group = await _groupService.GetAsync(groupId);
            var canEdit = await _groupService.CanEditAsync(group);

            var links = _groupLinkProvider.GetGroupLinks(groupId, canEdit);

            return new GroupHeaderViewModel
            {
                Title = group.Title,
                RoomPageLink = links.GroupRoomPage,
                GroupLinks = links
            };
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Hide(Guid id)
        {
            var group = await _groupService.GetAsync(id);

            if (group == null || group.IsHidden)
            {
                return NotFound();
            }

            var canHide = await _groupService.CanHideAsync(id);

            if (!canHide) return Ok(_groupLinkProvider.GetGroupRoomLink(id));

            var command = new HideGroupCommand(id);
            _commandPublisher.Publish(command);

            return Ok(_groupLinkProvider.GetGroupsOverviewLink());

        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Subscribe(Guid groupId)
        {
            var currentMember = await _memberService.GetCurrentMemberAsync();

            var group = await _groupService.GetAsync(groupId);

            if (group == null || group.IsHidden)
            {
                return NotFound();
            }

            if (await _groupMemberService.IsGroupMemberAsync(groupId, currentMember.Id))
            {
                await _groupMemberService.RemoveAsync(groupId, currentMember.Id);
            }
            else
            {
                var subscription = new GroupMemberSubscriptionModel
                {
                    MemberId = currentMember.Id,
                    IsAdmin = false
                };

                await _groupMemberService.AddAsync(groupId, subscription);
            }

            return Ok(_groupLinkProvider.GetGroupRoomLink(groupId));
        }

        private async Task<GroupViewModel> MapGroupViewModelAsync(GroupModel group, Task<bool> isCurrentUserMember)
        {
            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = await isCurrentUserMember;
            groupModel.MembersCount = await _groupMemberService.GetMembersCountAsync(group.Id);
            groupModel.Creator = _memberService.Get(group.CreatorId).Map<MemberViewModel>();
            groupModel.GroupUrl = _groupLinkProvider.GetGroupRoomLink(group.Id);
            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = _imageHelper.GetImageWithPreset(_mediaModelService.Get(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupModel;
        }
    }
}