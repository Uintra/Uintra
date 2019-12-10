using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Likes.Converters.Models;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Likes.Converters
{
    public class LikesPanelViewModelConverter : INodeViewModelConverter<LikesPanelModel, LikesPanelViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ILikesService _likesService;

        public LikesPanelViewModelConverter(IIntranetMemberService<IntranetMember> intranetMemberService, ILikesService likesService)
        {
            _likesService = likesService;
            _intranetMemberService = intranetMemberService;
        }

        public void Map(LikesPanelModel node, LikesPanelViewModel viewModel)
        {
            if (Guid.TryParse(HttpContext.Current?.Request["id"], out Guid pageId))
            {
                var likes = _likesService.GetLikeModels(pageId);

                Guid currentMemberId = _intranetMemberService.GetCurrentMemberId();
                var likeModels = likes as IList<LikeModel> ?? likes.ToList();
                var canAddLike = likeModels.All(el => el.UserId != currentMemberId);

                viewModel.EntityId = pageId;
                viewModel.MemberId = currentMemberId;
                viewModel.Count = likeModels.Count;
                viewModel.CanAddLike = canAddLike;
                viewModel.Users = likeModels.Select(el => el.User);
                viewModel.ShowTitle = true;
            }
        }
    }
}