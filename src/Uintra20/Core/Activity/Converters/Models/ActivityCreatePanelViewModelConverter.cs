using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Compent.Shared.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Models;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Core.Activity.Converters.Models
{
    public class ActivityCreatePanelViewModelConverter : INodeViewModelConverter<ActivityCreatePanelModel, ActivityCreatePanelViewModel>
    {
        private readonly IBulletinsService<BulletinBase> _bulletinsService;
        private readonly IIntranetMemberService<IIntranetMember> _memberService;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public ActivityCreatePanelViewModelConverter(IBulletinsService<BulletinBase> bulletinsService, 
                                                    IIntranetMemberService<IIntranetMember> memberService, 
                                                    IActivityTypeProvider activityTypeProvider)
        {
            _bulletinsService = bulletinsService;
            _memberService = memberService;
            _activityTypeProvider = activityTypeProvider;
        }

        public void Map(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            //ConvertToBulletins(node, viewModel);
        }

        private void ConvertToBulletins(ActivityCreatePanelModel node, ActivityCreatePanelViewModel viewModel)
        {
            var currentMember = _memberService.GetCurrentMember();
            var mediaSettings = _bulletinsService.GetMediaSettings();
            
            viewModel.Title = currentMember.DisplayedName;
            viewModel.ActivityType = _activityTypeProvider[(int)IntranetActivityTypeEnum.Bulletins];
            viewModel.Dates = DateTime.UtcNow.ToDateFormat().ToEnumerable();
            viewModel.Creator = currentMember.Map<MemberViewModel>();
            viewModel.Links = null;//TODO: Research links
            viewModel.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
            viewModel.MediaRootId = mediaSettings.MediaRootId;
        }
    }
}