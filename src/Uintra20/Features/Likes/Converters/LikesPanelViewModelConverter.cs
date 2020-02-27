using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Likes.Converters
{
    public class LikesPanelViewModelConverter : INodeViewModelConverter<LikesPanelModel, LikesPanelViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ILikesService _likesService;
        private readonly IActivityTypeHelper _activityTypeHelper;
        
        public LikesPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            IIntranetMemberService<IntranetMember> intranetMemberService, 
            ILikesService likesService,
            IActivityTypeHelper activityTypeHelper)
        {
            _requestContext = requestContext;
            _likesService = likesService;
            _intranetMemberService = intranetMemberService;
            _activityTypeHelper = activityTypeHelper;
        }

        public void Map(LikesPanelModel node, LikesPanelViewModel viewModel)
        {
            var activityIdStr = HttpContext.Current.Request.GetRequestQueryValue("id");

            Enum activityType = Guid.TryParse(activityIdStr, out Guid activityId)
                ? _activityTypeHelper.GetActivityType(activityId)
                : IntranetActivityTypeEnum.ContentPage;

            Guid? id = activityType.Equals(IntranetActivityTypeEnum.ContentPage)
                ? _requestContext.Node?.Key
                : activityId;

            if (!id.HasValue)
                return;

            var likes = _likesService.GetLikeModels(id.Value).ToArray();

            var currentMemberId = _intranetMemberService.GetCurrentMemberId();

            viewModel.Likes = likes;
            viewModel.EntityId = id.Value;
            viewModel.LikedByCurrentUser = likes.Any(el => el.UserId == currentMemberId);
            viewModel.ActivityType = activityType;
        }
    }
}