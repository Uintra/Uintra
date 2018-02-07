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
    public class PagePromotionService : PagePromotionServiceBase<Entities.PagePromotion>,
        IFeedItemService,
        ILikeableService,
        ICommentableService
    {
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly IGridHelper _gridHelper;
        private readonly IDocumentIndexer _documentIndexer;

        public PagePromotionService(
            IActivityTypeProvider activityTypeProvider,
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
            _userService = userService;
            _likesService = likesService;
            _commentsService = commentsService;
            _gridHelper = gridHelper;
            _documentIndexer = documentIndexer;
        }

        public override Enum ActivityType => IntranetActivityTypeEnum.PagePromotion;

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = CentralFeedTypeEnum.PagePromotion,
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

        public ILikeable AddLike(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            UpdateCachedEntity(activityId);
            return Get(activityId);
        }

        public ILikeable RemoveLike(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            UpdateCachedEntity(activityId);
            return Get(activityId);
        }

        public CommentModel CreateComment(CommentCreateDto dto)
        {
            var comment = _commentsService.Create(dto);
            UpdateCachedEntity(comment.ActivityId);
            return comment;
        }

        public void UpdateComment(CommentEditDto dto)
        {
            var comment = _commentsService.Update(dto);
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

        protected override Entities.PagePromotion UpdateCachedEntity(Guid id)
        {
            var cachedEntity = Get(id);

            var activity = base.UpdateCachedEntity(id);
            if (IsPagePromotionHidden(activity))
            {
                if (cachedEntity != null)
                {
                    _documentIndexer.DeleteFromIndex(cachedEntity.MediaIds);
                }

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
            if (config == null) return null;

            Mapper.Map(config, pagePromotion);

            pagePromotion.Type = ActivityType;
            pagePromotion.CreatorId = _userService.Get(pagePromotion.UmbracoCreatorId.Value).Id;

            var panelValues = _gridHelper.GetValues(content, GridEditorConstants.CommentsPanelAlias, GridEditorConstants.LikesPanelAlias).ToList();
            pagePromotion.Commentable = panelValues.Any(panel => panel.alias == GridEditorConstants.CommentsPanelAlias);
            pagePromotion.Likeable = panelValues.Any(panel => panel.alias == GridEditorConstants.LikesPanelAlias);

            return pagePromotion;
        }

        private IOrderedEnumerable<Entities.PagePromotion> GetOrderedActualItems() => GetManyActual().OrderByDescending(i => i.PublishDate);
    }
}