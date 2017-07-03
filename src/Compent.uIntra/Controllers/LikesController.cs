using System.Web.Mvc;
using Compent.uIntra.Core.Comments;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Likes;
using uIntra.Likes.Web;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class LikesController : LikesControllerBase
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public LikesController(IActivitiesServiceFactory activitiesServiceFactory, IIntranetUserService<IntranetUser> intranetUserService, ILikesService likesService)
            : base(activitiesServiceFactory, intranetUserService, likesService)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public override PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            var like = base.AddLike(model);

            if (model.ActivityId == CommentsTestConstants.ActivityId)
            {
                return like;
            }

            var notifiableService = _activitiesServiceFactory.GetServiceSafe<INotifyableService>(model.ActivityId);
            if (notifiableService != null)
            {
                if (model.CommentId.HasValue)
                {
                    notifiableService.Notify(model.CommentId.Value, NotificationTypeEnum.CommentLikeAdded);
                }
                else
                {
                    notifiableService.Notify(model.ActivityId, NotificationTypeEnum.ActivityLikeAdded);
                }
            }

            return like;
        }
    }
}