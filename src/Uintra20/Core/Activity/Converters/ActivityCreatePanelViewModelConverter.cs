using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
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

        public ActivityCreatePanelViewModelConverter(
            INewsService<News> newsService,
            ISocialService<Social> socialService, 
            IIntranetMemberService<IntranetMember> memberService, 
            IPermissionsService permissionsService,
            IUserTagProvider tagProvider,
            IFeedLinkService feedLinkService)
        {
            _socialService = socialService;
            _memberService = memberService;
            _permissionsService = permissionsService;
            _tagProvider = tagProvider;
            _newsService = newsService;
            _feedLinkService = feedLinkService;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var activityType = (IntranetActivityTypeEnum)Enum.Parse(typeof(IntranetActivityTypeEnum), node.TabType);
            var currentMember = _memberService.GetCurrentMember();

            viewModel.ActivityType = activityType;
            viewModel.Creator = currentMember.ToViewModel();
            viewModel.PinAllowed = _permissionsService.Check(viewModel.ActivityType, PermissionActionEnum.CanPin);
            viewModel.CanCreate = _permissionsService.Check(viewModel.ActivityType, PermissionActionEnum.Create);
            viewModel.CanEditOwner = _permissionsService.Check(viewModel.ActivityType, PermissionActionEnum.EditOwner);
            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(new PermissionSettingIdentity(viewModel.ActivityType, viewModel.ActivityType));

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

            //TODO: Uncomment when events create will be done
            //viewModel.CreateEventsLink = _permissionsService.Check(IntranetActivityTypeEnum.Events, PermissionActionEnum.Create) ? 
            //    _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create
            //    : null;
            viewModel.CreateNewsLink = _permissionsService.Check(IntranetActivityTypeEnum.News, PermissionActionEnum.Create) ?
                _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create
                : null;

            var groupIdStr = HttpContext.Current.Request.GetRequestQueryValue("groupId");
            if (!Guid.TryParse(groupIdStr, out var groupId))
                return;

            viewModel.GroupId = groupId;

            viewModel.CreateNewsLink = viewModel.CreateNewsLink?.AddGroupId(groupId);
            //viewModel.CreateEventsLink = viewModel.CreateEventsLink?.AddGroupId(groupId);
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