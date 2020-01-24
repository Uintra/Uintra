using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Features.Navigation.ModelBuilders.TopMenu
{
    public class TopNavigationModelBuilder : ITopNavigationModelBuilder
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IContentPageContentProvider _contentPageContentPropvider;


        public TopNavigationModelBuilder(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IContentPageContentProvider contentPageContentPropvider)
        {
            _intranetMemberService = intranetMemberService;
            _contentPageContentPropvider = contentPageContentPropvider;
        }

        public TopNavigationModel Get()
        {
            var result = new TopNavigationModel
            {
                //CurrentMember = _intranetMemberService.GetCurrentMember(),
                //CentralUserListUrl = _contentPageContentPropvider.GetUserListContentPageFromPicker()?.Url
            };

            return result;
        }
    }
}