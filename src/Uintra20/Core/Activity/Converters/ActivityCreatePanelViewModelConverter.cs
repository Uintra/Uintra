using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Models;
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
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Activity.Converters
{
    public class ActivityCreatePanelViewModelConverter : INodeViewModelConverter<ActivityCreatePanelModel, ActivityCreatePanelViewModel>
    {
        private readonly ISocialService<Social> _socialService;
        private readonly INewsService<News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagService _tagsService;
        private readonly IUserTagProvider _tagProvider;
        private readonly IMemberServiceHelper _memberHelper;
        private readonly IFeedLinkService _feedLinkService;

        public ActivityCreatePanelViewModelConverter(
            INewsService<News> newsService,
            ISocialService<Social> socialService, 
            IIntranetMemberService<IntranetMember> memberService, 
            IActivityTypeProvider activityTypeProvider,
            IPermissionsService permissionsService,
            IUserTagService tagsService,
            IUserTagProvider tagProvider,
            IMemberServiceHelper memberHelper, 
            IFeedLinkService feedLinkService)
        {
            _socialService = socialService;
            _memberService = memberService;
            _activityTypeProvider = activityTypeProvider;
            _permissionsService = permissionsService;
            _tagsService = tagsService;
            _tagProvider = tagProvider;
            _newsService = newsService;
            _memberHelper = memberHelper;
            _feedLinkService = feedLinkService;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            switch (Enum.Parse(typeof(IntranetActivityTypeEnum), node.TabType))
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
            //viewModel.CreateEventsLink = _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.Events).Create.OriginalUrl;
            viewModel.CreateNewsLink = _feedLinkService.GetCreateLinks(IntranetActivityTypeEnum.News).Create.OriginalUrl;
        }

        private void ConvertToNews(ActivityCreatePanelViewModel viewModel)
        {
            var mediaSettings = _newsService.GetMediaSettings();
            var currentMember = _memberService.GetCurrentMember();

            viewModel.PublishDate = DateTime.UtcNow;
            viewModel.Creator = _memberHelper.ToViewModel(currentMember);
            viewModel.ActivityType = IntranetActivityTypeEnum.News;
            viewModel.Links = null;//TODO: Research links
            viewModel.MediaRootId = null;//mediaSettings.MediaRootId; //TODO: uncomment when media settings service is ready
            viewModel.PinAllowed = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.CanPin);

            viewModel.CanEditOwner = _permissionsService.Check(viewModel.ActivityType, PermissionActionEnum.EditOwner);

            if (viewModel.CanEditOwner)
                viewModel.Members = GetUsersWithAccess(PermissionSettingIdentity.Of(PermissionActionEnum.Create, viewModel.ActivityType));
        }

        private void ConvertToSocials(ActivityCreatePanelViewModel viewModel)
        {
            var cookies = HttpContext.Current.Request.Cookies;
            var currentMember = _memberService.GetCurrentMember();
            var mediaSettings = _socialService.GetMediaSettings();

            viewModel.Title = currentMember.DisplayedName;
            viewModel.ActivityType = IntranetActivityTypeEnum.Social;
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.Creator = _memberHelper.ToViewModel(currentMember);
            viewModel.Links = null;//TODO: Research links
            viewModel.AllowedMediaExtensions = null;//mediaSettings.AllowedMediaExtensions; //TODO: uncomment when media settings service is ready
            viewModel.MediaRootId = null;//mediaSettings.MediaRootId; //TODO: uncomment when media settings service is ready
            viewModel.CanCreateBulletin = true; /*_permissionsService.Check(
                PermissionResourceTypeEnum.Bulletins,
                PermissionActionEnum.Create);*/ //TODO: uncomment when permissons service is ready
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