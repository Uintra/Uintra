using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Features.Comments.Converters.Models;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.Converters
{
    public class CommentsPanelViewModelConverter : INodeViewModelConverter<CommentsPanelModel, CommentsPanelViewModel>
    {
        private readonly ICommentsService _commentsService;
        private readonly ICommentsHelper _commentsHelper;

        public CommentsPanelViewModelConverter(
            ICommentsService commentsService,
            ICommentsHelper commentsHelper)
        {
            _commentsService = commentsService;
            _commentsHelper = commentsHelper;
        }

        public void Map(CommentsPanelModel node, CommentsPanelViewModel viewModel)
        {
            var queryId = HttpContext.Current?.Request.GetUbaselineQueryValue("id");

            if (!Guid.TryParse(queryId, out var activityId)) 
                return;

            var comments = _commentsService.GetMany(activityId);

            viewModel.ActivityId = activityId;
            viewModel.Comments = _commentsHelper.GetCommentViews(comments);
            viewModel.ElementId = $"js-comments-overview-{activityId}";
        }
    }
}