using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;

namespace uCommunity.Navigation.Core
{
    public class MyLinksModelBuilder : IMyLinksModelBuilder
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMyLinksService _myLinksService;

        public MyLinksModelBuilder(
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMyLinksService myLinksService)
        {
            _intranetUserService = intranetUserService;
            _myLinksService = myLinksService;
        }

        public MyLinksModel Get()
        {
            var result = new MyLinksModel();
            var currentUser = _intranetUserService.GetCurrentUser();

            var myLinks = _myLinksService.GetUserLinks(currentUser.Id);
            result.MyLinks = myLinks.Map<IEnumerable<MyLinkItemModel>>().ToList();

            return result;
        }
    }
}
