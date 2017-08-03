using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Events;
using uIntra.Likes;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Search;
using uIntra.Subscribe;
using uIntra.Users;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace Compent.uIntra.Core.Events
{
    public class EventsService : IntranetActivityService<Event>,
        IEventsService<Event>,
        ICentralFeedItemService,
        ISubscribableService,
        ILikeableService,
        ICommentableService,
        INotifyableService,
        IReminderableService<Event>,
        IIndexer
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IElasticActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IActivityTypeProvider _activityTypeProvider;

        private readonly ICentralFeedTypeProvider _centralFeedTypeProvider;
        private readonly ISearchableTypeProvider _searchableTypeProvider;

        public EventsService(UmbracoHelper umbracoHelper,
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService<IntranetUser> intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IElasticActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
			IActivityTypeProvider activityTypeProvider, 
            ICentralFeedTypeProvider centralFeedTypeProvider,
            ISearchableTypeProvider searchableTypeProvider,
            IIntranetMediaService intranetMediaService)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService)
        {
            _umbracoHelper = umbracoHelper;
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _subscribeService = subscribeService;
            _permissionsService = permissionsService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _activityTypeProvider = activityTypeProvider;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _searchableTypeProvider = searchableTypeProvider;
        }

        protected List<string> OverviewXPath => new List<string> { HomePage.ModelTypeAlias, EventsOverviewPage.ModelTypeAlias };
        
        public override IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.Events.ToInt());

        public override IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(GetPath()));
        }

        public override IPublishedContent GetDetailsPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(GetPath(EventsDetailsPage.ModelTypeAlias)));
        }

        public override IPublishedContent GetCreatePage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(GetPath(EventsCreatePage.ModelTypeAlias)));
        }

        public override IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(GetPath(EventsEditPage.ModelTypeAlias)));
        }

        public IEnumerable<Event> GetPastEvents()
        {
            return GetAll().Where(@event => !IsActual(@event) && !@event.IsHidden);
        }

        public IEnumerable<Event> GetComingEvents(DateTime fromDate)
        {
            var events = GetAll()
                .Where(e => e.PublishDate > fromDate)
                .OrderBy(e => e.PublishDate);
            return events;
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
            return _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.EventsContent.ToInt());
        }

        public CentralFeedSettings GetCentralFeedSettings()
        {
            return new CentralFeedSettings
            {
                Type = _centralFeedTypeProvider.Get(CentralFeedTypeEnum.Events.ToInt()),
                Controller = "Events",
                OverviewPage = GetOverviewPage(),
                CreatePage = GetCreatePage(),
                HasSubscribersFilter = true,
                HasPinnedFilter = true
            };
        }

        public override bool CanEdit(IIntranetActivity cached)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var isWebmater = _permissionsService.IsUserWebmaster(currentUser);
            if (isWebmater)
            {
                return true;
            }

            var creatorId = Get(cached.Id).CreatorId;
            var isCreator = creatorId == currentUser.Id;

            var isUserHasPermissions = _permissionsService.IsRoleHasPermissions(currentUser.Role, ActivityType, IntranetActivityActionEnum.Edit);
            return isCreator && isUserHasPermissions;
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

        protected override void MapBeforeCache(IList<IIntranetActivity> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity as Event;
                _subscribeService.FillSubscribers(entity);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
            }
        }

        protected override Event UpdateCachedEntity(Guid id)
        {
            var cachedEvent = Get(id);
            var @event = base.UpdateCachedEntity(id);
            if (IsEventHidden(@event))
            {
                _activityIndex.Delete(id);
                _documentIndexer.DeleteFromIndex(cachedEvent.MediaIds);
                _mediaHelper.DeleteMedia(cachedEvent.MediaIds);
                return null;
            }

            _activityIndex.Index(Map(@event));
            _documentIndexer.Index(@event.MediaIds);
            return @event;
        }

        public void UnSubscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Unsubscribe(userId, activityId);
            UpdateCachedEntity(activityId);
        }

        public void UpdateNotification(Guid id, bool value)
        {
            var subscribe = _subscribeService.UpdateNotification(id, value);
            UpdateCachedEntity(subscribe.ActivityId);
        }

        public ILikeable AddLike(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public ILikeable RemoveLike(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public IEnumerable<LikeModel> GetLikes(Guid activityId)
        {
            return Get(activityId).Likes;
        }

        public Comment CreateComment(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var comment = _commentsService.Create(userId, activityId, text, parentId);
            UpdateCachedEntity(activityId);
            return comment;
        }

        public void UpdateComment(Guid id, string text)
        {
            var comment = _commentsService.Update(id, text);
            UpdateCachedEntity(comment.ActivityId);
        }

        public void DeleteComment(Guid id)
        {
            var comment = _commentsService.Get(id);
            _commentsService.Delete(id);
            UpdateCachedEntity(comment.ActivityId);
        }

        public ICommentable GetCommentsInfo(Guid activityId)
        {
            return Get(activityId);
        }

        public bool CanEditSubscribe(Guid activityId)
        {
            return !Get(activityId).Subscribers.Any();
        }

        public void Notify(Guid entityId, IIntranetType notificationType)
        {
            var notifierData = GetNotifierData(entityId, notificationType);

            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
        }

        private NotifierData GetNotifierData(Guid entityId, IIntranetType notificationType)
        {
            Event currentEvent;
            var currentUser = _intranetUserService.GetCurrentUser();

            var data = new NotifierData
            {
                NotificationType = notificationType
            };

            switch (notificationType.Id)
            {
                case (int) NotificationTypeEnum.CommentReplied:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = ActivityType,
                            NotifierId = currentUser.Id,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id),
                            CommentId = comment.Id
                        };
                    }
                    break;
                case (int) NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = currentEvent.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = ActivityType,
                            NotifierId = comment.UserId,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id)
                        };
                        break;
                    }
                case (int) NotificationTypeEnum.CommentAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = GetNotifiedSubscribers(currentEvent).Concat(currentEvent.CreatorId.ToEnumerableOfOne()).Distinct();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = ActivityType,
                            NotifierId = comment.UserId,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id)
                        };
                    }
                    break;
                case (int) NotificationTypeEnum.ActivityLikeAdded:
                    {
                        currentEvent = Get(entityId);
                        data.ReceiverIds = currentEvent.CreatorId.ToEnumerableOfOne();
                        data.Value = new LikesNotifierDataModel
                        {
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", currentEvent.Id),
                            Title = currentEvent.Title,
                            ActivityType = ActivityType,
                            NotifierId = currentUser.Id,
                            CreatedDate = DateTime.Now
                        };
                    }
                    break;
                case (int) NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = currentEvent.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            CommentId = entityId,
                            ActivityType = ActivityType,
                            NotifierId = currentUser.Id,
                            Title = currentEvent.Title,
                            Url = GetUrlWithComment(currentEvent.Id, comment.Id)
                        };
                    }
                    break;

                case (int) NotificationTypeEnum.BeforeStart:
                    {
                        currentEvent = Get(entityId);
                        data.ReceiverIds = GetNotifiedSubscribers(currentEvent);
                        data.Value = new ActivityReminderDataModel
                        {
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", currentEvent.Id),
                            Title = currentEvent.Title,
                            ActivityType = ActivityType,
                            StartDate = currentEvent.StartDate
                        };
                    }
                    break;

                case (int) NotificationTypeEnum.EventHided:
                case (int) NotificationTypeEnum.EventUpdated:
                    {
                        currentEvent = Get(entityId);
                        data.ReceiverIds = GetNotifiedSubscribers(currentEvent);
                        data.Value = new ActivityNotifierDataModel
                        {
                            ActivityType = ActivityType,
                            Title = currentEvent.Title,
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", currentEvent.Id),
                            NotifierId = currentUser.Id
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

        private static IEnumerable<Guid> GetNotifiedSubscribers(Event currentEvent)
        {
            return currentEvent.Subscribers.Where(s => !s.IsNotificationDisabled).Select(s => s.UserId);
        }

        public ISubscribable Subscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Subscribe(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public override IPublishedContent GetOverviewPage(IPublishedContent currentPage)
        {
            return GetOverviewPage();
        }

        public override IPublishedContent GetDetailsPage(IPublishedContent currentPage)
        {
            return GetDetailsPage();
        }

        public override IPublishedContent GetCreatePage(IPublishedContent currentPage)
        {
            return GetCreatePage();
        }

        public override IPublishedContent GetEditPage(IPublishedContent currentPage)
        {
            return GetEditPage();
        }

        public Event GetActual(Guid id)
        {
            var @event = Get(id);
            return !@event.IsHidden ? @event : null;
        }

        public void FillIndex()
        {
            var activities = GetAll().Where(s => !IsEventHidden(s));
            var searchableActivities = activities.Select(Map);

            var searchableType = _searchableTypeProvider.Get(SearchableTypeEnum.Events.ToInt());
            _activityIndex.DeleteByType(searchableType);
            _activityIndex.Index(searchableActivities);
        }

        private string[] GetPath(params string[] aliases)
        {
            var basePath = OverviewXPath;

            if (aliases.Any())
            {
                basePath.AddRange(aliases.ToList());
            }
            return basePath.ToArray();
        }

        private bool IsEventHidden(Event @event)
        {
            return @event == null || @event.IsHidden;
        }

        private SearchableActivity Map(Event @event)
        {
            var searchableActivity = @event.Map<SearchableActivity>();
            searchableActivity.Url = GetDetailsPage().Url.AddIdParameter(@event.Id);
            return searchableActivity;
        }
    }
}