using Compent.CommandBus;
using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra.Core.Activity;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Services;
using Uintra.Core.Feed.Settings;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Indexers;
using Uintra.Core.Search.Indexers.Diagnostics;
using Uintra.Core.Search.Indexers.Diagnostics.Models;
using Uintra.Core.Search.Indexes;
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

namespace Uintra.Features.Social
{
    public class SocialService<T> : SocialServiceBase<T>,
        ISocialService<T>,
        IFeedItemService,
        INotifyableService,
        IIndexer,
        IHandle<VideoConvertedCommand> 
        where T : Entities.Social
    {
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly INotificationsService _notificationService;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkPreviewService _activityLinkPreviewService;
        private readonly IGroupService _groupService;
        private readonly INotifierDataBuilder _notifierDataBuilder;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService;

        public SocialService(
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IActivityTypeProvider activityTypeProvider,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IMediaHelper mediaHelper,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLocationService activityLocationService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder,
            IUserTagService userTagService,
            IActivityLinkService activityLinkService, IIndexerDiagnosticService indexerDiagnosticService)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService,
                activityLocationService, activityLinkPreviewService, intranetMemberService, permissionsService, groupActivityService, groupService)
        {
            _commentsService = commentsService;
            _likesService = likesService;
            _notificationService = notificationService;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _mediaHelper = mediaHelper;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _activityLinkPreviewService = activityLinkPreviewService;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
            _userTagService = userTagService;
            _activityLinkService = activityLinkService;
            _indexerDiagnosticService = indexerDiagnosticService;
        }

        public override Enum Type => IntranetActivityTypeEnum.Social;

        public override Enum PermissionActivityType => PermissionResourceTypeEnum.Social;

        public MediaSettings GetMediaSettings() => _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.SocialsContent);


        public FeedSettings GetFeedSettings() =>
            new FeedSettings
            {
                Type = CentralFeedTypeEnum.Social,
                HasPinnedFilter = false,
                HasSubscribersFilter = false,
            };

        public IEnumerable<IFeedItem> GetItems() => GetOrderedActualItems();
        public IEnumerable<IFeedItem> GetGroupItems(Guid groupId)
        {
	        return GetOrderedActualItems().Where(a => a.GroupId == groupId);
        }

        public async Task<IEnumerable<IFeedItem>> GetItemsAsync() => await GetOrderedActualItemsAsync();

        private async Task<IOrderedEnumerable<T>> GetOrderedActualItemsAsync() =>
            (await GetManyActualAsync())
            .OrderByDescending(i => i.PublishDate);

        private IOrderedEnumerable<T> GetOrderedActualItems() =>
            GetManyActual().OrderByDescending(i => i.PublishDate);

        protected override void MapBeforeCache(IList<T> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.GroupId = _groupActivityService.GetGroupId(activity.Id);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
                FillLinkPreview(entity);
            }
        }

        protected override async Task MapBeforeCacheAsync(IList<T> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.GroupId = await _groupActivityService.GetGroupIdAsync(activity.Id);
                await _commentsService.FillCommentsAsync(entity);
                await _likesService.FillLikesAsync(entity);
                await FillLinkPreviewAsync(entity);
            }
        }

        public override async Task<T> UpdateActivityCacheAsync(Guid id)
        {
            var cachedBulletin = await GetAsync(id);
            var bulletin = await base.UpdateActivityCacheAsync(id);
            if (IsCacheable(bulletin) && (bulletin.GroupId is null || _groupService.IsActivityFromActiveGroup(bulletin)))
            {
                _activityIndex.Index(Map(bulletin));
                _documentIndexer.Index(bulletin.MediaIds);
                return bulletin;
            }

            if (cachedBulletin == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedBulletin.MediaIds);
            _mediaHelper.DeleteMedia(cachedBulletin.MediaIds);
            return null;
        }
        public override T UpdateActivityCache(Guid id)
        {
            var cachedBulletin = Get(id);
            var bulletin = base.UpdateActivityCache(id);
            if (IsCacheable(bulletin) && (bulletin.GroupId is null || _groupService.IsActivityFromActiveGroup(bulletin)))
            {
                _activityIndex.Index(Map(bulletin));
                _documentIndexer.Index(bulletin.MediaIds);
                return bulletin;
            }

            if (cachedBulletin == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedBulletin.MediaIds);
            _mediaHelper.DeleteMedia(cachedBulletin.MediaIds);
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
                var activities = GetAll().Where(IsCacheable);
                var searchableActivities = activities.Select(Map).ToList();
                _activityIndex.DeleteByType(UintraSearchableTypeEnum.Socials);
                _activityIndex.Index(searchableActivities);

                return _indexerDiagnosticService.GetSuccessResult(typeof(SocialService<T>).Name, searchableActivities);
            }
            catch (Exception e)
            {
                return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(SocialService<T>).Name);
            }
        }

        private void FillLinkPreview(Entities.Social social)
        {
            var linkPreview = _activityLinkPreviewService.GetActivityLinkPreview(social.Id);
            social.LinkPreview = linkPreview;
            social.LinkPreviewId = linkPreview?.Id;
        }

        private async Task FillLinkPreviewAsync(Entities.Social social)
        {
            var linkPreview = await _activityLinkPreviewService.GetActivityLinkPreviewAsync(social.Id);
            social.LinkPreview = linkPreview;
            social.LinkPreviewId = linkPreview?.Id;
        }

        private static bool IsBulletinHidden(Entities.Social social) => social == null || social.IsHidden;

        private bool IsCacheable(Entities.Social social) =>
            !IsBulletinHidden(social) && IsActualPublishDate(social);

        private static bool IsActualPublishDate(Entities.Social social) =>
            DateTime.Compare(social.PublishDate, DateTime.UtcNow) <= 0;

        private SearchableUintraActivity Map(Entities.Social social)
        {
            var searchableActivity = social.Map<SearchableUintraActivity>();
            searchableActivity.Url = _activityLinkService.GetLinks(social.Id).Details;
            searchableActivity.UserTagNames = _userTagService.Get(social.Id).Select(t => t.Text);
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