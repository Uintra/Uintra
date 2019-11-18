using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Likes;
using Uintra20.Core.Likes.Web;
using Uintra20.Core.User;

namespace Uintra20.Controllers
{
    [ThreadCulture]
    public class LikesController : LikesControllerBase
    {
        public LikesController(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            ILikesService likesService,
            ICommandPublisher commandPublisher)
            : base(activitiesServiceFactory, intranetMemberService, likesService, commandPublisher)
        {
        }
    }
}