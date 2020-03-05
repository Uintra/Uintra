using System.Collections.Generic;
using System.Linq;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Localization;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Links;
using Uintra20.Features.News.Entities;
using Uintra20.Features.Social.Entities;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Logging;

namespace Uintra20.Core.Feed.Services
{
    public class FeedPresentationService : IFeedPresentationService
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IActivityLinkService _linkService;
        private readonly IIntranetLocalizationService _localizationService;
        private readonly IFeedActivityHelper _feedActivityHelper;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly ILogger _logger;
        private readonly IEnumerable<IIntranetActivityService<IIntranetActivity>> _intranetActivityServices;

        public FeedPresentationService(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IActivityLinkService linkService,
            IIntranetLocalizationService localizationService,
            IFeedActivityHelper feedActivityHelper,
            ICommentsService commentsService,
            ILikesService likesService,
            ILightboxHelper lightboxHelper,
            ILogger logger,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _intranetMemberService = intranetMemberService;
            _linkService = linkService;
            _localizationService = localizationService;
            _feedActivityHelper = feedActivityHelper;
            _commentsService = commentsService;
            _likesService = likesService;
            _lightboxHelper = lightboxHelper;
            _logger = logger;
            _intranetActivityServices = activitiesServiceFactory.GetServices<IIntranetActivityService<IIntranetActivity>>();
        }

        public IntranetActivityPreviewModelBase GetPreviewModel(IFeedItem feedItem, bool isGroupFeed)
        {
            var baseModel = GetBaseModel(feedItem, isGroupFeed);

            switch (feedItem)
            {
                case News news:
                    return ApplyNewsSpecific(news, baseModel);
                case Social social:
                    return ApplySocialSpecific(social, baseModel);
                case Event @event:
                    return null;
            }

            return baseModel;
        }

        private IntranetActivityPreviewModelBase GetBaseModel(IFeedItem feedItem, bool isGroupFeed)
        {
            if (feedItem is IntranetActivity activity)
            {
                var baseModel = activity.Map<IntranetActivityPreviewModelBase>();
                baseModel.Links = _linkService.GetLinks(feedItem.Id);
                baseModel.Type = _localizationService.Translate(activity.Type.ToString());
                baseModel.CommentsCount = _commentsService.GetCount(feedItem.Id);
                baseModel.Likes = _likesService.GetLikeModels(activity.Id);
                baseModel.GroupInfo = isGroupFeed ? null: _feedActivityHelper.GetGroupInfo(feedItem.Id);
                _lightboxHelper.FillGalleryPreview(baseModel, activity.MediaIds);

                return baseModel;
            }

            _logger.Warn<FeedPresentationService>("Feed item is not IntranetActivity (id={0};type={1})", feedItem.Id, feedItem.Type.ToInt());
            return null;

        }

        private IntranetActivityPreviewModelBase ApplyNewsSpecific(News news, IntranetActivityPreviewModelBase previewModel)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            previewModel.Owner = _intranetMemberService.Get(news).ToViewModel();
            previewModel.LikedByCurrentUser = news.Likes.Any(x => x.UserId == currentMember.Id);
            previewModel.IsGroupMember = !news.GroupId.HasValue || currentMember.GroupIds.Contains(news.GroupId.Value);
            previewModel.IsPinActual = news.IsPinActual;
            previewModel.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.News)).CanEdit(news);
            return previewModel;
        }

        private IntranetActivityPreviewModelBase ApplySocialSpecific(Social social, IntranetActivityPreviewModelBase previewModel)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            previewModel.Owner = _intranetMemberService.Get(social).ToViewModel();
            previewModel.LikedByCurrentUser = social.Likes.Any(x => x.UserId == currentMember.Id);
            previewModel.IsGroupMember = !social.GroupId.HasValue || currentMember.GroupIds.Contains(social.GroupId.Value);
            previewModel.IsPinActual = social.IsPinActual;
            previewModel.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.Social)).CanEdit(social);

            return previewModel;
        }
    }

    public interface IFeedPresentationService
    {
        IntranetActivityPreviewModelBase GetPreviewModel(IFeedItem feedItems, bool isGroupFeed);
    }
}