using Compent.Extensions;
using System.Linq;
using UBaseline.Core.RequestContext;
using Uintra20.Core;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.Converters
{
    public class CommentsPanelViewModelConverter : 
        UintraRestrictedNodeViewModelConverter<CommentsPanelModel, CommentsPanelViewModel>
    {
        private readonly IUBaselineRequestContext _requestContext;
        private readonly ICommentsService _commentsService;
        private readonly ICommentsHelper _commentsHelper;
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUBaselineRequestContext _context;

        public CommentsPanelViewModelConverter(
            IUBaselineRequestContext requestContext,
            ICommentsService commentsService,
            ICommentsHelper commentsHelper,
            IActivityTypeHelper activityTypeHelper,
            IGroupActivityService groupActivityService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext context)
        : base(errorLinksService)
        {
            _requestContext = requestContext;
            _commentsService = commentsService;
            _commentsHelper = commentsHelper;
            _activityTypeHelper = activityTypeHelper;
            _groupActivityService = groupActivityService;
            _intranetMemberService = intranetMemberService;
            _context = context;
        }

        public override ConverterResponseModel MapViewModel(CommentsPanelModel node, CommentsPanelViewModel viewModel)
        {
            var activityId = _context.ParseQueryString("id").TryParseGuid();

            var activityType = activityId.HasValue
                ? _activityTypeHelper.GetActivityType(activityId.Value)
                : IntranetEntityTypeEnum.ContentPage;

            var id = activityType.Equals(IntranetEntityTypeEnum.ContentPage)
                ? _requestContext.Node.Key
                : activityId;

            if (!id.HasValue) return NotFoundResult();

            var groupId = _groupActivityService.GetGroupId(activityId.Value);
            var currentMember = _intranetMemberService.GetCurrentMember();

            viewModel.IsGroupMember = !groupId.HasValue || currentMember.GroupIds.Contains(groupId.Value);

            if (!viewModel.IsGroupMember) return ForbiddenResult();

            var comments = _commentsService.GetMany(id.Value);

            viewModel.Comments = _commentsHelper.GetCommentViews(comments);
            viewModel.ActivityType = activityType;
            viewModel.EntityId = id.Value;
            viewModel.CommentsType = IntranetEntityTypeEnum.Comment;

            return OkResult();
        }
    }
}