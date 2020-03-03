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

        public UintraNewsCreatePageViewModelConverter(
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            IPermissionsService permissionsService,
            IUserTagProvider tagProvider,
            IFeedLinkService feedLinkService,
            IGroupMemberService groupMemberService)
        {
            _memberService = memberService;
            _permissionsService = permissionsService;
            _tagProvider = tagProvider;
            _newsService = newsService;
            _feedLinkService = feedLinkService;
            _groupMemberService = groupMemberService;
        }

        public void Map(UintraNewsCreatePageModel node, UintraNewsCreatePageViewModel viewModel)
        {
            viewModel.Data = GetData();
        }

        private NewsCreateDataViewModel GetData()
        {
            var model = new NewsCreateDataViewModel();

            var currentMember = _memberService.GetCurrentMember();

            model.CanCreate = _permissionsService.Check(PermissionType, PermissionActionEnum.Create);

            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            if (Guid.TryParse(groupIdStr, out var parsedGroupId))
            {
                model.GroupId = parsedGroupId;
                model.CanCreate = model.CanCreate &&
                                         _groupMemberService.IsGroupMember(parsedGroupId, currentMember.Id);
            }

            if (!model.CanCreate)
            {
                return null;
            }

            model.CanEditOwner = _permissionsService.Check(PermissionType, PermissionActionEnum.EditOwner);
            model.Creator = currentMember.ToViewModel();
            model.PinAllowed = _permissionsService.Check(PermissionType, PermissionActionEnum.CanPin);

            if (model.CanEditOwner)
                model.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionType));

            model.Links = model.GroupId.HasValue ?
                _feedLinkService.GetCreateLinks(ActivityType, model.GroupId.Value)
                : _feedLinkService.GetCreateLinks(ActivityType);
            
            var mediaSettings = _newsService.GetMediaSettings();

            model.PublishDate = DateTime.UtcNow;
            model.Tags = _tagProvider.GetAll();
            model.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;

            return model;
        }

        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();
    }
}