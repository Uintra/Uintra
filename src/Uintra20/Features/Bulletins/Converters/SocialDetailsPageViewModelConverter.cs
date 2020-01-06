using Compent.Extensions;
using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Bulletin.Converters.Models;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Bulletins.Converters
{
    public class SocialDetailsPageViewModelConverter : INodeViewModelConverter<SocialDetailsPageModel, SocialDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;

        public SocialDetailsPageViewModelConverter(IFeedLinkService feedLinkService,
            ISocialService<Entities.Social> socialService,
            IIntranetMemberService<IntranetMember> memberService)
        {
            _feedLinkService = feedLinkService;
            _socialService = socialService;
            _memberService = memberService;
        }

        public void Map(SocialDetailsPageModel node, SocialDetailsPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetUbaselineQueryValue("id");

            if (Guid.TryParse(id, out var parseId))
            {
                viewModel.Details = GetViewModel(parseId);
            }
        }

        protected SocialExtendedViewModel GetViewModel(Guid id)
        {
            var bulletin = _socialService.Get(id);

            if (bulletin == null)
            {
                return null;
            }

            IActivityLinks links = null;//_feedLinkService.GetLinks(id);//TODO:Uncomment when profile link service is ready

            var viewModel = bulletin.Map<SocialViewModel>();

            viewModel.CanEdit = _socialService.CanEdit(bulletin);
            viewModel.Links = links;
            viewModel.IsReadOnly = false;

            viewModel.HeaderInfo = bulletin.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = bulletin.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(bulletin).Map<MemberViewModel>();
            viewModel.HeaderInfo.Links = links;

            var extendedModel = viewModel.Map<SocialExtendedViewModel>();
            //extendedModel = social.Map(extendedModel);
            return extendedModel;
        }
    }
}