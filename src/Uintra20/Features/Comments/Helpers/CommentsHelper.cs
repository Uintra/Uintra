using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes.Models;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.LinkPreview.Models;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.Helpers
{
    public class CommentsHelper : ICommentsHelper
    {
        private readonly ICommentsService _commentsService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly ILikesService _likesService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public CommentsHelper(
            ICommentsService commentsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            ILikesService likesService)
        {
            _commentsService = commentsService;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
            _likesService = likesService;
        }

        public virtual IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<CommentModel> comments)
        {
            comments = comments.OrderBy(c => c.CreatedDate);
            var commentsList = comments as List<CommentModel> ?? comments.ToList();
            var currentMemberId = _intranetMemberService.GetCurrentMemberId();
            var creators = _intranetMemberService.GetAll().ToList();
            var replies = commentsList.FindAll(_commentsService.IsReply);

            foreach (var comment in commentsList.FindAll(c => !_commentsService.IsReply(c)))
            {
                var model = GetCommentView(comment, currentMemberId,
                    creators.SingleOrDefault(c => c.Id == comment.UserId));
                var commentReplies = replies.FindAll(reply => reply.ParentId == model.Id);
                model.Replies = commentReplies.Select(reply =>
                    GetCommentView(reply, currentMemberId, creators.SingleOrDefault(c => c.Id == reply.UserId)));

                yield return model;
            }
        }

        public virtual CommentViewModel GetCommentView(
            CommentModel comment,
            Guid currentMemberId,
            IIntranetMember creator)
        {
            var model = comment.Map<CommentViewModel>();
            model.ModifyDate = _commentsService.WasChanged(comment) ? comment.ModifyDate.ToDateTimeFormat() : null;
            model.CanEdit = _commentsService.CanEdit(comment, currentMemberId);
            model.CanDelete = _commentsService.CanDelete(comment, currentMemberId);
            model.Creator = creator.Map<MemberViewModel>();
            model.ElementOverviewId = $"js-comments-overview-{comment.ActivityId}";
            model.CommentViewId = _commentsService.GetCommentViewId(comment.Id);
            model.CreatorProfileUrl = creator == null ? null : _profileLinkProvider.GetProfileLink(creator);
            model.LinkPreview = comment.LinkPreview.Map<LinkPreviewViewModel>();
            model.LikeModel = GetLikesViewModel(comment.Id);

            return model;
        }

        public virtual LikesViewModel GetLikesViewModel(Guid commentId)
        {
            var likes = _likesService.GetLikeModels(commentId);

            var memberId = _intranetMemberService.GetCurrentMemberId();

            var likeList = likes as IList<LikeModel> ?? likes.ToList();

            var result = new LikesViewModel
            {
                EntityId = commentId,
                MemberId = memberId,
                Count = likeList.Count,
                CanAddLike = likeList.All(el => el.UserId != memberId),
                Users = likeList.Select(el => el.User)
            };

            return result;
        }
    }
}