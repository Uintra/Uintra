using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra20.Features.Activity;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Likes.Web;
using Uintra20.Features.User;

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