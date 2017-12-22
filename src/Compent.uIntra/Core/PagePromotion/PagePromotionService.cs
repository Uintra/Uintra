using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Grid;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Likes;
using uIntra.Search;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.PagePromotion
{
    public class PagePromotionService : IPagePromotionService<Entities.PagePromotion>, IFeedItemService
    {
        private const string CacheKey = "PagePromotionCache";

        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGridHelper _gridHelper;
        private readonly ICacheService _cache;
        private readonly IDocumentIndexer _documentIndexer;

        public PagePromotionService(
            IActivityTypeProvider activityTypeProvider,
            IFeedTypeProvider feedTypeProvider,
            UmbracoHelper umbracoHelper,
            IIntranetUserService<IIntranetUser> userService,
            ILikesService likesService,
            ICommentsService commentsService,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGridHelper gridHelper,
            ICacheService cache,
            IDocumentIndexer documentIndexer)
        {
            _activityTypeProvider = activityTypeProvider;
            _feedTypeProvider = feedTypeProvider;
            _umbracoHelper = umbracoHelper;
            _userService = userService;
            _likesService = likesService;
            _commentsService = commentsService;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _gridHelper = gridHelper;
            _cache = cache;
            _documentIndexer = documentIndexer;
        }

        public IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.PagePromotion.ToInt());

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = _feedTypeProvider.Get(CentralFeedTypeEnum.PagePromotion.ToInt()),
                Controller = "PagePromotion",
                HasSubscribersFilter = false,
                HasPinnedFilter = false,
                ExcludeFromAvailableActivityTypes = true,
                ExcludeFromLatestActivities = true
            };
        }

        public Entities.PagePromotion Get(Guid id)
        {
            var cached = GetAll(true).SingleOrDefault(s => s.Id == id);
            return cached;
        }

        public IEnumerable<Entities.PagePromotion> GetManyActual()
        {
            var cached = GetAll(true);
            return cached.Where(IsActual);
        }

        public IEnumerable<Entities.PagePromotion> GetAll(bool includeHidden = false)
        {
            if (!_cache.HasValue(CacheKey))
            {
                UpdateCache();
            }

            var cached = GetAllFromCache();
            if (!includeHidden)
            {
                cached = cached.Where(s => !s.IsHidden);
            }

            return cached;
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            return GetOrderedActualItems();
        }

        public bool CanEdit(IIntranetActivity cached) => false;

        public bool CanEdit(Guid id) => false;

        public void Delete(Guid id)
        {
            UpdateCachedEntity(id);
        }

        public bool IsActual(IIntranetActivity cachedActivity)
        {
            var pagePromotion = (Entities.PagePromotion)cachedActivity;
            return !pagePromotion.IsHidden && pagePromotion.PublishDate <= DateTime.Now;
        }

        public Guid Create(IIntranetActivity activity)
        {
            throw new NotImplementedException();
        }

        public void Save(IIntranetActivity activity)
        {
            UpdateCachedEntity(activity.Id);
        }

        private IOrderedEnumerable<Entities.PagePromotion> GetOrderedActualItems() => GetManyActual().OrderByDescending(i => i.PublishDate);

        private void UpdateCache()
        {
            FillCache();
        }

        private void FillCache()
        {
            var items = GetAllFromStorage();
            _cache.Set(CacheKey, items, CacheHelper.GetMidnightUtcDateTimeOffset());
        }

        private IEnumerable<Entities.PagePromotion> GetAllFromCache()
        {
            var activities = _cache.Get<IList<Entities.PagePromotion>>(CacheKey);
            return activities;
        }

        private Entities.PagePromotion UpdateCachedEntity(Guid id)
        {
            var cachedEntity = Get(id);

            var activity = GetFromStorage(id);
            var cached = GetAll(true);
            var cachedList = (cached as List<Entities.PagePromotion> ?? cached.ToList()).FindAll(s => s.Id != id);

            if (activity != null)
            {
                MapBeforeCache(activity.ToListOfOne());
                cachedList.Add(activity);
            }

            _cache.Set(CacheKey, cachedList, CacheHelper.GetMidnightUtcDateTimeOffset());

            if (IsPagePromotionHidden(activity))
            {
                _documentIndexer.DeleteFromIndex(cachedEntity.MediaIds);
                return null;
            }

            var cachedEntityMediaIds = cachedEntity?.MediaIds ?? Enumerable.Empty<int>();
            _documentIndexer.DeleteFromIndex(cachedEntityMediaIds.Except(activity.MediaIds));
            _documentIndexer.Index(activity.MediaIds);
            return activity;
        }

        private Entities.PagePromotion GetFromStorage(Guid id)
        {
            var content = _umbracoHelper.TypedContent(id);
            if (content == null || !PagePromotionHelper.IsPagePromotion(content)) return null;

            return MapInternal(content);
        }

        private IList<Entities.PagePromotion> GetAllFromStorage()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(pc => pc.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()));

            var activities = homePage
                .Descendants()
                .Where(PagePromotionHelper.IsPagePromotion)
                .Select(MapInternal)
                .ToList();

            return activities;
        }

        private void MapBeforeCache(IList<Entities.PagePromotion> cached)
        {
            foreach (var activity in cached)
            {
                //var entity = activity;
                //entity.GroupId = _groupActivityService.GetGroupId(activity.Id);
                //_subscribeService.FillSubscribers(entity);
                //_commentsService.FillComments(entity);
                //_likesService.FillLikes(entity);
            }
        }

        private Entities.PagePromotion MapInternal(IPublishedContent content)
        {
            var pagePromotion = content.Map<Entities.PagePromotion>();
            var config = PagePromotionHelper.GetConfig(content);

            Mapper.Map(config, pagePromotion);

            pagePromotion.Type = ActivityType;
            pagePromotion.CreatorId = _userService.Get(pagePromotion.UmbracoCreatorId.Value).Id;

            var panelValues = _gridHelper.GetValues(content, GridEditorConstants.CommentsPanelAlias, GridEditorConstants.LikesPanelAlias).ToList();
            pagePromotion.Commentable = panelValues.Any(panel => panel.alias == GridEditorConstants.CommentsPanelAlias);
            pagePromotion.Likeable = panelValues.Any(panel => panel.alias == GridEditorConstants.LikesPanelAlias);

            if (pagePromotion.Likeable)
            {
                _likesService.FillLikes(pagePromotion);
            }

            if (pagePromotion.Commentable)
            {
                _commentsService.FillComments(pagePromotion);
            }

            return pagePromotion;
        }

        private bool IsPagePromotionHidden(Entities.PagePromotion pagePromotion)
        {
            return pagePromotion == null || pagePromotion.IsHidden;
        }
    }
}