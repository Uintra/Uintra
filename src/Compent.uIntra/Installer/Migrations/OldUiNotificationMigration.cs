using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compent.uIntra.Core.Helpers;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups.Constants;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using umbraco.cms.businesslogic.packager;

namespace Compent.uIntra.Installer.Migrations
{
    public class OldUiNotificationMigration
    {
        private readonly ISqlRepository<Notification> _notificationRepository;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly ICommentsService _commentsService;
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly INotifierTypeProvider _notifierTypeProvider;


        public OldUiNotificationMigration(INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper, ISqlRepository<Notification> notificationRepository, INotifierDataHelper notifierDataHelper, IActivitiesServiceFactory activitiesServiceFactory, INotificationTypeProvider notificationTypeProvider, IIntranetUserService<IIntranetUser> intranetUserService, INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper1, INotificationSettingsService notificationSettingsService, INotifierTypeProvider notifierTypeProvider)
        {
            _notificationRepository = notificationRepository;
            _notifierDataHelper = notifierDataHelper;
            _activitiesServiceFactory = activitiesServiceFactory;
            _notificationTypeProvider = notificationTypeProvider;
            _intranetUserService = intranetUserService;
            _notificationModelMapper = notificationModelMapper1;
            _notificationSettingsService = notificationSettingsService;
            _notifierTypeProvider = notifierTypeProvider;

            _commentsService = GetService<ICommentsService>();
        }

        private T GetService<T>() => DependencyResolver.Current.GetService<T>();
        
        public void Execute()
        {
            var allNotifications = _notificationRepository
                .GetAll()
                .Select(n => (item: n, data: n.Value.Deserialize<OldNotifierData>()));

            var oldNotifications = allNotifications
                .Where(n => IsOldNotifierData(n.data))
                .ToList();

            var updatedNotifications = oldNotifications
                .Select(UpdateNotificationValue)
                .ToList();

           // _notificationRepository.Update(updatedNotifications);
        }

        private Notification UpdateNotificationValue((Notification item, OldNotifierData data) notification)
        {
            var item = notification.item;
            item.Value = MapToNewNotificationValue(notification).ToJson();
            return item;
        }

        private NotificationValue MapToNewNotificationValue((Notification item, OldNotifierData data) notification)
        {
            Guid activityId = ParseActivityId(notification.data.Url);
            
            var activityService =
                _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(notification.data.ActivityType.Id);

            var activity = activityService.Get(activityId);
            if (activity == null) return null;


            var notificationType = _notificationTypeProvider.Get(notification.item.Type);

            INotifierDataValue result;

            switch (notification.item.Type)
            {
                case (int)NotificationTypeEnum.CommentAdded:
                case (int)NotificationTypeEnum.CommentReplied:
                {
                    var commentId = ParseCommentId(notification.data.Url);
                    var comment = _commentsService.Get(commentId);
                    result = _notifierDataHelper.GetCommentNotifierDataModel(activity, comment, notificationType, notification.data.NotifierId);
                    break;
                }
               
                default:
                    result = null;
                    break;
            }

            var eventIdentity = new ActivityEventIdentity(notification.data.ActivityType, notificationType);
            var notificationIdentity = new ActivityEventNotifierIdentity(eventIdentity,
                _notifierTypeProvider.Get(NotifierTypeEnum.UiNotifier.ToInt()));

            var template = _notificationSettingsService.Get<UiNotifierTemplate>(notificationIdentity).Template;

            var receiver = _intranetUserService.Get(notification.item.ReceiverId);
            var message = _notificationModelMapper.Map(result, template, receiver);

            return new NotificationValue
            {
                Message = message.Message,
                Url = notification.data.Url
            };
        }

        private Guid ParseCommentId(string url)
        {
            int guidLength = Guid.Empty.ToString().Length;
            string key = "#js-comment-view-";
            var id = url.Substring(url.IndexOf(key) + key.Length, guidLength);

            return Guid.Parse(id);
        }

        private Guid ParseActivityId(string url)
        {
            int guidLength = Guid.Empty.ToString().Length;
            string key = "id=";
            var id = url.Substring(url.IndexOf(key) + key.Length, guidLength);

            return Guid.Parse(id);
        }

        private bool IsOldNotifierData(OldNotifierData data)
        {
            return data.Title.IsNotNullOrEmpty() && data.ActivityType != null;
        }

        private class OldNotifierData
        {
            public IntranetType ActivityType { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public Guid NotifierId { get; set; }
            public DateTime CreateDate { get; set; }    
            public Guid? CommentId { get; set; }
        }
    }
}
