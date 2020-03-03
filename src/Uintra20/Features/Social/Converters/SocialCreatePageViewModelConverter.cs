using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Converters
{
    public class SocialCreatePageViewModelConverter : INodeViewModelConverter<SocialCreatePageModel, SocialCreatePageViewModel>
    {
        private const IntranetActivityTypeEnum ActivityType = IntranetActivityTypeEnum.Social;
        private const PermissionResourceTypeEnum PermissionType = PermissionResourceTypeEnum.Social;

        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagProvider _tagProvider;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IGroupMemberService _groupMemberService;

        public SocialCreatePageViewModelConverter(
            ISocialService<Entities.Social> socialService,
            IIntranetMemberService<IntranetMember> memberService,
            IPermissionsService permissionsService,
            IUserTagProvider tagProvider,
            IFeedLinkService feedLinkService,
            IGroupMemberService groupMemberService)
        {
            _socialService = socialService;
            _memberService = memberService;
            _permissionsService = permissionsService;
            _tagProvider = tagProvider;
            _feedLinkService = feedLinkService;
            _groupMemberService = groupMemberService;
        }

        public void Map(SocialCreatePageModel node, SocialCreatePageViewModel viewModel)
        {
            viewModel.Data = GetData();
        }

        private SocialCreateDataViewModel GetData()
        {
            var viewModel = new SocialCreateDataViewModel();

            var currentMember = _memberService.GetCurrentMember();

            viewModel.CanCreate = _permissionsService.Check(PermissionType, PermissionActionEnum.Create);

            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            if (Guid.TryParse(groupIdStr, out var parsedGroupId))
            {
                viewModel.CanCreate = viewModel.CanCreate &&
                                         _groupMemberService.IsGroupMember(parsedGroupId, currentMember.Id);
                viewModel.GroupId = parsedGroupId;
            }

            if (!viewModel.CanCreate)
            {
                return null;
            }

            viewModel.CanEditOwner = _permissionsService.Check(PermissionType, PermissionActionEnum.EditOwner);
            viewModel.PinAllowed = _permissionsService.Check(PermissionType, PermissionActionEnum.CanPin);

            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionType));

            viewModel.Links = viewModel.GroupId.HasValue ?
                _feedLinkService.GetCreateLinks(ActivityType, viewModel.GroupId.Value)
                : _feedLinkService.GetCreateLinks(ActivityType);

            
            var mediaSettings = _socialService.GetMediaSettings();

            viewModel.Title = currentMember.DisplayedName;
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
            viewModel.Tags = _tagProvider.GetAll();
            viewModel.Creator = currentMember.ToViewModel();

            return viewModel;
        }

        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();
    }
}