using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Likes.Converters.Models;
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

        public LikesPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            IIntranetMemberService<IntranetMember> intranetMemberService, 
            ILikesService likesService)
        {
            _requestContext = requestContext;
            _likesService = likesService;
            _intranetMemberService = intranetMemberService;
        }

        public void Map(LikesPanelModel node, LikesPanelViewModel viewModel)
        {
            var currentNodeKey = _requestContext.Node?.Key;

            if (!currentNodeKey.HasValue) 
                return;

            var likes = _likesService.GetLikeModels(currentNodeKey.Value).ToArray();

            var currentMemberId = _intranetMemberService.GetCurrentMemberId();

            viewModel.Likes = likes;
            viewModel.EntityId = currentNodeKey.Value;
            viewModel.LikedByCurrentUser = likes.Any(el => el.UserId == currentMemberId);
            viewModel.ShowTitle = true;
            viewModel.ActivityType = ContextType.ContentPage.ToString();
        }
    }
}