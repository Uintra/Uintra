using Compent.Extensions;
using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Bulletin.Converters.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins.Converters.Models;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Media;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Bulletins.Converters
{
    public class SocialDetailsPageViewModelConverter : INodeViewModelConverter<SocialDetailsPageModel, SocialDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly IUserTagService _userTagService;
        private readonly ILikesService _likesService;
        private readonly ICommentsService _commentsService;
        private readonly ICommentsHelper _commentsHelper;
        private readonly ISocialsService<Social> _socialsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;

        public SocialDetailsPageViewModelConverter(
            IFeedLinkService feedLinkService,
            ISocialsService<Social> socialsService,
            IIntranetMemberService<IntranetMember> memberService, 
            IUserTagService userTagService, 
            ILikesService likesService, 
            ICommentsService commentsService, 
            ICommentsHelper commentsHelper)
        {
            _feedLinkService = feedLinkService;
            _socialsService = socialsService;
            _memberService = memberService;
            _userTagService = userTagService;
            _likesService = likesService;
            _commentsService = commentsService;
            _commentsHelper = commentsHelper;
        }

        public void Map(SocialDetailsPageModel node, SocialDetailsPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetUbaselineQueryValue("id");

            if (Guid.TryParse(id, out var parseId))
            {
                viewModel.Details = GetViewModel(parseId);
                viewModel.Tags = _userTagService.Get(parseId);
                viewModel.Likes = _likesService.Get(parseId);
                viewModel.LikedByCurrentUser = _likesService.LikedByCurrentUser(parseId, viewModel.Details.HeaderInfo.Owner.Id);
                viewModel.Comments = _commentsHelper.GetCommentViews(_commentsService.GetMany(parseId));
            }
        }

        protected SocialExtendedViewModel GetViewModel(Guid id)
        {
            var social = _socialsService.Get(id);
            
            if (social == null) return null;

            IActivityLinks links = null;//feedLinkService.GetLinks(id);//TODO:Uncomment when profile link service is ready

            var viewModel = social.Map<SocialViewModel>();

            viewModel.Media = MediaHelper.GetMediaUrls(social.MediaIds);
            viewModel.CanEdit = _socialsService.CanEdit(social);
            viewModel.Links = links;
            viewModel.IsReadOnly = false;
            viewModel.HeaderInfo = social.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = social.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(social).Map<MemberViewModel>();
            viewModel.HeaderInfo.Links = links;

            var extendedModel = viewModel.Map<SocialExtendedViewModel>();

            return extendedModel;
        }
    }
}