using System;
using System.Web;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.Converters
{
    public class CommentsPanelViewModelConverter : INodeViewModelConverter<CommentsPanelModel, CommentsPanelViewModel>
    {
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ICommentsService _commentsService;
        private readonly ICommentsHelper _commentsHelper;
        private readonly IActivityTypeHelper _activityTypeHelper;

        public CommentsPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            ICommentsService commentsService,
            ICommentsHelper commentsHelper,
            IActivityTypeHelper activityTypeHelper)
        {
            _requestContext = requestContext;
            _commentsService = commentsService;
            _commentsHelper = commentsHelper;
            _activityTypeHelper = activityTypeHelper;
        }

        public void Map(CommentsPanelModel node, CommentsPanelViewModel viewModel)
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

            var comments = _commentsService.GetMany(id.Value);

            viewModel.ActivityType = activityType;
            viewModel.Comments = _commentsHelper.GetCommentViews(comments);
            viewModel.EntityId = id.Value;
        }
    }
}