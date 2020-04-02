using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.Extensions;
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
using Uintra20.Features.Subscribe;
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

        private IntranetActivityPreviewModelBase GetBaseModel(IFeedItem feedItem, bool isGroupFeed)
        {
            if (feedItem is IntranetActivity activity)
            {
                var baseModel = new IntranetActivityPreviewModelBase
                {
                    Id = feedItem.Id,
                    Links = _linkService.GetLinks(feedItem.Id),
                    Type = _localizationService.Translate(activity.Type.ToString()),
                    CommentsCount = _commentsService.GetCount(feedItem.Id),
                    Likes = _likesService.GetLikeModels(activity.Id),
                    GroupInfo = isGroupFeed ? null : _feedActivityHelper.GetGroupInfo(feedItem.Id),
                    ActivityType = feedItem.Type
                };
                _lightboxHelper.FillGalleryPreview(baseModel, activity.MediaIds);

                return baseModel;
            }

            _logger.Warn<FeedPresentationService>("Feed item is not IntranetActivity (id={0};type={1})", feedItem.Id, feedItem.Type.ToInt());
            return null;

        }

        private IntranetActivityPreviewModelBase ApplyNewsSpecific(News news, IntranetActivityPreviewModelBase previewModel)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            previewModel.Description = news.Description;
            previewModel.Title = news.Title;
            previewModel.Owner = _intranetMemberService.Get(news).ToViewModel();
            previewModel.LikedByCurrentUser = news.Likes.Any(x => x.UserId == currentMember.Id);
            previewModel.IsGroupMember = !news.GroupId.HasValue || currentMember.GroupIds.Contains(news.GroupId.Value);
            previewModel.IsPinActual = news.IsPinActual;
            previewModel.IsPinned = news.IsPinned;
            previewModel.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.News)).CanEdit(news);
            previewModel.Dates = news.PublishDate.ToDateFormat().ToEnumerable();
            previewModel.Location = news.Location;

            return previewModel;
        }

        private IntranetActivityPreviewModelBase ApplySocialSpecific(Social social, IntranetActivityPreviewModelBase previewModel)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            previewModel.Description = social.Description;
            previewModel.Title = social.Title;
            previewModel.Owner = _intranetMemberService.Get(social).ToViewModel();
            previewModel.LikedByCurrentUser = social.Likes.Any(x => x.UserId == currentMember.Id);
            previewModel.IsGroupMember = !social.GroupId.HasValue || currentMember.GroupIds.Contains(social.GroupId.Value);
            previewModel.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.Social)).CanEdit(social);
            previewModel.Dates = social.PublishDate.ToDateFormat().ToEnumerable();
            previewModel.Location = social.Location;
            
            return previewModel;
        }

        private IntranetActivityPreviewModelBase ApplyEventSpecific(Event @event, IntranetActivityPreviewModelBase previewModel)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            previewModel.Description = @event.Description;
            previewModel.Title = @event.Title;
            previewModel.Owner = _intranetMemberService.Get(@event).ToViewModel();
            previewModel.LikedByCurrentUser = @event.Likes.Any(x => x.UserId == currentMember.Id);
            previewModel.IsGroupMember = !@event.GroupId.HasValue || currentMember.GroupIds.Contains(@event.GroupId.Value);
            previewModel.IsPinActual = @event.IsPinActual;
            previewModel.CanEdit = _intranetActivityServices.First(s => Equals(s.Type, IntranetActivityTypeEnum.Events)).CanEdit(@event);
            previewModel.CurrentMemberSubscribed = @event.Subscribers.Any(x => x.UserId == currentMember.Id);
            previewModel.IsPinActual = @event.IsPinActual;
            previewModel.IsPinned = @event.IsPinned;
            previewModel.Location = @event.Location;
            
            var startDate = @event.StartDate.ToDateTimeFormat();
            string endDate;

            if (@event.StartDate.Date == @event.EndDate.Date)
            {
                endDate = @event.EndDate.ToTimeFormat();
            }
            else
            {
                endDate = @event.EndDate.ToDateTimeFormat();
            }

            @previewModel.Dates = new[] { startDate, endDate };

            return previewModel;
        }
    }

    public interface IFeedPresentationService
    {
        IntranetActivityPreviewModelBase GetPreviewModel(IFeedItem feedItems, bool isGroupFeed);
    }
}