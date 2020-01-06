using Compent.Extensions;
using System;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Converters.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.News;
using Uintra20.Features.News.Entities;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Activity.Converters
{
    public class ActivityCreatePanelViewModelConverter : INodeViewModelConverter<ActivityCreatePanelModel, ActivityCreatePanelViewModel>
    {
        private readonly INewsService<News> _newsService;
        private readonly ISocialService<Features.Bulletins.Entities.Social> _socialService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagService _tagsService;
        private readonly IUserTagProvider _tagProvider;

        public ActivityCreatePanelViewModelConverter(ISocialService<Features.Bulletins.Entities.Social> socialService,
            INewsService<News> newsService,
                                                    IIntranetMemberService<IntranetMember> memberService,
                                                    IActivityTypeProvider activityTypeProvider,
                                                    IPermissionsService permissionsService,
                                                    IUserTagService tagsService,
                                                    IUserTagProvider tagProvider)
        {
            _socialService = socialService;
            _memberService = memberService;
            _activityTypeProvider = activityTypeProvider;
            _permissionsService = permissionsService;
            _tagsService = tagsService;
            _tagProvider = tagProvider;
            _newsService = newsService;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            switch (Enum.Parse(typeof(IntranetActivityTypeEnum), node.TabType))
            {
                case IntranetActivityTypeEnum.Social:
                    {
                        ConvertToBulletins(node, viewModel);
                        break;
                    }
                case IntranetActivityTypeEnum.News:
                    {
                        ConvertToNews(node, viewModel);
                        break;
                    }
            }
            //ConvertToBulletins(node, viewModel);
            viewModel.Tags = GetTagsViewModel();
        }

        private void ConvertToNews(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var mediaSettings = _newsService.GetMediaSettings();
            var currentMember = _memberService.GetCurrentMember();

            viewModel.PublishDate = DateTime.UtcNow;
            viewModel.Creator = currentMember.Map<MemberViewModel>();
            viewModel.ActivityType = _activityTypeProvider[(int)IntranetActivityTypeEnum.News];
            viewModel.Links = null;//TODO: Research links
            viewModel.MediaRootId = null;//mediaSettings.MediaRootId; //TODO: uncomment when media settings service is ready
            viewModel.PinAllowed = _permissionsService.Check(PermissionResourceTypeEnum.News, PermissionActionEnum.CanPin);
        }

        private void ConvertToBulletins(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var currentMember = _memberService.GetCurrentMember();
            var mediaSettings = _socialService.GetMediaSettings();

            viewModel.Title = currentMember.DisplayedName;
            viewModel.ActivityType = _activityTypeProvider[(int)IntranetActivityTypeEnum.Social];
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.Creator = currentMember.Map<MemberViewModel>();
            viewModel.Links = null;//TODO: Research links
            viewModel.AllowedMediaExtensions = null;//mediaSettings.AllowedMediaExtensions; //TODO: uncomment when media settings service is ready
            viewModel.MediaRootId = null;//mediaSettings.MediaRootId; //TODO: uncomment when media settings service is ready
            viewModel.CanCreateBulletin = true; /*_permissionsService.Check(
                PermissionResourceTypeEnum.Bulletins,
                PermissionActionEnum.Create);*/ //TODO: uncomment when permissons service is ready
        }

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