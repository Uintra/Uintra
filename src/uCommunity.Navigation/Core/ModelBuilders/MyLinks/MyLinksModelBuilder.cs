using uCommunity.Core.User;

namespace uCommunity.Navigation.Core
{
    public class MyLinksModelBuilder : IMyLinksModelBuilder
    {
        private readonly IIntranetUserService _intranetUserService;

        public MyLinksModelBuilder(
            IIntranetUserService intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public MyLinksModel Get()
        {
            var result = new MyLinksModel();

            var homePage = GetHomePage();
            if (IsContentUnavailable(homePage))
            {
                return result;
            }

            var homePageMenu = GetHomePageMenuItem(homePage);
            result.MenuItems.Add(homePageMenu);

            var homePageMenuItemsIds = homePageMenu.Children.Select(mItem => mItem.Id).ToList();
            var leftMenuTree = BuildLeftMenuTree(homePage, homePageMenuItemsIds);
            result.MenuItems.AddRange(leftMenuTree);

            return result;
        }
    }
}
