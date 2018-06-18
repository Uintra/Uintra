using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.User;
using Uintra.Likes;
using Uintra.Likes.Web;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    [TrackContext]
    public class LikesController : LikesControllerBase
    {
        public LikesController(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ILikesService likesService,
            IContextTypeProvider contextTypeProvider,
            ICommandPublisher commandPublisher)
            : base(activitiesServiceFactory, intranetUserService, likesService, contextTypeProvider, commandPublisher)
        {
        }
    }
}