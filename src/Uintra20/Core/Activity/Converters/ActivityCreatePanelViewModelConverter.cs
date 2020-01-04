using Compent.Extensions;
using System;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Converters.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Activity.Converters
{
    public class ActivityCreatePanelViewModelConverter : INodeViewModelConverter<ActivityCreatePanelModel, ActivityCreatePanelViewModel>
    {
        private readonly ISocialsService<Features.Bulletins.Entities.Social> _socialsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IPermissionsService _permissionsService;
        private readonly IUserTagService _tagsService;
        private readonly IUserTagProvider _tagProvider;

        public ActivityCreatePanelViewModelConverter(ISocialsService<Features.Bulletins.Entities.Social> socialsService, 
                                                    IIntranetMemberService<IntranetMember> memberService, 
                                                    IActivityTypeProvider activityTypeProvider,
                                                    IPermissionsService permissionsService,
                                                    IUserTagService tagsService,
                                                    IUserTagProvider tagProvider)
        {
            _socialsService = socialsService;
            _memberService = memberService;
            _activityTypeProvider = activityTypeProvider;
            _permissionsService = permissionsService;
            _tagsService = tagsService;
            _tagProvider = tagProvider;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            ConvertToBulletins(node, viewModel);
            viewModel.Tags = GetTagsViewModel();
        }

        private void ConvertToBulletins(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var currentMember = _memberService.GetCurrentMember();
            var mediaSettings = _socialsService.GetMediaSettings();

            viewModel.Title = currentMember?.DisplayedName;
            viewModel.ActivityType = _activityTypeProvider[(int)IntranetActivityTypeEnum.Socials];
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.Creator = currentMember?.Map<MemberViewModel>();
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