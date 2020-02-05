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
using Uintra20.Features.Social.Details.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Details.Converters
{
    public class SocialDetailsPageViewModelConverter : 
        INodeViewModelConverter<SocialDetailsPageModel, SocialDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly IUserTagService _userTagService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly ICommentsHelper _commentsHelper;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IMemberServiceHelper _memberHelper;

        public SocialDetailsPageViewModelConverter(
            IFeedLinkService feedLinkService,
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService userTagService,
            ILikesService likesService,
            ICommentsService commentsService,
            ISocialService<Entities.Social> socialsService,
            ICommentsHelper commentsHelper,
            ILightboxHelper lightboxHelper,
            IMemberServiceHelper memberHelper)
        {
            _feedLinkService = feedLinkService;
            _userTagService = userTagService;
            _likesService = likesService;
            _commentsService = commentsService;
            _commentsHelper = commentsHelper;
            _socialService = socialsService;
            _memberService = memberService;
            _lightboxHelper = lightboxHelper;
            _memberHelper = memberHelper;
        }

        public void Map(SocialDetailsPageModel node, SocialDetailsPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parseId)) 
                return;

            var userId = _memberService.GetCurrentMemberId();

            viewModel.Details = GetViewModel(parseId);
            viewModel.Tags = _userTagService.Get(parseId);
            viewModel.Likes = _likesService.GetLikeModels(parseId);
            viewModel.LikedByCurrentUser = viewModel.Likes.Any(l => l.UserId == userId);
            viewModel.Comments = _commentsHelper.GetCommentViews(_commentsService.GetMany(parseId));
        }

        protected SocialExtendedViewModel GetViewModel(Guid id)
        {
            var social = _socialService.Get(id);

            var viewModel = social.Map<SocialViewModel>();

            viewModel.Media = MediaHelper.GetMediaUrls(social.MediaIds);

            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, RenderStrategies.ForActivityDetails);
            viewModel.CanEdit = _socialService.CanEdit(social);
            viewModel.Links = _feedLinkService.GetLinks(id);
            viewModel.IsReadOnly = false;
            viewModel.HeaderInfo = social.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = social.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberHelper.ToViewModel(_memberService.Get(social));
            viewModel.HeaderInfo.Links = _feedLinkService.GetLinks(id);

            var extendedModel = viewModel.Map<SocialExtendedViewModel>();

            return extendedModel;
        }
    }
}