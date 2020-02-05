using Compent.Extensions;
using System;
using System.Linq;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Strategies.ImageResize;
using Uintra20.Features.News.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Converters
{
    public class UintraNewsDetailsPageViewModelConverter : INodeViewModelConverter<UintraNewsDetailsPageModel, UintraNewsDetailsPageViewModel>
    {
        private readonly ICommentsService _commentsService;
        private readonly IUserTagService _userTagService;
        private readonly ILikesService _likesService;
        private readonly ICommentsHelper _commentsHelper;
        private readonly IFeedLinkService _feedLinkService;
        private readonly INewsService<Entities.News> _newsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ILightboxHelper _lightBoxHelper;
        private readonly IMemberServiceHelper _memberHelper;

        public UintraNewsDetailsPageViewModelConverter(
            ICommentsService commentsService,
            IUserTagService userTagService,
            ILikesService likesService,
            ICommentsHelper commentsHelper, 
            IFeedLinkService feedLinkService,
            INewsService<Entities.News> newsService,
            IIntranetMemberService<IntranetMember> memberService,
            ILightboxHelper lightBoxHelper,
            IMemberServiceHelper memberHelper)
        {
            _commentsService = commentsService;
            _userTagService = userTagService;
            _likesService = likesService;
            _commentsHelper = commentsHelper;
            _feedLinkService = feedLinkService;
            _newsService = newsService;
            _memberService = memberService;
            _memberHelper = memberHelper;
            _lightBoxHelper = lightBoxHelper;
        }

        public void Map(UintraNewsDetailsPageModel node, UintraNewsDetailsPageViewModel viewModel)
        {
            var idStr = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(idStr, out var id))
                return;

            var userId = _memberService.GetCurrentMemberId();

            viewModel.Details = GetDetails(id);
            viewModel.Tags = _userTagService.Get(id);
            viewModel.Likes = _likesService.GetLikeModels(id);
            viewModel.LikedByCurrentUser = viewModel.Likes.Any(l => l.UserId == userId);
            viewModel.Comments = _commentsHelper.GetCommentViews(_commentsService.GetMany(id));

        }

        private NewsViewModel GetDetails(Guid activityId)
        {
            var news = _newsService.Get(activityId);

            var details = news.Map<NewsViewModel>();

            details.Media = MediaHelper.GetMediaUrls(news.MediaIds);

            details.LightboxPreviewModel = _lightBoxHelper.GetGalleryPreviewModel(news.MediaIds, RenderStrategies.ForActivityDetails);
            details.CanEdit = _newsService.CanEdit(news);
            details.Links = _feedLinkService.GetLinks(activityId);
            details.IsReadOnly = false;
            details.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            details.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
            details.HeaderInfo.Owner = _memberHelper.ToViewModel(_memberService.Get(news));
            details.HeaderInfo.Links = _feedLinkService.GetLinks(activityId);


            return details;
        }
    }
}