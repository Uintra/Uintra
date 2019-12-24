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
        private readonly ISocialsService<Entities.Social> _socialsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;

        public SocialDetailsPageViewModelConverter(IFeedLinkService feedLinkService,
            ISocialsService<Entities.Social> socialsService,
            IIntranetMemberService<IntranetMember> memberService)
        {
            _feedLinkService = feedLinkService;
            _socialsService = socialsService;
            _memberService = memberService;
        }

        public void Map(SocialDetailsPageModel node, SocialDetailsPageViewModel viewModel)
        {
            if (Guid.TryParse(HttpContext.Current?.Request["id"], out Guid id))
            {
                viewModel.Details = GetViewModel(id);
            }
        }

        protected SocialExtendedViewModel GetViewModel(Guid id)
        {
            var bulletin = _socialsService.Get(id);

            if (bulletin == null)
            {
                return null;
            }

            IActivityLinks links = null;//_feedLinkService.GetLinks(id);//TODO:Uncomment when profile link service is ready

            var viewModel = bulletin.Map<SocialViewModel>();

            viewModel.CanEdit = _socialsService.CanEdit(bulletin);
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