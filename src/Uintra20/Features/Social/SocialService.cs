using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Compent.CommandBus;
using Compent.Extensions;
using Uintra20.Core.Activity;
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
using Uintra20.Features.Social.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;
using static Uintra20.Features.Notification.Configuration.NotificationTypeEnum;

namespace Uintra20.Features.Social
{
    public class SocialService<T> : SocialServiceBase<T>,
        ISocialService<T>,
        IFeedItemService,
        INotifyableService,
        //IIndexer,
        IHandle<VideoConvertedCommand> 
        where T : Entities.Social
    {
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly INotificationsService _notificationService;
        //private readonly IElasticUintraActivityIndex _activityIndex;
        //private readonly IDocumentIndexer _documentIndexer;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _linkService;
        private readonly IActivityLinkPreviewService _activityLinkPreviewService;
        private readonly IGroupService _groupService;
        private readonly INotifierDataBuilder _notifierDataBuilder;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly IFeedActivityHelper _feedActivityHelper;

        public SocialService(
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IActivityTypeProvider activityTypeProvider,
            //IElasticUintraActivityIndex activityIndex,
            //IDocumentIndexer documentIndexer,
            IMediaHelper mediaHelper,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            IActivityLocationService activityLocationService,
            IUserTagService userTagService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder,
            IIntranetLocalizationService localizationService,
            IMemberServiceHelper memberHelper,
            IFeedActivityHelper feedActivityHelper)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService,
                activityLocationService, activityLinkPreviewService, intranetMemberService, permissionsService)
        {
            _commentsService = commentsService;
            _likesService = likesService;
            _notificationService = notificationService;
            //_activityIndex = activityIndex;
            //_documentIndexer = documentIndexer;
            _mediaHelper = mediaHelper;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _activityLinkPreviewService = activityLinkPreviewService;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
            _intranetMemberService = intranetMemberService;
            _localizationService = localizationService;
            _feedActivityHelper = feedActivityHelper;
        }

        public override Enum Type => IntranetActivityTypeEnum.Social;

        public override Enum PermissionActivityType => PermissionResourceTypeEnum.Social;
        public override IntranetActivityPreviewModelBase GetPreviewModel(Guid activityId)
        {
            var bulletin = Get(activityId);

            if (bulletin == null)
            {
                return null;
            }

            var links = _linkService.GetLinks(activityId);

            var currentMemberId = _intranetMemberService.GetCurrentMemberId();

            var viewModel = bulletin.Map<SocialPreviewModel>();
            viewModel.CanEdit = CanEdit(bulletin);
            viewModel.Links = links;
            viewModel.Owner = _intranetMemberService.Get(bulletin).ToViewModel();
            viewModel.Type = _localizationService.Translate(bulletin.Type.ToString());
            viewModel.LikedByCurrentUser = bulletin.Likes.Any(x => x.UserId == currentMemberId);
            viewModel.CommentsCount = _commentsService.GetCount(viewModel.Id);
            viewModel.GroupInfo = _feedActivityHelper.GetGroupInfo(activityId);
            _likesService.FillLikes(viewModel);
            DependencyResolver.Current.GetService<ILightboxHelper>().FillGalleryPreview(viewModel, bulletin.MediaIds);

            return viewModel;
        }

        public MediaSettings GetMediaSettings() => _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.SocialsContent);

        //protected override void UpdateCache()
        //{
        //    base.UpdateCache();
        //    FillIndex();
        //}

        public FeedSettings GetFeedSettings() =>
            new FeedSettings
            {
                Type = CentralFeedTypeEnum.Social,
                Controller = "Bulletins",
                HasPinnedFilter = false,
                HasSubscribersFilter = false,
            };

        public IEnumerable<IFeedItem> GetItems() => GetOrderedActualItems();

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

        public override T UpdateActivityCache(Guid id)
        {
            var cachedBulletin = Get(id);
            var bulletin = base.UpdateActivityCache(id);
            if (IsCacheable(bulletin) && (bulletin.GroupId is null || _groupService.IsActivityFromActiveGroup(bulletin)))
            {
                //_activityIndex.Index(Map(social));
                //_documentIndexer.Index(social.MediaIds);
                return bulletin;
            }

            if (cachedBulletin == null) return null;

            //_activityIndex.Delete(id);
            //_documentIndexer.DeleteFromIndex(cachedBulletin.MediaIds);
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

        //public void FillIndex()
        //{
        //    var activities = GetAll().Where(IsCacheable);
        //    var searchableActivities = activities.Select(Map);
        //    _activityIndex.DeleteByType(UintraSearchableTypeEnum.Bulletins);
        //    _activityIndex.Index(searchableActivities);
        //}

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

        //private SearchableUintraActivity Map(Social social)
        //{
        //    var searchableActivity = social.Map<SearchableUintraActivity>();
        //    searchableActivity.Url = _linkService.GetLinks(social.Id).Details;
        //    searchableActivity.UserTagNames = _userTagService.Get(social.Id).Select(t => t.Text);
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