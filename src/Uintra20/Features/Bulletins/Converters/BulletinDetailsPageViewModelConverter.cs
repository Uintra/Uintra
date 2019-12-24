using System;
using System.Web;
using Compent.Extensions;
using Compent.Shared.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Bulletin.Converters.Models;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Bulletins.Converters
{
    public class BulletinDetailsPageViewModelConverter : INodeViewModelConverter<BulletinDetailsPageModel, BulletinDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly IBulletinsService<Entities.Bulletin> _bulletinsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;

        public BulletinDetailsPageViewModelConverter(IFeedLinkService feedLinkService,
            IBulletinsService<Entities.Bulletin> bulletinsService,
            IIntranetMemberService<IntranetMember> memberService)
        {
            _feedLinkService = feedLinkService;
            _bulletinsService = bulletinsService;
            _memberService = memberService;
        }

        public void Map(BulletinDetailsPageModel node, BulletinDetailsPageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetByKey("id");

            if (Guid.TryParse(id, out var parseId))
            {
                viewModel.Details = GetViewModel(parseId);
            }
        }

        protected BulletinExtendedViewModel GetViewModel(Guid id)
        {
            var bulletin = _bulletinsService.Get(id);

            if (bulletin == null)
            {
                return null;
            }

            IActivityLinks links = null;//_feedLinkService.GetLinks(id);//TODO:Uncomment when profile link service is ready

            var viewModel = bulletin.Map<BulletinViewModel>();

            viewModel.CanEdit = _bulletinsService.CanEdit(bulletin);
            viewModel.Links = links;
            viewModel.IsReadOnly = false;

            viewModel.HeaderInfo = bulletin.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = bulletin.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(bulletin).Map<MemberViewModel>();
            viewModel.HeaderInfo.Links = links;

            var extendedModel = viewModel.Map<BulletinExtendedViewModel>();
            //extendedModel = bulletin.Map(extendedModel);
            return extendedModel;
        }
    }
}