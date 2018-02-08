using System.Web.Mvc;
using Localization.Umbraco.Attributes;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.User;
using uIntra.Likes;
using uIntra.Likes.Web;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    [ThreadCulture]
    public class LikesController : LikesControllerBase
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public LikesController(
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ILikesService likesService,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            UmbracoHelper umbracoHelper,
            INotificationTypeProvider notificationTypeProvider)
            : base(activitiesServiceFactory, intranetUserService, likesService, documentTypeAliasProvider, umbracoHelper)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _notificationTypeProvider = notificationTypeProvider;
        }

        public override PartialViewResult AddLike(AddRemoveLikeModel model)
        {
            var like = base.AddLike(model);
            if (IsForContentPage(model) || IsForPagePromotion(model))
            {
                return like;
            }

            var notifiableService = _activitiesServiceFactory.GetService<INotifyableService>(model.ActivityId);
            if (notifiableService != null)
            {
                if (IsForComment(model))
                {
                    var notificationType = NotificationTypeEnum.CommentLikeAdded;
                    notifiableService.Notify(model.CommentId.Value, notificationType);
                }
                else
                {
                    var notificationType = NotificationTypeEnum.ActivityLikeAdded;
                    notifiableService.Notify(model.ActivityId, notificationType);
                }
            }

            return like;
        }
    }
}