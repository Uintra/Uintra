using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Likes;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.PagePromotion
{
    public class PagePromotionService : IPagePromotionService<PagePromotion>, IFeedItemService
    {
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public PagePromotionService(
            IActivityTypeProvider activityTypeProvider,
            IFeedTypeProvider feedTypeProvider,
            UmbracoHelper umbracoHelper,
            IIntranetUserService<IIntranetUser> userService,
            ILikesService likesService,
            ICommentsService commentsService,
            IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _activityTypeProvider = activityTypeProvider;
            _feedTypeProvider = feedTypeProvider;
            _umbracoHelper = umbracoHelper;
            _userService = userService;
            _likesService = likesService;
            _commentsService = commentsService;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.PagePromotion.ToInt());

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = _feedTypeProvider.Get(CentralFeedTypeEnum.PagePromotion.ToInt()),
                Controller = "PagePromotion",
                HasSubscribersFilter = false,
                HasPinnedFilter = false
            };
        }

        public PagePromotion GetPagePromotion(Guid pageId)
        {
            var content = _umbracoHelper.TypedContent(pageId);
            var contentAndPagePromotionConfig = GetActualContentAndPagePromotionConfig(content);
            return MapToPagePromotionModel(contentAndPagePromotionConfig);
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            return GetManyActual().OrderByDescending(i => i.PublishDate);
        }

        private IEnumerable<IFeedItem> GetManyActual()
        {
            var homePage = _umbracoHelper.TypedContentAtRoot().Single(pc => pc.DocumentTypeAlias.Equals(_documentTypeAliasProvider.GetHomePage()));

            var contentAndPagePromotionConfigs = homePage
                .Descendants()
                .Select(GetActualContentAndPagePromotionConfig);

            return contentAndPagePromotionConfigs.Where(conf => conf != null).Select(MapToCentralFeedItem);
        }

        private Tuple<IPublishedContent, PagePromotionConfig> GetActualContentAndPagePromotionConfig(IPublishedContent content)
        {
            var isContentPromotionPage = false;// content is IPagePromotionComposition;
            if (!isContentPromotionPage)
            {
                return null;
            }

            var pagePromotionConfig = GetPagePromotionConfig(content);
            if (pagePromotionConfig != null && pagePromotionConfig.PromoteOnCentralFeed && pagePromotionConfig.PublishDate <= DateTime.Now)
            {
                return new Tuple<IPublishedContent, PagePromotionConfig>(content, pagePromotionConfig);
            }

            return null;
        }

        private PagePromotion MapToPagePromotionModel(Tuple<IPublishedContent, PagePromotionConfig> contentAndPagePromotionConfig)
        {
            var pagePromotionConfig = contentAndPagePromotionConfig.Item2;
            var pagePromotionModel = contentAndPagePromotionConfig.Item1.Map<PagePromotion>();
            pagePromotionModel.CreatorId = _userService.Get(pagePromotionModel.UmbracoCreatorId.Value).Id;

            FillPagePromotionConfig(pagePromotionModel, pagePromotionConfig);
            return pagePromotionModel;
        }

        private IFeedItem MapToCentralFeedItem(Tuple<IPublishedContent, PagePromotionConfig> contentAndPagePromotionConfig)
        {
            var pagePromotionConfig = contentAndPagePromotionConfig.Item2;

            var centralFeedItem = contentAndPagePromotionConfig.Item1.Map<PagePromotion>();
            FillPagePromotionConfig(centralFeedItem, pagePromotionConfig);

            centralFeedItem.Commentable = pagePromotionConfig.Comentable;
            centralFeedItem.Likeable = pagePromotionConfig.Likeable;
            centralFeedItem.CreatorId = _userService.Get(centralFeedItem.UmbracoCreatorId.Value).Id;

            _likesService.FillLikes(centralFeedItem);
            _commentsService.FillComments(centralFeedItem);

            return centralFeedItem;
        }

        private void FillPagePromotionConfig(PagePromotion model, PagePromotionConfig pagePromotionConfig)
        {
            model.Type = new IntranetType
            {
                Id = ActivityType.Id,
                Name = model.PageAlias
            };
            model.Title = pagePromotionConfig.Title;
            model.Description = pagePromotionConfig.Description;
            model.PublishDate = pagePromotionConfig.PublishDate;
            model.MediaIds = pagePromotionConfig.Files.ToIntCollection();
        }

        private PagePromotionConfig GetPagePromotionConfig(IPublishedContent content)
        {
            var config = new PagePromotionConfig();
            var prop = content.GetPropertyValue<string>("pagePromotionConfig");
            if (prop.IsNotNullOrEmpty())
            {
                config = prop.Deserialize<PagePromotionConfig>();
               // config.Comentable = content.GridContainsPlugin("Comments");
               // config.Likeable = content.GridContainsPlugin("Likes");
                return config;
            }
            return config;
        }
    }
}