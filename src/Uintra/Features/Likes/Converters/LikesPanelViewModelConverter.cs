using Compent.Extensions;
using System.Linq;
using UBaseline.Core.RequestContext;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Helpers;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups.Services;
using Uintra.Features.Likes.Models;
using Uintra.Features.Likes.Services;
using Uintra.Features.Links;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Likes.Converters
{
    public class LikesPanelViewModelConverter : 
        UintraRestrictedNodeViewModelConverter<LikesPanelModel, LikesPanelViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ILikesService _likesService;
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IUBaselineRequestContext _context;
        public LikesPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            IIntranetMemberService<IntranetMember> intranetMemberService, 
            ILikesService likesService,
            IActivityTypeHelper activityTypeHelper,
            IGroupActivityService groupActivityService,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context)
        : base(errorLinksService)
        {
            _requestContext = requestContext;
            _likesService = likesService;
            _intranetMemberService = intranetMemberService;
            _activityTypeHelper = activityTypeHelper;
            _groupActivityService = groupActivityService;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(LikesPanelModel node, LikesPanelViewModel viewModel)
        {
            var activityId = _context.ParseQueryString("id").TryParseGuid();

            var activityType = activityId.HasValue
                ? _activityTypeHelper.GetActivityType(activityId.Value)
                : IntranetActivityTypeEnum.ContentPage;

            var id = activityType.Equals(IntranetActivityTypeEnum.ContentPage)
                ? _requestContext.Node?.Key
                : activityId;

            if (!id.HasValue) return NotFoundResult();

            var groupId = _groupActivityService.GetGroupId(id.Value);
            
            var currentMember = _intranetMemberService.GetCurrentMember();

            viewModel.IsGroupMember = !groupId.HasValue || currentMember.GroupIds.Contains(groupId.Value);

            if (!viewModel.IsGroupMember) return ForbiddenResult();

            var likes = _likesService.GetLikeModels(id.Value).ToArray();

            viewModel.Likes = likes;
            viewModel.EntityId = id.Value;
            viewModel.LikedByCurrentUser = likes.Any(el => el.UserId == currentMember.Id);
            viewModel.ActivityType = activityType;

            return OkResult();
        }
    }
}