using Uintra.Core.Providers;
using Uintra.Core.User;

namespace Uintra.Navigation
{
    public class TopNavigationModelBuilder : ITopNavigationModelBuilder
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IContentPageContentProvider _contentPageContentPropvider;

        public TopNavigationModelBuilder(
            IIntranetUserService<IIntranetUser> intranetUserService,
            IContentPageContentProvider contentPageContentPropvider)
        {
            _intranetUserService = intranetUserService;
            _contentPageContentPropvider = contentPageContentPropvider;
        }

        public TopNavigationModel Get()
        {
            var result = new TopNavigationModel
            {
                CurrentUser = _intranetUserService.GetCurrentUser(),
                CentralUserListUrl = _contentPageContentPropvider.GetUserListContentPageFromPicker()?.Url
            };

            return result;
        }
    }
}
