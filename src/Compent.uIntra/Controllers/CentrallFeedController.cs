using System.Linq;
using System.Web.Mvc;
using uIntra.CentralFeed;
using uIntra.CentralFeed.App_Plugins.CentralFeed.Models;
using uIntra.CentralFeed.Web;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using uIntra.Subscribe;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class CentralFeedController : CentralFeedControllerBase
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly ISubscribeService _subscribeService;
        private readonly ICentralFeedService _centralFeedService;

        public CentralFeedController(ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            ISubscribeService subscribeService,
            IActivitiesServiceFactory activitiesServiceFactory)
            : base(centralFeedService, centralFeedContentHelper, activitiesServiceFactory)
        {
            _intranetUserService = intranetUserService;
            _subscribeService = subscribeService;
            _centralFeedService = centralFeedService;
        }

        public override ActionResult List(CentralFeedListModel model)
        {
            var items = GetCentralFeedItems(model.Type.GetHashCode().ToEnum<IntranetActivityTypeEnum>());

            var currentVersion = _centralFeedService.GetFeedVersion(items);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            if (model.ShowSubscribed.GetValueOrDefault())
            {
                items = items.FindAll(i => i is ISubscribable && _subscribeService.IsSubscribed(currentUserId, (ISubscribable)i));
            }

            var take = model.Page * ItemsPerPage;

            var pagedItemsList = items.OrderByDescending(IsPinActual).Take(take).ToList();            

            var centralFeedModel = new CentralFeedListViewModel
            {
                Version = _centralFeedService.GetFeedVersion(items),
                Items = pagedItemsList,
                Settings = _centralFeedService.GetAllSettings(),
                Type = model.Type,
                BlockScrolling = items.Count < take,
                ShowSubscribed = model.ShowSubscribed ?? false
            };

            return PartialView(ListViewPath, centralFeedModel);
        }

       
    }
}