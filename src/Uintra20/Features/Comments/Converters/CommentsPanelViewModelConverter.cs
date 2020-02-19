using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Infrastructure.Context;

namespace Uintra20.Features.Comments.Converters
{
    public class CommentsPanelViewModelConverter : INodeViewModelConverter<CommentsPanelModel, CommentsPanelViewModel>
    {
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ICommentsService _commentsService;
        private readonly ICommentsHelper _commentsHelper;

        public CommentsPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            ICommentsService commentsService,
            ICommentsHelper commentsHelper)
        {
            _requestContext = requestContext;
            _commentsService = commentsService;
            _commentsHelper = commentsHelper;
        }

        public void Map(CommentsPanelModel node, CommentsPanelViewModel viewModel)
        {
            var currentNodeKey = _requestContext.Node?.Key;

            if (!currentNodeKey.HasValue)
                return;

            var comments = _commentsService.GetMany(currentNodeKey.Value);

            viewModel.ActivityId = ContextType.ContentPage;
            viewModel.Comments = _commentsHelper.GetCommentViews(comments);
            viewModel.EntityId = currentNodeKey.Value;
        }
    }
}