using uCommunity.Core.User;

namespace uCommunity.Navigation.Core
{
    public class TopNavigationModelBuilder : ITopNavigationModelBuilder
    {
        private readonly IIntranetUserService<IntranetUserBase> _intranetUserService;

        public TopNavigationModelBuilder(
            IIntranetUserService<IntranetUserBase> intranetUserService)
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
