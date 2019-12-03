using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Member;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Likes.Web;

namespace Uintra20.Controllers
{
    [ThreadCulture]
    [ValidateModel]
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