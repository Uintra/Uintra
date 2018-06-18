using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra.Comments;
using Uintra.Comments.Web;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    [TrackContext]
    public class CommentsController : CommentsControllerBase
    {
        protected override string OverviewViewPath { get; } = "~/Views/Comments/CommentsOverView.cshtml";
        protected override string ViewPath { get; } = "~/Views/Comments/CommentsView.cshtml";


        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IProfileLinkProvider profileLinkProvider,
            IContextTypeProvider contextTypeProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory)
            : base(commentsService, intranetUserService, profileLinkProvider, contextTypeProvider, commandPublisher, activitiesServiceFactory)
        {
        }
    }
}