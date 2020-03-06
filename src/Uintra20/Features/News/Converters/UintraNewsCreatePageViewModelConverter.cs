using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.News.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Converters
{
    public class UintraNewsCreatePageViewModelConverter : INodeViewModelConverter<UintraNewsCreatePageModel, UintraNewsCreatePageViewModel>
    {
        private const PermissionResourceTypeEnum PermissionType = PermissionResourceTypeEnum.News;
        private const IntranetActivityTypeEnum ActivityType = IntranetActivityTypeEnum.News;

        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagProvider _tagProvider;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IGroupHelper _groupHelper;

        public UintraNewsCreatePageViewModelConverter(
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            IPermissionsService permissionsService,
            IUserTagProvider tagProvider,
            IFeedLinkService feedLinkService,
            IGroupMemberService groupMemberService,
            IGroupHelper groupHelper)
        {
            _memberService = memberService;
            _permissionsService = permissionsService;
            _tagProvider = tagProvider;
            _newsService = newsService;
            _feedLinkService = feedLinkService;
            _groupMemberService = groupMemberService;
            _groupHelper = groupHelper;
        }

        public void Map(UintraNewsCreatePageModel node, UintraNewsCreatePageViewModel viewModel)
        {
            var groupId = GetGroupId();

            if (!HasPermission(groupId))
            {
                return;
            }
            
            viewModel.CanCreate = true;
            viewModel.Data = GetData(groupId);
            viewModel.GroupHeader = groupId.HasValue ? _groupHelper.GetHeader(groupId.Value) : null;
        }

        private NewsCreateDataViewModel GetData(Guid? groupId)
        {
            var model = new NewsCreateDataViewModel();

            var currentMember = _memberService.GetCurrentMember();

            model.CanEditOwner = _permissionsService.Check(PermissionType, PermissionActionEnum.EditOwner);
            model.PinAllowed = _permissionsService.Check(PermissionType, PermissionActionEnum.CanPin);

            if (model.CanEditOwner)
                model.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionType));

            model.Links = model.GroupId.HasValue ?
                _feedLinkService.GetCreateLinks(ActivityType, model.GroupId.Value)
                : _feedLinkService.GetCreateLinks(ActivityType);
            
            var mediaSettings = _newsService.GetMediaSettings();

            model.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
            model.Tags = _tagProvider.GetAll();
            model.Creator = currentMember.ToViewModel();
            model.GroupId = groupId;

            model.PublishDate = DateTime.UtcNow;

            return model;
        }

        private static Guid? GetGroupId()
        {
            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");

            return Guid.TryParse(groupIdStr, out var parsedGroupId) ? (Guid?)parsedGroupId : null;
        }

        private bool HasPermission(Guid? groupId)
        {
            var hasPermission = _permissionsService.Check(PermissionType, PermissionActionEnum.Create);

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