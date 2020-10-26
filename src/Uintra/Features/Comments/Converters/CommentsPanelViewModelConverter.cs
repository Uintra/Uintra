using Compent.Extensions;
using System.Linq;
using UBaseline.Core.RequestContext;
using Uintra.Core;
using Uintra.Core.Activity.Helpers;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Comments.Helpers;
using Uintra.Features.Comments.Models;
using Uintra.Features.Comments.Services;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Comments.Converters
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
                ? _requestContext.Node?.Key
                : activityId;

            if (!id.HasValue) return NotFoundResult();

            var groupId = _groupActivityService.GetGroupId(id.Value);
            
            var currentMember = _intranetMemberService.GetCurrentMember();

            viewModel.IsGroupMember = !groupId.HasValue || currentMember.GroupIds.Contains(groupId.Value);

            if (!viewModel.IsGroupMember) return ForbiddenResult();

            var comments = _commentsService.GetMany(id.Value);

            viewModel.Comments = _commentsHelper.GetCommentViews(comments);
            viewModel.EntityId = id.Value;
            viewModel.CommentsType = activityType;

            return OkResult();
        }
    }
}