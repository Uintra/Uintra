
using System.Web.Mvc;
using Compent.uCommunity.Core.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Likes;
using uCommunity.Likes.Web;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;

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
            var like = base.AddLike(model);

            if (model.ActivityId == CommentsTestConstants.ActivityId)
            {
                return like;
            }

            var service = ActivitiesServiceFactory.GetServiceSafe<INotifyableService>(model.ActivityId);
            if (service != null)
            {
                var notifyableService = (INotifyableService)service;
                notifyableService.Notify(model.ActivityId, NotificationTypeEnum.LikeAdded);
            }

            return like;
        }
    }
}