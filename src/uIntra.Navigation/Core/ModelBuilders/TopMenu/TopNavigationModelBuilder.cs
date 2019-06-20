using Uintra.Core.Extensions;
using Uintra.Core.Providers;
using Uintra.Core.User;

namespace Uintra.Navigation
{
    public class TopNavigationModelBuilder : ITopNavigationModelBuilder
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IContentPageContentProvider _contentPageContentPropvider;


        public TopNavigationModelBuilder(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IContentPageContentProvider contentPageContentPropvider)
        {
            _intranetMemberService = intranetMemberService;
            _contentPageContentPropvider = contentPageContentPropvider;
        }

        public TopNavigationModel Get()
        {
            var result = new TopNavigationModel
            {
                CurrentMember = _intranetMemberService.GetCurrentMember(),
                CentralUserListUrl = _contentPageContentPropvider.GetUserListContentPageFromPicker()?.Url
            };

            return result;
        }
    }
}
