
using System.Web.Mvc;
using Compent.uCommunity.Core.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Likes;
using uCommunity.Likes.Web;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;
using uCommunity.Users.Core;

namespace Compent.uCommunity.Controllers
{
    public class LikesController : LikesControllerBase
    {
        public LikesController(IActivitiesServiceFactory activitiesServiceFactory, IIntranetUserService<IntranetUser> intranetUserService, ILikesService likesService)
            : base(activitiesServiceFactory, intranetUserService, likesService)
        {
        }

        public override PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            var like = base.AddLike(model);

            if (model.ActivityId == CommentsTestConstants.ActivityId)
            {
                return like;
            }

            var notifyableService = ActivitiesServiceFactory.GetServiceSafe<INotifyableService>(model.ActivityId);
            if (notifyableService != null)
            {
                if (model.CommentId.HasValue)
                {
                    notifyableService.Notify(model.CommentId.Value, NotificationTypeEnum.CommentLikeAdded);
                }
                else
                {
                    notifyableService.Notify(model.ActivityId, NotificationTypeEnum.ActivityLikeAdded);
                }
            }

            return like;
        }
    }
}