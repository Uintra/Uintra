using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Media;
using UBaseline.Core.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Social.Models;
using Uintra20.Infrastructure.Constants;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Groups.Converters
{
    public class UintraGroupsRoomPageViewModelConverter : INodeViewModelConverter<UintraGroupsRoomPageModel, UintraGroupsRoomPageViewModel>
    {
        private readonly IGroupMemberService _groupMemberService;
        private readonly IPermissionsService _permissionsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IImageHelper _imageHelper;
        private readonly IMediaModelService _mediaModelService;
        private readonly IGroupService _groupService;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INodeModelService _nodeModelService;

        public UintraGroupsRoomPageViewModelConverter(
            IGroupMemberService groupMemberService,
            IPermissionsService permissionsService,
            IIntranetMemberService<IntranetMember> memberService,
            IGroupLinkProvider groupLinkProvider,
            IImageHelper imageHelper,
            IMediaModelService mediaModelService,
            IGroupService groupService,
            IFeedLinkService feedLinkService,
            INodeModelService nodeModelService)
        {
            _groupMemberService = groupMemberService;
            _permissionsService = permissionsService;
            _memberService = memberService;
            _groupLinkProvider = groupLinkProvider;
            _imageHelper = imageHelper;
            _mediaModelService = mediaModelService;
            _groupService = groupService;
            _feedLinkService = feedLinkService;
            _nodeModelService = nodeModelService;
        }

        public void Map(UintraGroupsRoomPageModel node, UintraGroupsRoomPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            if (!Guid.TryParse(idStr, out var id))
                return;

            viewModel.GroupId = id;
            viewModel.GroupInfo = GetGroupInfo(id);

            //TODO: Uncomment when events create will be done
            //viewModel.CreateEventsLink = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.Create) ? 
            //    _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create?.AddGroupId(id)
            //    : null;
            viewModel.CreateNewsLink = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create.AddGroupId(id)
                : null;

            var socialCreateModel = _nodeModelService.AsEnumerable().OfType<SocialCreatePageModel>().First();
            viewModel.SocialCreateModel = _nodeModelService.GetViewModel<SocialCreatePageViewModel>(socialCreateModel);
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