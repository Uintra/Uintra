using uCommunity.Core.User;

namespace uCommunity.Navigation.Core
{
    public class TopNavigationModelBuilder : ITopNavigationModelBuilder
    {
        private readonly IIntranetUserService _intranetUserService;

        public TopNavigationModelBuilder(
            IIntranetUserService intranetUserService)
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
