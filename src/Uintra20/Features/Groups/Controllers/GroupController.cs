using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Compent.CommandBus;
using UBaseline.Core.Controllers;
using UBaseline.Core.Media;
using Uintra20.Attributes;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.CommandBus.Commands;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Image.Helpers.Contracts;
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
        private readonly IGroupLinkProvider _groupLinkProvider;

        public GroupController(
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IMediaHelper mediaHelper,
            IGroupMediaService groupMediaService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IImageHelper imageHelper,
            ICommandPublisher commandPublisher,
            IMediaModelService mediaModelService,
            IGroupLinkProvider groupLinkProvider)
        {
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _mediaHelper = mediaHelper;
            _memberService = intranetMemberService;
            _groupMediaService = groupMediaService;
            _imageHelper = imageHelper;
            _commandPublisher = commandPublisher;
            _mediaModelService = mediaModelService;
            _groupLinkProvider = groupLinkProvider;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Edit(GroupEditModel model)
        {
            var group = _groupService.Get(model.Id);

            if (group == null || group.IsHidden)
            {
                return NotFound();
            }

            if (!_groupService.CanEdit(group))
            {
                return StatusCode(HttpStatusCode.Forbidden);
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
            if (!_groupService.CanCreate())
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var currentMemberId = await _memberService.GetCurrentMemberIdAsync();

            var groupId = await _groupMemberService.CreateAsync(createModel, new GroupMemberSubscriptionModel
            {
                IsAdmin = true,
                MemberId = currentMemberId,
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


            var groups = allGroups
                .Select(g => MapGroupViewModel(g, currentMember.Id))
                .OrderByDescending(g => g.Creator.Id == currentMember.Id)
                .ThenByDescending(s => s.IsMember)
                .ThenBy(g => g.Title)
                .Skip(skip)
                .Take(take);

            return groups;
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

        private GroupViewModel MapGroupViewModel(GroupModel group,Guid currentMemberId)
        {
            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = _groupMemberService.IsGroupMember(groupModel.Id, currentMemberId);
            groupModel.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupModel.Creator = _memberService.Get(group.CreatorId).ToViewModel();
            groupModel.GroupUrl = _groupLinkProvider.GetGroupRoomLink(group.Id);
            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = _imageHelper.GetImageWithPreset(_mediaModelService.Get(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupModel;
        }
    }
}