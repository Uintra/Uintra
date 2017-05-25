using uIntra.Core.User;

namespace uIntra.Navigation
{
    public class TopNavigationModelBuilder : ITopNavigationModelBuilder
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public TopNavigationModelBuilder(
            IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public TopNavigationModel Get()
        {
            var result = new TopNavigationModel
            {
                CurrentUser = _intranetUserService.GetCurrentUser()
            };

            return result;
        }
    }
}
