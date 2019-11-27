using System;
using Compent.Extensions;
using Compent.Shared.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Converters.Models;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Activity.Converters
{
    public class ActivityCreatePanelViewModelConverter : INodeViewModelConverter<ActivityCreatePanelModel, ActivityCreatePanelViewModel>
    {
        private readonly IBulletinsService<Features.Bulletins.Entities.Bulletin> _bulletinsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IPermissionsService _permissionsService;

        public ActivityCreatePanelViewModelConverter(IBulletinsService<Features.Bulletins.Entities.Bulletin> bulletinsService, 
                                                    IIntranetMemberService<IntranetMember> memberService, 
                                                    IActivityTypeProvider activityTypeProvider,
                                                    IPermissionsService permissionsService)
        {
            _bulletinsService = bulletinsService;
            _memberService = memberService;
            _activityTypeProvider = activityTypeProvider;
            _permissionsService = permissionsService;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            ConvertToBulletins(node, viewModel);
        }

        private void ConvertToBulletins(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var currentMember = _memberService.GetCurrentMember();
            var mediaSettings = _bulletinsService.GetMediaSettings();

            viewModel.Title = currentMember.DisplayedName;
            viewModel.ActivityType = _activityTypeProvider[(int)IntranetActivityTypeEnum.Bulletins];
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.Creator = currentMember.Map<MemberViewModel>();//TODO: uncomment when member service is ready
            viewModel.Links = null;//TODO: Research links
            viewModel.AllowedMediaExtensions = null;//mediaSettings.AllowedMediaExtensions; //TODO: uncomment when media settings service is ready
            viewModel.MediaRootId = null;//mediaSettings.MediaRootId; //TODO: uncomment when media settings service is ready
            viewModel.CanCreateBulletin = true; /*_permissionsService.Check(
                PermissionResourceTypeEnum.Bulletins,
                PermissionActionEnum.Create);*/ //TODO: uncomment when permissons service is ready
        }
    }
}