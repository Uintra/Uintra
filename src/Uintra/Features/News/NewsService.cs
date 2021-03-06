﻿using Compent.CommandBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Extensions.Bcl;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using UBaseline.Search.Core;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Indexers;
using Uintra.Core.Search.Indexers.Diagnostics;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Services;
using Uintra.Core.Feed.Settings;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.Search.Queries.DeleteByType;
using Uintra.Core.Search.Repository;
using Uintra.Features.CentralFeed.Enums;
using Uintra.Features.Comments.Services;
using Uintra.Features.Groups.Services;
using Uintra.Features.Likes.Services;
using Uintra.Features.LinkPreview.Services;
using Uintra.Features.Links;
using Uintra.Features.Location.Services;
using Uintra.Features.Media.Enums;
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Intranet.Services.Contracts;
using Uintra.Features.Media.Models;
using Uintra.Features.Media.Video.Commands;
using Uintra.Features.News.Extensions;
using Uintra.Features.Notification;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Services;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Caching;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.TypeProviders;
using static Uintra.Features.Notification.Configuration.NotificationTypeEnum;
using ObjectExtensions = Compent.Extensions.ObjectExtensions;

namespace Uintra.Features.News
{
    public class NewsService : NewsServiceBase<Entities.News>,
        INewsService<Entities.News>,
        IFeedItemService,
        INotifyableService,
        ISearchDocumentIndexer,
        IHandle<VideoConvertedCommand>
    {
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly INotifierDataBuilder _notifierDataBuilder;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IActivityLocationService _activityLocationService;
        private readonly IGroupService _groupService;
        private readonly IUintraSearchRepository<SearchableActivity> _uintraSearchRepository;
        private readonly IIndexContext<SearchableActivity> _indexContext;

        // TODO: delete after test
        private readonly IDependencyProvider dependencyProvider;

        public NewsService(IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder,
            IUserTagService userTagService,
            IActivityLinkService activityLinkService, 
            IUintraSearchRepository<SearchableActivity> uintraSearchRepository, 
            IIndexContext<SearchableActivity> indexContext,
            IDependencyProvider dependencyProvider)
            : base(intranetActivityRepository, cacheService, intranetMemberService,
                activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService,
                permissionsService, groupActivityService, groupService)
        {
            _commentsService = commentsService;
            _likesService = likesService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            _documentIndexer = documentIndexer;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
            _userTagService = userTagService;
            _activityLinkService = activityLinkService;
            _uintraSearchRepository = uintraSearchRepository;
            _indexContext = indexContext;
            this.dependencyProvider = dependencyProvider;
            _activityLocationService = activityLocationService;
        }



        Type ISearchDocumentIndexer.Type => typeof(SearchableActivity);

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

        public IEnumerable<IFeedItem> GetGroupItems(Guid groupId)
        {
	        return GetOrderedActualItems().Where(a => a.GroupId == groupId);
        }

        public async Task<IEnumerable<IFeedItem>> GetItemsAsync()
        {
            var items = await GetOrderedActualItemsAsync();
            return items;
        }

        public async Task<bool> RebuildIndex()
        {
            try
            {
                var activities = GetAll().Where(a => a.IsInCache());
                var searchableActivities = activities.Select(Map).ToList();

                var ensure = await _indexContext.EnsureIndex();

                var query = new DeleteSearchableActivityByTypeQuery
                {
                    Type = UintraSearchableTypeEnum.News
                };
                await _uintraSearchRepository.DeleteByQuery(query, string.Empty);
                await _uintraSearchRepository.IndexAsync(searchableActivities);

                return true;

                // TODO: Extend to return diagnostics?
                //return _indexerDiagnosticService.GetSuccessResult(typeof(NewsService).Name, searchableActivities);
            }
            catch (Exception e)
            {
                return false;

                //return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(NewsService).Name);
            }
        }

        public Task<bool> Delete(IEnumerable<string> nodeIds)
        {
            return Task.FromResult(true);
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
            AsyncHelpers.RunSync(RebuildIndex);
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
            if (news.IsInCache() && (news.GroupId is null || _groupService.IsActivityFromActiveGroup(news)))
            {
                AsyncHelpers.RunSync(() => _uintraSearchRepository.IndexAsync(Map(news)));
                AsyncHelpers.RunSync(() =>_documentIndexer.Index(news.MediaIds));
                return news;
            }

            if (cachedNews == null) return null;

            AsyncHelpers.RunSync(() => _uintraSearchRepository.DeleteAsync(id.ToString()));
            AsyncHelpers.RunSync(() => _documentIndexer.DeleteFromIndex(cachedNews.MediaIds));
            _mediaHelper.DeleteMedia(cachedNews.MediaIds);

            return null;
        }
        
        public override async Task<Entities.News> UpdateActivityCacheAsync(Guid id)
        {
            var cachedNews = await GetAsync(id);
            var news = await base.UpdateActivityCacheAsync(id);
            if (news.IsInCache() && (news.GroupId is null || _groupService.IsActivityFromActiveGroup(news)))
            {
                await _uintraSearchRepository.IndexAsync(Map(news));
                await _documentIndexer.Index(news.MediaIds);
                return news;
            }

            if (cachedNews == null) return null;

            await _uintraSearchRepository.DeleteAsync(id.ToString());
            await _documentIndexer.DeleteFromIndex(cachedNews.MediaIds);
            _mediaHelper.DeleteMedia(cachedNews.MediaIds);
            return null;
        }

        public void Notify(Guid entityId, Enum notificationType)
        {
            NotifierData notifierData;

            if (ObjectExtensions.In(notificationType, CommentAdded, CommentEdited, CommentLikeAdded, CommentReplied))
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

            if (ObjectExtensions.In(notificationType, CommentAdded, CommentEdited, CommentLikeAdded, CommentReplied))
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

        private SearchableActivity Map(Entities.News news)
        {
            var searchableActivity = news.Map<SearchableActivity>();
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