using Compent.Extensions;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
                    return ApplyEventSpecific(@event, baseModel);
            }

            return baseModel;
        }

        private IntranetActivityPreviewModelBase GetBaseModel(
            IFeedItem feedItem,
            bool isGroupFeed)
        {
            if (feedItem is IntranetActivity activity)
            {
                var baseModel = new IntranetActivityPreviewModelBase
                {
                    Links = _linkService.GetLinks(feedItem.Id),
                    Type = _localizationService.Translate(activity.Type.ToString()),
                    CommentsCount = _commentsService.GetCount(feedItem.Id),
                    Likes = _likesService.GetLikeModels(activity.Id),
                    GroupInfo = isGroupFeed ? null : _feedActivityHelper.GetGroupInfo(feedItem.Id),
                };
                _lightboxHelper.FillGalleryPreview(baseModel, activity.MediaIds);

                return baseModel;
            }

            _logger.Warn<FeedPresentationService>("Feed item is not IntranetActivity (id={0};type={1})", feedItem.Id, feedItem.Type.ToInt());

            return null;
        }

        private IntranetActivityPreviewModelBase ApplyNewsSpecific(
            News news,
            IntranetActivityPreviewModelBase preview)
        {
            var member = _intranetMemberService.GetCurrentMember();
            var mapped = news.Map(preview);
            
            mapped.Owner = _intranetMemberService.Get(news).ToViewModel();
            mapped.LikedByCurrentUser = news.Likes.Any(x => x.UserId == member.Id);
            mapped.IsGroupMember = !news.GroupId.HasValue || member.GroupIds.Contains(news.GroupId.Value);
            mapped.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.News)).CanEdit(news);

            return mapped;
        }

        private IntranetActivityPreviewModelBase ApplySocialSpecific(
            Social social,
            IntranetActivityPreviewModelBase preview)
        {
            var member = _intranetMemberService.GetCurrentMember();
            var mapped = social.Map(preview);

            mapped.Owner = _intranetMemberService.Get(social).ToViewModel();
            mapped.LikedByCurrentUser = social.Likes.Any(x => x.UserId == member.Id);
            mapped.IsGroupMember = !social.GroupId.HasValue || member.GroupIds.Contains(social.GroupId.Value);
            mapped.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.Social)).CanEdit(social);

            return mapped;
        }

        private IntranetActivityPreviewModelBase ApplyEventSpecific(
            Event @event,
            IntranetActivityPreviewModelBase preview)
        {
            var member = _intranetMemberService.GetCurrentMember();
            var mapped = @event.Map(preview);
            
            mapped.Owner = _intranetMemberService.Get(@event).ToViewModel();
            mapped.LikedByCurrentUser = @event.Likes.Any(x => x.UserId == member.Id);
            mapped.IsGroupMember = !@event.GroupId.HasValue || member.GroupIds.Contains(@event.GroupId.Value);
            mapped.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.Events)).CanEdit(@event);
            mapped.CurrentMemberSubscribed = @event.Subscribers.Any(x => x.UserId == member.Id);
            mapped.Dates = SetDates(@event);

            return mapped;
        }

        private IEnumerable<string> SetDates(Event @event)
        {
            var startDate = @event.StartDate.ToDateTimeFormat();

            var endDate = @event.StartDate.Date == @event.EndDate.Date
                ? @event.EndDate.ToTimeFormat()
                : @event.EndDate.ToDateTimeFormat();

            return new[] { startDate, endDate };
        }
    }
}