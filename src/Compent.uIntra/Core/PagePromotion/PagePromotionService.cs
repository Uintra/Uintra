using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Constants;
using uIntra.Core.Extensions;
using uIntra.Core.Grid;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Likes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.PagePromotion
{
    public class PagePromotionService : IPagePromotionService<Entities.PagePromotion>, IIntranetActivityService<Entities.PagePromotion>, IFeedItemService
    {
        protected virtual string PagePromotionConfigAlias { get; } = "pagePromotionConfig";

        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGridHelper _gridHelper;

        public PagePromotionService(
            IActivityTypeProvider activityTypeProvider,
            IFeedTypeProvider feedTypeProvider,
            UmbracoHelper umbracoHelper,
            IIntranetUserService<IIntranetUser> userService,
            ILikesService likesService,
            ICommentsService commentsService,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IGridHelper gridHelper)
        {
            _activityTypeProvider = activityTypeProvider;
            _feedTypeProvider = feedTypeProvider;
            _umbracoHelper = umbracoHelper;
            _userService = userService;
            _likesService = likesService;
            _commentsService = commentsService;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _gridHelper = gridHelper;
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
            var content = _umbracoHelper.TypedContent(id);
            if (content == null) return null;

            var config = GetPagePromotionConfig(content);
            return GetPagePromotion(content, config);
        }

        public Entities.PagePromotion GetPagePromotion(Guid pageId)
        {
            var content = _umbracoHelper.TypedContent(pageId);
            if (content == null) return null;

            var config = GetPagePromotionConfig(content);
            return GetPagePromotion(content, config);
        }

        public virtual IEnumerable<IFeedItem> GetItems()
        {
            return GetManyActual().OrderByDescending(i => i.PublishDate);
        }

        protected virtual IEnumerable<IFeedItem> GetManyActual()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(pc => pc.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()));

            var contentAndPagePromotionConfigs = homePage
                .Descendants()
                .Where(IsPagePromotion)
                .Select(GetContentAndConfig)
                .Where(contentAndConfig => IsActual(contentAndConfig.config));

            return contentAndPagePromotionConfigs.Select(contentAndConfig => GetCentralFeedItem(contentAndConfig.content, contentAndConfig.config));
        }

        protected virtual (IPublishedContent content, PagePromotionConfig config) GetContentAndConfig(IPublishedContent content)
        {
            var pagePromotionConfig = GetPagePromotionConfig(content);
            return (content, pagePromotionConfig);
        }

        protected virtual Entities.PagePromotion GetPagePromotion(IPublishedContent content, PagePromotionConfig config)
        {
            var pagePromotion = content.Map<Entities.PagePromotion>();
            Mapper.Map(config, pagePromotion);

            pagePromotion.Type = new IntranetType
            {
                Id = ActivityType.Id,
                Name = ActivityType.Name
            };

            pagePromotion.CreatorId = _userService.Get(pagePromotion.UmbracoCreatorId.Value).Id;

            return pagePromotion;
        }

        protected virtual PagePromotionConfig GetPagePromotionConfig(IPublishedContent content)
        {
            var config = new PagePromotionConfig();
            var prop = content.GetPropertyValue<string>(PagePromotionConfigAlias);
            if (prop.IsNotNullOrEmpty())
            {
                config = prop.Deserialize<PagePromotionConfig>();

                var panelValues = _gridHelper.GetValues(content, GridEditorConstants.CommentsPanelAlias, GridEditorConstants.LikesPanelAlias).ToList();

                config.Commentable = panelValues.Any(panel => panel.alias == GridEditorConstants.CommentsPanelAlias);
                config.Likeable = panelValues.Any(panel => panel.alias == GridEditorConstants.LikesPanelAlias);
                return config;
            }

            return config;
        }

        protected virtual IFeedItem GetCentralFeedItem(IPublishedContent content, PagePromotionConfig config)
        {
            var centralFeedItem = GetPagePromotion(content, config);

            if (centralFeedItem.Likeable)
            {
                _likesService.FillLikes(centralFeedItem);
            }

            if (centralFeedItem.Commentable)
            {
                _commentsService.FillComments(centralFeedItem);
            }

            return centralFeedItem;
        }

        protected virtual bool IsPagePromotion(IPublishedContent content)
        {
            return content.HasProperty(PagePromotionConfigAlias);
        }

        protected virtual bool IsActual(PagePromotionConfig config)
        {
            return config.PromoteOnCentralFeed && config.PublishDate <= DateTime.Now;
        }

        #region NotImplemented

        public bool CanEdit(IIntranetActivity cached)
        {
            throw new NotImplementedException();
        }

        protected void MapBeforeCache(IList<Entities.PagePromotion> cached)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool CanEdit(Guid id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Entities.PagePromotion> IIntranetActivityService<Entities.PagePromotion>.GetManyActual()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entities.PagePromotion> GetAll(bool includeHidden = false)
        {
            throw new NotImplementedException();
        }

        public bool IsActual(IIntranetActivity cachedActivity)
        {
            throw new NotImplementedException();
        }

        public Guid Create(IIntranetActivity activity)
        {
            throw new NotImplementedException();
        }

        public void Save(IIntranetActivity activity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}