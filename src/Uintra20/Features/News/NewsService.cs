using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Compent.CommandBus;
using Compent.Extensions;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.Settings;
using Uintra20.Core.Localization;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.LinkPreview;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Location.Services;
using Uintra20.Features.Media;
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
        //IIndexer,
        IHandle<VideoConvertedCommand>
    {
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        //private readonly IElasticUintraActivityIndex _activityIndex;
        //private readonly IDocumentIndexer _documentIndexer;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _linkService;
        private readonly INotifierDataBuilder _notifierDataBuilder;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLocationService _activityLocationService;
        private readonly IGroupService _groupService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly IMemberServiceHelper _memberHelper;

        public NewsService(IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            //IElasticUintraActivityIndex activityIndex,
            //IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            IActivityLocationService activityLocationService,
            IUserTagService userTagService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder,
            IIntranetLocalizationService localizationService,
            IMemberServiceHelper memberHelper)
            : base(intranetActivityRepository, cacheService, intranetMemberService,
                activityTypeProvider, intranetMediaService, activityLocationService, activityLinkPreviewService,
                permissionsService)
        {
            _commentsService = commentsService;
            _likesService = likesService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            //_activityIndex = activityIndex;
            //_documentIndexer = documentIndexer;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _userTagService = userTagService;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
            _activityLocationService = activityLocationService;
            _intranetMemberService = intranetMemberService;
            _localizationService = localizationService;
            _memberHelper = memberHelper;
        }

        public override Enum Type => IntranetActivityTypeEnum.News;

        public override Enum PermissionActivityType => PermissionResourceTypeEnum.News;

        public override IntranetActivityPreviewModelBase GetPreviewModel(Guid activityId)
        {
            var news = Get(activityId);

            var links = _linkService.GetLinks(activityId);

            var currentMemberId = _intranetMemberService.GetCurrentMemberId();

            var viewModel = news.Map<IntranetActivityPreviewModelBase>();
            viewModel.CanEdit = CanEdit(news);
            viewModel.Links = links;
            viewModel.Owner = _memberHelper.ToViewModel(_intranetMemberService.Get(news));
            viewModel.IsPinActual = IsPinActual(news);
            viewModel.Type = _localizationService.Translate(news.Type.ToString());
            viewModel.LikedByCurrentUser = news.Likes.Any(x => x.UserId == currentMemberId);
            viewModel.CommentsCount = _commentsService.GetCount(viewModel.Id);
            
            var dates = news.PublishDate.ToDateTimeFormat().ToEnumerable().ToList();

            if (news.UnpublishDate.HasValue)
            {
                dates.Add(news.UnpublishDate.Value.ToDateTimeFormat());
            }

            viewModel.Dates = dates;

            _likesService.FillLikes(viewModel);
            DependencyResolver.Current.GetService<ILightboxHelper>().FillGalleryPreview(viewModel, news.MediaIds);

            return viewModel;
        }

        public MediaSettings GetMediaSettings()
        {
            return _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.NewsContent);
        }

        public FeedSettings GetFeedSettings() =>
            new FeedSettings
            {
                Type = CentralFeedTypeEnum.News,
                Controller = "News",
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

        //protected override void UpdateCache()
        //{
        //    base.UpdateCache();
        //    FillIndex();
        //}

        public override Entities.News UpdateActivityCache(Guid id)
        {
            var cachedNews = Get(id);
            var news = base.UpdateActivityCache(id);
            if (IsInCache(news) && (news.GroupId is null || _groupService.IsActivityFromActiveGroup(news)))
            {
                //_activityIndex.Index(Map(news));
                //_documentIndexer.Index(news.MediaIds);
                return news;
            }

            if (cachedNews == null) return null;

            //_activityIndex.Delete(id);
            //_documentIndexer.DeleteFromIndex(cachedNews.MediaIds);
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

        //public void FillIndex()
        //{
        //    var activities = GetAll().Where(IsInCache);
        //    var searchableActivities = activities.Select(Map);
        //    //_activityIndex.DeleteByType(UintraSearchableTypeEnum.News);
        //    //_activityIndex.Index(searchableActivities);
        //}

        private static bool IsInCache(Entities.News news)
        {
            return !IsNewsHidden(news) && IsActualPublishDate(news);
        }

        private static bool IsNewsHidden(IIntranetActivity news) =>
            news == null || news.IsHidden;

        private static bool IsActualPublishDate(INewsBase news) =>
            DateTime.Compare(news.PublishDate, DateTime.UtcNow) <= 0;

        //private SearchableUintraActivity Map(Entities.News news)
        //{
        //    var searchableActivity = news.Map<SearchableUintraActivity>();
        //    searchableActivity.Url = _linkService.GetLinks(news.Id).Details;
        //    searchableActivity.UserTagNames = _userTagService.Get(news.Id).Select(t => t.Text);
        //    return searchableActivity;
        //}

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