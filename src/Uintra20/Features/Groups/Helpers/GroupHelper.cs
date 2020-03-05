using System;
using System.Linq;
using System.Threading.Tasks;
using UBaseline.Core.Media;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Helpers
{
    public class GroupHelper : IGroupHelper
    {
        private readonly IGroupService _groupService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IImageHelper _imageHelper;
        private readonly IMediaModelService _mediaModelService;
        private readonly ILightboxHelper _lightboxHelper;

        public GroupHelper(
            IGroupService groupService, 
            IGroupLinkProvider groupLinkProvider,
            IGroupMemberService groupMemberService,
            IIntranetMemberService<IntranetMember> memberService,
            IImageHelper imageHelper,
            IMediaModelService mediaModelService,
            ILightboxHelper lightboxHelper)
        {
            _groupService = groupService;
            _groupLinkProvider = groupLinkProvider;
            _groupMemberService = groupMemberService;
            _memberService = memberService;
            _groupLinkProvider = groupLinkProvider;
            _imageHelper = imageHelper;
            _mediaModelService = mediaModelService;
            _groupService = groupService;
            _lightboxHelper = lightboxHelper;
        }

        public GroupHeaderViewModel GetHeader(Guid groupId)
        {
            var group = _groupService.Get(groupId);
            var canEdit = _groupService.CanEdit(group);

            var links = _groupLinkProvider.GetGroupLinks(groupId, canEdit);

            return new GroupHeaderViewModel
            {
                Title = group.Title,
                RoomPageLink = links.GroupRoomPage,
                GroupLinks = links
            };
        }

        public async Task<GroupHeaderViewModel> GetHeaderAsync(Guid groupId)
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

        public GroupViewModel GetGroupViewModel(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            var currentMemberId = _memberService.GetCurrentMemberId();

            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = _groupMemberService.IsGroupMember(group.Id, currentMemberId);
            groupModel.IsCreator = group.CreatorId == currentMemberId;
            groupModel.MembersCount = _groupMemberService.GetMembersCount(group.Id);
            groupModel.Creator = _memberService.Get(group.CreatorId).ToViewModel();
            groupModel.GroupUrl = _groupLinkProvider.GetGroupRoomLink(group.Id);

            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = _imageHelper.GetImageWithPreset(_mediaModelService.Get(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupModel;
        }

        public async Task<GroupViewModel> GetGroupViewModelAsync(Guid groupId)
        {
            var group = await _groupService.GetAsync(groupId);

            var currentMemberId = await _memberService.GetCurrentMemberIdAsync();

            var groupModel = group.Map<GroupViewModel>();
            groupModel.IsMember = await _groupMemberService.IsGroupMemberAsync(group.Id, currentMemberId);
            groupModel.IsCreator = group.CreatorId == currentMemberId;
            groupModel.MembersCount = await _groupMemberService.GetMembersCountAsync(group.Id);
            groupModel.Creator = (await _memberService.GetAsync(group.CreatorId)).ToViewModel();
            groupModel.GroupUrl = _groupLinkProvider.GetGroupRoomLink(group.Id);

            if (groupModel.HasImage)
            {
                groupModel.GroupImageUrl = _imageHelper.GetImageWithPreset(_mediaModelService.Get(group.ImageId.Value).Url, UmbracoAliases.ImagePresets.GroupImageThumbnail);
            }

            return groupModel;
        }

        public GroupInfoViewModel GetInfoViewModel(Guid groupId)
        {
            var group = _groupService.Get(groupId);

            var groupInfo = group.Map<GroupInfoViewModel>();

            groupInfo.CanHide = _groupService.CanHide(group);

            if (group.ImageId.HasValue)
            {
                _lightboxHelper.FillGalleryPreview(groupInfo, Enumerable.Repeat(group.ImageId.Value, 1));
            }

            return groupInfo;
        }

        public async Task<GroupInfoViewModel> GetInfoViewModelAsync(Guid groupId)
        {
            var group = await _groupService.GetAsync(groupId);

            var groupInfo = group.Map<GroupInfoViewModel>();

            groupInfo.CanHide = await _groupService.CanHideAsync(group);

            if (group.ImageId.HasValue)
            {
                _lightboxHelper.FillGalleryPreview(groupInfo, Enumerable.Repeat(group.ImageId.Value, 1));
            }

            return groupInfo;
        }
    }
}