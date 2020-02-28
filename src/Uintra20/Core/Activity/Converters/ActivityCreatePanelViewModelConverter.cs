using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.News;
using Uintra20.Features.News.Entities;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Social;
using Uintra20.Features.Social.Entities;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Activity.Converters
{
    public class ActivityCreatePanelViewModelConverter : INodeViewModelConverter<ActivityCreatePanelModel, ActivityCreatePanelViewModel>
    {
        private readonly ISocialService<Social> _socialService;
        private readonly INewsService<News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagProvider _tagProvider;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IGroupMemberService _groupMemberService;

        public ActivityCreatePanelViewModelConverter(
            INewsService<News> newsService,
            ISocialService<Social> socialService, 
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
            _newsService = newsService;
            _feedLinkService = feedLinkService;
            _groupMemberService = groupMemberService;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var activityType = (IntranetActivityTypeEnum)Enum.Parse(typeof(IntranetActivityTypeEnum), node.TabType);
            var currentMember = _memberService.GetCurrentMember();
            var permissionResourceType = (PermissionResourceTypeEnum)activityType;

            viewModel.CanCreate = _permissionsService.Check(permissionResourceType, PermissionActionEnum.Create);

            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            Guid? groupId = null;
            if (Guid.TryParse(groupIdStr, out var parsedGroupId))
            {
                groupId = parsedGroupId;
                viewModel.CanCreate = viewModel.CanCreate &&
                                         _groupMemberService.IsGroupMember(parsedGroupId, currentMember.Id);
            }

            //TODO: Uncomment when events create will be done
            //viewModel.CreateEventsLink = _permissionsService.Check(PermissionResourceTypeEnum.Events, PermissionActionEnum.Create) ? 
            //    _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create
            //    : null;
            viewModel.CreateNewsLink = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create
                : null;

            if (groupId.HasValue)
            {
                viewModel.GroupId = groupId;

                viewModel.CreateNewsLink = viewModel.CreateNewsLink?.AddGroupId(groupId.Value);
                //viewModel.CreateEventsLink = viewModel.CreateEventsLink?.AddGroupId(groupId);
            }

            viewModel.ActivityType = activityType;

            if (!viewModel.CanCreate)
            {
                return;
            }

            viewModel.CanEditOwner = _permissionsService.Check(permissionResourceType, PermissionActionEnum.EditOwner);
            viewModel.Creator = currentMember.ToViewModel();
            viewModel.PinAllowed = _permissionsService.Check(permissionResourceType, PermissionActionEnum.CanPin);
            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(new PermissionSettingIdentity(PermissionActionEnum.Create, permissionResourceType));
            viewModel.Links = groupId.HasValue ? 
                _feedLinkService.GetCreateLinks(activityType, groupId.Value)
                : _feedLinkService.GetCreateLinks(activityType);

            switch (activityType)
            {
                case IntranetActivityTypeEnum.Social:
                        ConvertToSocials(viewModel);
                        break;

                case IntranetActivityTypeEnum.News:
                        ConvertToNews(viewModel);
                        break;
                    
            }            
            viewModel.Tags = GetTagsViewModel();
        }

        private void ConvertToNews(ActivityCreatePanelViewModel viewModel)
        {
            var mediaSettings = _newsService.GetMediaSettings();

            viewModel.PublishDate = DateTime.UtcNow;
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
        }

        private void ConvertToSocials(ActivityCreatePanelViewModel viewModel)
        {
            var currentMember = _memberService.GetCurrentMember();
            var mediaSettings = _socialService.GetMediaSettings();

            viewModel.Title = currentMember.DisplayedName;
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
        }

        private IEnumerable<IntranetMember> GetUsersWithAccess(PermissionSettingIdentity permissionSettingIdentity) =>
            _memberService
                .GetAll()
                .Where(member => _permissionsService.Check(member, permissionSettingIdentity))
                .OrderBy(user => user.DisplayedName)
                .ToArray();

        private TagsPickerViewModel GetTagsViewModel()
        {
            var pickerViewModel = new TagsPickerViewModel
            {
                UserTagCollection = _tagProvider.GetAll()
            };

            return pickerViewModel;
        }
    }
}