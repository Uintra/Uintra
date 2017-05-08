using System.Linq;
using System.Web.Mvc;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Models;
using uCommunity.CentralFeed.Web;
using uCommunity.Core.Activity;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using uCommunity.Subscribe;
using uCommunity.Users.Core;

namespace Compent.uCommunity.Controllers
{
    public class CentralFeedController : CentralFeedControllerBase
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly ISubscribeService _subscribeService;

        public CentralFeedController(ICentralFeedService centralFeedService,
            ICentralFeedContentHelper centralFeedContentHelper,
            IIntranetUserService<IntranetUser> intranetUserService,
            ISubscribeService subscribeService)
            : base(centralFeedService, centralFeedContentHelper)
        {
            _intranetUserService = intranetUserService;
            _subscribeService = subscribeService;
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
            var pagedItemsList = items.OrderByDescending(el => el.PublishDate).Take(take).ToList();

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