using System;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Compent.Shared.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Bulletin.Converters.Models;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Tagging.UserTags.Models;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Bulletin.Converters
{
    public class BulletinDetailsPageViewModelConverter : INodeViewModelConverter<BulletinDetailsPageModel, BulletinDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly IBulletinsService<Features.Bulletins.Entities.Bulletin> _bulletinsService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _tagsService;
        private readonly IUserTagProvider _tagProvider;

        public BulletinDetailsPageViewModelConverter(IFeedLinkService feedLinkService,
            IBulletinsService<Features.Bulletins.Entities.Bulletin> bulletinsService,
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService tagsService,
            IUserTagProvider tagProvider)
        {
            _feedLinkService = feedLinkService;
            _bulletinsService = bulletinsService;
            _memberService = memberService;
            _tagsService = tagsService;
            _tagProvider = tagProvider;
        }

        public void Map(BulletinDetailsPageModel node, BulletinDetailsPageViewModel viewModel)
        {
            bool idParsed = Guid.TryParse(HttpContext.Current?.Request["id"], out Guid id);

            if (idParsed)
            {
                viewModel.Details = GetViewModel(id);
            }

            viewModel.Tags = GetTagsViewModel(idParsed ? (Guid?)id : null);
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

            viewModel.CanEdit = true;//_bulletinsService.CanEdit(bulletin);//TODO: Uncomment when members service is ready
            viewModel.Links = links;
            viewModel.IsReadOnly = false;

            viewModel.HeaderInfo = bulletin.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = bulletin.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(bulletin).Map<MemberViewModel>();//TODO: uncomment when member service is ready
            viewModel.HeaderInfo.Links = links;

            var extendedModel = viewModel.Map<BulletinExtendedViewModel>();
            extendedModel = bulletin.Map(extendedModel);
            return extendedModel;
        }

        private TagsPickerViewModel GetTagsViewModel(Guid? entityId = null)
        {
            var pickerViewModel = new TagsPickerViewModel
            {
                UserTagCollection = _tagProvider.GetAll(),
                TagIdsData = entityId.HasValue
                    ? _tagsService.Get(entityId.Value).Select(t => t.Id)
                    : Enumerable.Empty<Guid>()
            };

            return pickerViewModel;
        }
    }
}