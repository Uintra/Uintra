using Compent.Extensions;
using System;
using System.Linq;
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
using Uintra20.Features.Likes.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Bulletins.Converters
{
    public class SocialDetailsPageViewModelConverter : INodeViewModelConverter<SocialDetailsPageModel, SocialDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly ISocialsService<Social> _socialsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService userTagService;
        private readonly ILikesService likesService;

        public SocialDetailsPageViewModelConverter(
            IFeedLinkService feedLinkService,
            ISocialsService<Social> socialsService,
            IIntranetMemberService<IntranetMember> memberService, 
            IUserTagService userTagService, 
            ILikesService likesService)
        {
            _feedLinkService = feedLinkService;
            _socialsService = socialsService;
            _memberService = memberService;
            this.userTagService = userTagService;
            this.likesService = likesService;
        }

        public void Map(SocialDetailsPageModel node, SocialDetailsPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetByKey("id");
            
            if (Guid.TryParse(id, out var parseId))
            {
                viewModel.Details = GetViewModel(parseId);
                viewModel.Tags = userTagService.Get(parseId);
                viewModel.Likes = likesService.Get(parseId);
                viewModel.LikedByCurrentUser = likesService.LikedByCurrentUser(parseId, viewModel.Details.HeaderInfo.Owner.Id);
            }
        }

        protected SocialExtendedViewModel GetViewModel(Guid id)
        {
            var social = _socialsService.Get(id);
            
            if (social == null) return null;

            IActivityLinks links = null;//_feedLinkService.GetLinks(id);//TODO:Uncomment when profile link service is ready

            var viewModel = social.Map<SocialViewModel>();

            viewModel.CanEdit = _socialsService.CanEdit(social);
            viewModel.Links = links;
            viewModel.IsReadOnly = false;
            viewModel.HeaderInfo = social.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = social.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(social).Map<MemberViewModel>();
            viewModel.HeaderInfo.Links = links;

            var extendedModel = viewModel.Map<SocialExtendedViewModel>();

            //extendedModel = social.Map(extendedModel);

            return extendedModel;
        }
    }
}