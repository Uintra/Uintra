using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Grid;
using Uintra.Core.PagePromotion;
using Uintra.Core.User;
using Uintra.Likes;
using Uintra.Search;
using Umbraco.Core.Models;
using Umbraco.Web;
using static Uintra.Core.Constants.GridEditorConstants;

namespace Compent.Uintra.Core.PagePromotion
{
    public class PagePromotionService : PagePromotionServiceBase<Entities.PagePromotion>, IFeedItemService
    {
        private readonly IIntranetMemberService<IIntranetMember> _memberService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly IGridHelper _gridHelper;
        private readonly IDocumentIndexer _documentIndexer;

        public PagePromotionService(
            UmbracoHelper umbracoHelper,
            IIntranetMemberService<IIntranetMember> memberService,
            ILikesService likesService,
            ICommentsService commentsService,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGridHelper gridHelper,
            ICacheService cacheService,
            IDocumentIndexer documentIndexer)
            : base(cacheService, umbracoHelper, documentTypeAliasProvider)
        {
            _memberService = memberService;
            _likesService = likesService;
            _commentsService = commentsService;
            _gridHelper = gridHelper;
            _documentIndexer = documentIndexer;
        }

        public override Enum Type => IntranetActivityTypeEnum.PagePromotion;

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

        [Obsolete("This method should be removed. Use UpdateActivityCache instead.")]
        protected override Entities.PagePromotion UpdateCachedEntity(Guid id) => UpdateActivityCache(id);

        public override Entities.PagePromotion UpdateActivityCache(Guid id)
        {
            var cachedEntity = Get(id);

            var activity = base.UpdateActivityCache(id);
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
            if (config.IsNone) return null;


            config.IfSome(cfg => Mapper.Map(cfg, pagePromotion));            
            pagePromotion.Type = Type;
            pagePromotion.CreatorId = _memberService.Get(pagePromotion.UmbracoCreatorId.Value).Id;

            var panelValues = _gridHelper.GetValues(content, CommentsPanelAlias, LikesPanelAlias).ToList();
            pagePromotion.Commentable = panelValues.Any(panel => panel.alias == CommentsPanelAlias);
            pagePromotion.Likeable = panelValues.Any(panel => panel.alias == LikesPanelAlias);

            return pagePromotion;
        }

        private IOrderedEnumerable<Entities.PagePromotion> GetOrderedActualItems() => GetManyActual().OrderByDescending(i => i.PublishDate);
    }
}