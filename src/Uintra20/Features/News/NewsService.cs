using Compent.CommandBus;
using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.Settings;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Indexers;
using Uintra20.Core.Search.Indexers.Diagnostics;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.LinkPreview.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Intranet.Services.Contracts;
using Uintra20.Features.Media.Models;
using Uintra20.Features.Media.Video.Commands;
using Uintra20.Features.Notification;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;
using static Uintra20.Features.Notification.Configuration.NotificationTypeEnum;

namespace Uintra20.Features.News
{
    public class NewsService : NewsServiceBase<Entities.News>,
        INewsService<Entities.News>,
        IFeedItemService,
        INotifyableService,
        IIndexer,
        IHandle<VideoConvertedCommand>
    {
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly INotifierDataBuilder _notifierDataBuilder;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IActivityLocationService _activityLocationService;
        private readonly IGroupService _groupService;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService;

        public NewsService(IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder,
            IUserTagService userTagService,
            IActivityLinkService activityLinkService, IIndexerDiagnosticService indexerDiagnosticService)
            : base(intranetActivityRepository, cacheService, intranetMemberService,
                activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService,
                permissionsService)
        {
            _commentsService = commentsService;
            _likesService = likesService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
            _userTagService = userTagService;
            _activityLinkService = activityLinkService;
            _indexerDiagnosticService = indexerDiagnosticService;
            _activityLocationService = activityLocationService;
        }

        public override Enum Type => IntranetActivityTypeEnum.News;

        public override Enum PermissionActivityType => PermissionResourceTypeEnum.News;

        public MediaSettings GetMediaSettings()
        {
            return _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.NewsContent);
        }
        public FeedSettings GetFeedSettings() =>
            new FeedSettings
            {
                Type = CentralFeedTypeEnum.News,
                HasSubscribersFilter = false,
                HasPinnedFilter = true,
            };

        public IEnumerable<IFeedItem> GetItems()
        {
            var items = GetOrderedActualItems();
            return items;
        }

        public async Task<IEnumerable<IFeedItem>> GetItemsAsync()
        {
            var items = await GetOrderedActualItemsAsync();
            return items;
        }

        private IOrderedEnumerable<Entities.News> GetOrderedActualItems()
        {
            var items = GetManyActual().OrderByDescending(i => i.PublishDate);
            return items;
        }

        private async Task<IOrderedEnumerable<Entities.News>> GetOrderedActualItemsAsync()
        {
            var items = (await GetManyActualAsync()).OrderByDescending(i => i.PublishDate);
            return items;
        }

        protected override void MapBeforeCache(IList<Entities.News> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.Location = _activityLocationService.Get(entity.Id);
                entity.GroupId = _groupActivityService.GetGroupId(activity.Id);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
            }
        }

        protected override async Task MapBeforeCacheAsync(IList<Entities.News> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.Location = await _activityLocationService.GetAsync(entity.Id);
                entity.GroupId = await _groupActivityService.GetGroupIdAsync(activity.Id);
                await _commentsService.FillCommentsAsync(entity);
                await _likesService.FillLikesAsync(entity);
            }
        }

        protected override void UpdateCache()
        {
            base.UpdateCache();
            FillIndex();
        }

        public override bool IsActual(IIntranetActivity activity)
        {
            var news = (NewsBase)activity;
            var isActual = base.IsActual(news);

            if (!isActual) return false;

            if (IsExpired(news))
            {
                news.IsHidden = true;
                news.UnpublishDate = null;
                Save(news);
                return false;
            }

            return news.PublishDate <= DateTime.UtcNow || IsOwner(news);
        }

        public override Entities.News UpdateActivityCache(Guid id)
        {
            var cachedNews = Get(id);
            var news = base.UpdateActivityCache(id);
            if (IsInCache(news) && (news.GroupId is null || _groupService.IsActivityFromActiveGroup(news)))
            {
                _activityIndex.Index(Map(news));
                _documentIndexer.Index(news.MediaIds);
                return news;
            }

            if (cachedNews == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedNews.MediaIds);
            _mediaHelper.DeleteMedia(cachedNews.MediaIds);
            return null;
        }
        
        public override async Task<Entities.News> UpdateActivityCacheAsync(Guid id)
        {
            var cachedNews = await GetAsync(id);
            var news = await base.UpdateActivityCacheAsync(id);
            if (IsInCache(news) && (news.GroupId is null || _groupService.IsActivityFromActiveGroup(news)))
            {
                _activityIndex.Index(Map(news));
                _documentIndexer.Index(news.MediaIds);
                return news;
            }

            if (cachedNews == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedNews.MediaIds);
            _mediaHelper.DeleteMedia(cachedNews.MediaIds);
            return null;
        }

        public void Notify(Guid entityId, Enum notificationType)
        {
            NotifierData notifierData;

            if (notificationType.In(CommentAdded, CommentEdited, CommentLikeAdded, CommentReplied))
            {
                var comment = _commentsService.Get(entityId);
                var parentActivity = Get(comment.ActivityId);
                notifierData = _notifierDataBuilder.GetNotifierData(comment, parentActivity, notificationType);
            }
            else
            {
                var activity = Get(entityId);
                notifierData = _notifierDataBuilder.GetNotifierData(activity, notificationType);
            }

            _notificationService.ProcessNotification(notifierData);
        }

        public async Task NotifyAsync(Guid entityId, Enum notificationType)
        {
            NotifierData notifierData;

            if (notificationType.In(CommentAdded, CommentEdited, CommentLikeAdded, CommentReplied))
            {
                var comment = await _commentsService.GetAsync(entityId);
                var parentActivity = await GetAsync(comment.ActivityId);
                notifierData = await _notifierDataBuilder.GetNotifierDataAsync(comment, parentActivity, notificationType);
            }
            else
            {
                var activity = await GetAsync(entityId);
                notifierData = await _notifierDataBuilder.GetNotifierDataAsync(activity, notificationType);
            }

            await _notificationService.ProcessNotificationAsync(notifierData);
        }

        public IndexedModelResult FillIndex()
        {
            try
            {
                var activities = GetAll().Where(IsInCache);
                var searchableActivities = activities.Select(Map).ToList();
                _activityIndex.DeleteByType(UintraSearchableTypeEnum.News);
                _activityIndex.Index(searchableActivities);

                return _indexerDiagnosticService.GetSuccessResult(typeof(NewsService).Name, searchableActivities);
            }
            catch (Exception e)
            {
                return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(NewsService).Name);
            }
        }

        private static bool IsInCache(Entities.News news)
        {
            return !IsNewsHidden(news) && IsActualPublishDate(news);
        }

        private static bool IsNewsHidden(IIntranetActivity news) =>
            news == null || news.IsHidden;

        private static bool IsActualPublishDate(INewsBase news) =>
            DateTime.Compare(news.PublishDate, DateTime.UtcNow) <= 0;

        private SearchableUintraActivity Map(Entities.News news)
        {
            var searchableActivity = news.Map<SearchableUintraActivity>();
            searchableActivity.Url = _activityLinkService.GetLinks(news.Id).Details;
            searchableActivity.UserTagNames = _userTagService.Get(news.Id).Select(t => t.Text);
            return searchableActivity;
        }

        public BroadcastResult Handle(VideoConvertedCommand command)
        {
            var entityId = _intranetMediaService.GetEntityIdByMediaId(command.MediaId);
            var entity = Get(entityId);
            if (entity == null)
            {
                return BroadcastResult.Success;
            }

            entity.ModifyDate = DateTime.UtcNow;
            return BroadcastResult.Success;
        }
    }
}