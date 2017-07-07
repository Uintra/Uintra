using System.Web.Mvc;
using uIntra.Core.Activity;
using uIntra.Core.Comments;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Likes;
using uIntra.Likes.Web;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Users;

namespace uIntra.Controllers
{
    public class LikesController : LikesControllerBase
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public LikesController(IActivitiesServiceFactory activitiesServiceFactory, 
            IIntranetUserService<IntranetUser> intranetUserService, 
            ILikesService likesService, 
            INotificationTypeProvider notificationTypeProvider)
            : base(activitiesServiceFactory, intranetUserService, likesService)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _notificationTypeProvider = notificationTypeProvider;
        }

        public override PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            var like = base.AddLike(model);

            if (model.ActivityId == CommentsTestConstants.ActivityId)
            {
                return like;
            }

            var notifiableService = _activitiesServiceFactory.GetService<INotifyableService>(model.ActivityId);
            if (notifiableService != null)
            {
                if (model.CommentId.HasValue)
                {
                    var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.CommentLikeAdded.ToInt());
                    notifiableService.Notify(model.CommentId.Value, notificationType);
                }
                else
                {
                    var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.ActivityLikeAdded.ToInt());
                    notifiableService.Notify(model.ActivityId, notificationType);
                }
            }

            return like;
        }
    }
}