using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Likes;
using uCommunity.Likes.Web;

namespace Compent.uCommunity.Controllers
{
    public class LikesController : LikesControllerBase
    {
        public LikesController(IActivitiesServiceFactory activitiesServiceFactory, IIntranetUserService intranetUserService, ILikesService likesService)
            : base(activitiesServiceFactory, intranetUserService, likesService)
        {


        }

        public override PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            

            return base.AddLike(model);
        }
    }
}