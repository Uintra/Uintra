using Uintra.Core.Activity;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Groups.Services;
using Uintra.Features.LinkPreview.Services;
using Uintra.Features.Location.Services;
using Uintra.Features.Media.Intranet.Services.Contracts;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Infrastructure.Caching;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.Social
{
    public abstract class SocialServiceBase<TSocial> : IntranetActivityService<TSocial> where TSocial : SocialBase
    {
        protected SocialServiceBase(
            IIntranetActivityRepository activityRepository,
            ICacheService cache,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IPermissionsService permissionsService,
            IGroupActivityService groupActivityService,
            IGroupService groupService
        )
            : base(
                activityRepository, 
                cache,
                activityTypeProvider,
                intranetMediaService, 
                activityLocationService,
                activityLinkPreviewService,
                intranetMemberService,
                permissionsService,
                groupActivityService,
                groupService)
        {
        }
    }
}