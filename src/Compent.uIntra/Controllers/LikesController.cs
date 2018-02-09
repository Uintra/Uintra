using System.Web.Mvc;
using Localization.Umbraco.Attributes;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Likes;
using Uintra.Likes.Web;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
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