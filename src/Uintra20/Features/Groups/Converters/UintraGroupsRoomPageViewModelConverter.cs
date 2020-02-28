using System;
using System.Web;
using UBaseline.Core.Media;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsRoomPageViewModelConverter : INodeViewModelConverter<UintraGroupsRoomPageModel, UintraGroupsRoomPageViewModel>
    {
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMemberServiceHelper _memberServiceHelper;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IImageHelper _imageHelper;
        private readonly IMediaModelService _mediaModelService;
        private readonly IGroupService _groupService;

        public UintraGroupsRoomPageViewModelConverter(
            IGroupMemberService groupMemberService,
            IMemberServiceHelper memberServiceHelper,
            IIntranetMemberService<IntranetMember> memberService,
            IGroupLinkProvider groupLinkProvider,
            IImageHelper imageHelper,
            IMediaModelService mediaModelService,
            IGroupService groupService)
        {
            _groupMemberService = groupMemberService;
            _memberServiceHelper = memberServiceHelper;
            _memberService = memberService;
            _groupLinkProvider = groupLinkProvider;
            _imageHelper = imageHelper;
            _mediaModelService = mediaModelService;
            _groupService = groupService;
        }

        public void Map(UintraGroupsRoomPageModel node, UintraGroupsRoomPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.GroupId = id;
            viewModel.GroupInfo = GetGroupInfo(id);
        }

        public GroupViewModel GetGroupInfo(Guid groupId)
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
    }
}