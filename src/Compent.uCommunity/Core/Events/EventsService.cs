using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Entities;
using uCommunity.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using uCommunity.Events;
using uCommunity.Likes;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Entities;
using uCommunity.Notification.Core.Services;
using uCommunity.Subscribe;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uCommunity.Core.Events
{
    public class EventsService : IntranetActivityItemServiceBase<EventBase, Event>,
        IEventsService<EventBase, Event>,
        ICentralFeedItemService,
        ISubscribableService,
        ILikeableService,
        ICommentableService,
        INotifyableService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;

        public EventsService(UmbracoHelper umbracoHelper,
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService, 
            IPermissionsService permissionsService,
            INotificationsService notificationService
            )
            : base(intranetActivityRepository, cacheService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _subscribeService = subscribeService;
            _permissionsService = permissionsService;
            _notificationService = notificationService;
        }

        protected override List<string> OverviewXPath { get; }
        public override IntranetActivityTypeEnum ActivityType => IntranetActivityTypeEnum.Events;

        public override IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContent(1163);
        }

        public override IPublishedContent GetDetailsPage()
        {
            return _umbracoHelper.TypedContent(1165);
        }

        public override IPublishedContent GetCreatePage()
        {
            return _umbracoHelper.TypedContent(1164);
        }

        public override IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContent(1166);
        }

        public IEnumerable<Event> GetPastEvents()
        {
            return GetAll().Where(@event => !IsActual(@event) && !@event.IsHidden);
        }

        public void Hide(Guid id)
        {
            var @event = Get(id);
            @event.IsHidden = true;
            Save(@event);
        }

        public bool CanEditSubscribe(Event activity)
        {
            return !activity.Subscribers.Any();
        }

        public bool CanSubscribe(EventBase activity)
        {
            return IsActual(activity) && activity.CanSubscribe;
        }

        public bool HasSubscribers(Event activity)
        {
            return activity.Subscribers.Any();
        }

        public MediaSettings GetMediaSettings()
        {
            return new MediaSettings
            {
                MediaRootId = 1099
            };
        }

        public CentralFeedSettings GetCentralFeedSettings()
        {
            return new CentralFeedSettings
            {
                Type = ActivityType,
                Controller = "Events",
                OverviewPage = GetOverviewPage(),
                CreatePage = GetCreatePage()
            };
        }

        public override bool CanEdit(EventBase activity)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            return activity.CreatorId == currentUser.Id 
                || _permissionsService.IsRoleHasPermissions(currentUser.Role, IntranetActivityTypeEnum.Events, IntranetActivityActionEnum.Edit);
        }

        public ICentralFeedItem GetItem(Guid activityId)
        {
            var item = Get(activityId);
            return item;
        }

        public IEnumerable<ICentralFeedItem> GetItems()
        {
            var items = GetManyActual().OrderByDescending(i => i.PublishDate);
            return items;
        }

        protected override Event FillPropertiesOnGet(IntranetActivityEntity entity)
        {
            var activity = base.FillPropertiesOnGet(entity);
            _subscribeService.FillSubscribers(activity);
            _intranetUserService.FillCreator(activity);
            _commentsService.FillComments(activity);
            _likesService.FillLikes(activity);

            return activity;
        }

        public void UnSubscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Unsubscribe(userId, activityId);
            FillCache(activityId);
        }

        public void UpdateNotification(Guid id, bool value)
        {
            var subscribe = _subscribeService.UpdateNotification(id, value);
            FillCache(subscribe.ActivityId);
        }

        public ILikeable Add(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            return FillCache(activityId);
        }

        public ILikeable Remove(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            return FillCache(activityId);
        }

        public IEnumerable<LikeModel> GetLikes(Guid activityId)
        {
            return Get(activityId).Likes;
        }

        public Comment CreateComment(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var comment = _commentsService.Create(userId, activityId, text, parentId);
            FillCache(activityId);
            return comment;
        }

        public void UpdateComment(Guid id, string text)
        {
            var comment = _commentsService.Update(id, text);
            FillCache(comment.ActivityId);
        }

        public void DeleteComment(Guid id)
        {
            var comment = _commentsService.Get(id);
            _commentsService.Delete(id);
            FillCache(comment.ActivityId);
        }

        public ICommentable GetCommentsInfo(Guid activityId)
        {
            return Get(activityId);
        }

        public bool CanEditSubscribe(Guid activityId)
        {
            return !Get(activityId).Subscribers.Any();
        }

        public void Notify(Guid entityId, NotificationTypeEnum notificationType)
        {
            var notifierData = GetNotifierData(entityId, notificationType);

            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
        }

        private NotifierData GetNotifierData(Guid entityId, NotificationTypeEnum notificationType)
        {
            Event currentEvent;
            
            var data = new NotifierData
            {
                NotificationType = notificationType,
                
            };

            switch (notificationType)
            {
                case NotificationTypeEnum.CommentReplyed:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = IntranetActivityTypeEnum.Events,
                            NotifierId = _intranetUserService.GetCurrentUser().Id,
                            NotifierName = _intranetUserService.GetCurrentUser().DisplayedName,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id),
                            CommentId = comment.Id
                        };
                    }
                    break;
                case NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = currentEvent.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = IntranetActivityTypeEnum.Events,
                            NotifierId = comment.UserId,
                            NotifierName = _intranetUserService.Get(comment.UserId).DisplayedName,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id)
                        };
                        break;
                    }
                case NotificationTypeEnum.CommentAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = GetNotifiedSubsribers(currentEvent).Concat(currentEvent.CreatorId.ToEnumerableOfOne()).Distinct();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = IntranetActivityTypeEnum.Events,
                            NotifierId = comment.UserId,
                            NotifierName = _intranetUserService.Get(comment.UserId).DisplayedName,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id)
                        };
                    }
                    break;
                case NotificationTypeEnum.LikeAdded:
                    {
                        currentEvent = Get(entityId);
                        data.ReceiverIds = currentEvent.CreatorId.ToEnumerableOfOne();
                        data.Value = new LikesNotifierDataModel
                        {
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", currentEvent.Id),
                            Title = currentEvent?.Title,
                            ActivityType = IntranetActivityTypeEnum.Events,
                            NotifierId = _intranetUserService.GetCurrentUser().Id,
                            NotifierName = _intranetUserService.GetCurrentUser().DisplayedName
                        };
                    }
                    break;

                case NotificationTypeEnum.BeforeStart:
                    {
                        currentEvent = Get(entityId);
                        data.ReceiverIds = GetNotifiedSubsribers(currentEvent);
                        data.Value = new ActivityReminderDataModel
                        {
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", currentEvent.Id),
                            Title = currentEvent.Title,
                            ActivityType = IntranetActivityTypeEnum.Events,
                            StartDate = currentEvent.StartDate
                        };
                    }
                    break;

                case NotificationTypeEnum.EventHided:
                case NotificationTypeEnum.EventUpdated:
                    {
                        currentEvent = Get(entityId);
                        data.ReceiverIds = GetNotifiedSubsribers(currentEvent);
                        data.Value = new ActivityNotifierDataModel
                        {
                            ActivityType = currentEvent.Type,
                            Title = currentEvent.Title,
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", currentEvent.Id),
                            NotifierId = _intranetUserService.GetCurrentUser().Id,
                            NotifierName = _intranetUserService.GetCurrentUser().DisplayedName
                        };

                        break;
                    }
                default:
                   return null;
            }
            return data;
        }

        private string GetUrlWithComment(Guid eventId, Guid commentId)
        {
            return $"{GetDetailsPage().Url.UrlWithQueryString("id", eventId)}#{_commentsService.GetCommentViewId(commentId)}";
        }

        private static IEnumerable<Guid> GetNotifiedSubsribers(Event currentEvent)
        {
            return currentEvent.Subscribers.Where(s => !s.IsNotificationDisabled).Select(s => s.UserId);
        }

        public ISubscribable Subscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Subscribe(userId, activityId);
            return FillCache(activityId);
        }

        public override IPublishedContent GetOverviewPage(IPublishedContent currentPage)
        {
            throw new NotImplementedException();
        }

        public override IPublishedContent GetDetailsPage(IPublishedContent currentPage)
        {
            throw new NotImplementedException();
        }

        public override IPublishedContent GetCreatePage(IPublishedContent currentPage)
        {
            throw new NotImplementedException();
        }

        public override IPublishedContent GetEditPage(IPublishedContent currentPage)
        {
            throw new NotImplementedException();
        }
    }
}