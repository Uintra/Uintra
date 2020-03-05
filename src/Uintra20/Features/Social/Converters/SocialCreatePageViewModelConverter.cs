using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
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
            if(!HasPermission())
            {
                return;
            }

            viewModel.CanCreate = true;
            viewModel.Data = GetData();
        }

        private SocialCreateDataViewModel GetData()
        {
            var model = new SocialCreateDataViewModel();

            var currentMember = _memberService.GetCurrentMember();

            model.CanEditOwner = _permissionsService.Check(PermissionType, PermissionActionEnum.EditOwner);
            model.PinAllowed = _permissionsService.Check(PermissionType, PermissionActionEnum.CanPin);

            if (model.CanEditOwner)
                model.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionType));

            model.Links = model.GroupId.HasValue ?
                _feedLinkService.GetCreateLinks(ActivityType, model.GroupId.Value)
                : _feedLinkService.GetCreateLinks(ActivityType);

            var mediaSettings = _socialService.GetMediaSettings();

            model.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
            model.Tags = _tagProvider.GetAll();
            model.Creator = currentMember.ToViewModel();
            model.GroupId = GetGroupId();

            model.Title = currentMember.DisplayedName;
            model.Date = DateTime.UtcNow.ToDateFormat();
            
            return model;
        }

        private static Guid? GetGroupId()
        {
            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            return Guid.TryParse(groupIdStr, out var parsedGroupId) ? (Guid?)parsedGroupId : null;
        }

        private bool HasPermission()
        {
            var hasPermission = _permissionsService.Check(PermissionType, PermissionActionEnum.Create);
            var groupId = GetGroupId();

            if (groupId.HasValue)
            {
                hasPermission = hasPermission &&
                                      _groupMemberService.IsGroupMember(groupId.Value, _memberService.GetCurrentMemberId());
            }

            return hasPermission;
        }

        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();
    }
}