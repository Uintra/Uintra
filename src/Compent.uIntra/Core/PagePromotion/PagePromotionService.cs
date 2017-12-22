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
    public class PagePromotionService : PagePromotionServiceBase<Entities.PagePromotion>, IFeedItemService
    {
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly IGridHelper _gridHelper;
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
            ICacheService cacheService,
            IDocumentIndexer documentIndexer)
            : base(cacheService, umbracoHelper, documentTypeAliasProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _feedTypeProvider = feedTypeProvider;
            _userService = userService;
            _likesService = likesService;
            _commentsService = commentsService;
            _gridHelper = gridHelper;
            _documentIndexer = documentIndexer;
        }

        public override IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.PagePromotion.ToInt());

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

        public IEnumerable<IFeedItem> GetItems()
        {
            return GetOrderedActualItems();
        }

        protected override Entities.PagePromotion UpdateCachedEntity(Guid id)
        {
            var cachedEntity = Get(id);

            var activity = base.UpdateCachedEntity(id);
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

        protected override void MapBeforeCache(IList<Entities.PagePromotion> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                if (entity.Likeable)
                {
                    _likesService.FillLikes(entity);
                }

                if (entity.Commentable)
                {
                    _commentsService.FillComments(entity);
                }
            }
        }

        protected override Entities.PagePromotion MapInternal(IPublishedContent content)
        {
            var pagePromotion = content.Map<Entities.PagePromotion>();
            var config = PagePromotionHelper.GetConfig(content);

            Mapper.Map(config, pagePromotion);

            pagePromotion.Type = ActivityType;
            pagePromotion.CreatorId = _userService.Get(pagePromotion.UmbracoCreatorId.Value).Id;

            var panelValues = _gridHelper.GetValues(content, GridEditorConstants.CommentsPanelAlias, GridEditorConstants.LikesPanelAlias).ToList();
            pagePromotion.Commentable = panelValues.Any(panel => panel.alias == GridEditorConstants.CommentsPanelAlias);
            pagePromotion.Likeable = panelValues.Any(panel => panel.alias == GridEditorConstants.LikesPanelAlias);

            //if (pagePromotion.Likeable)
            //{
            //    _likesService.FillLikes(pagePromotion);
            //}

            //if (pagePromotion.Commentable)
            //{
            //    _commentsService.FillComments(pagePromotion);
            //}

            return pagePromotion;
        }

        private IOrderedEnumerable<Entities.PagePromotion> GetOrderedActualItems() => GetManyActual().OrderByDescending(i => i.PublishDate);
    }
}