using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.CentralFeed.Models;
using uCommunity.Core.Activity;
using Umbraco.Web.Mvc;

namespace uCommunity.CentralFeed
{
    public class CentralFeedController : SurfaceController
    {
        private readonly ICentralFeedService _centralFeedService;
        private const int ItemsPerPage = 8;

        public CentralFeedController(ICentralFeedService centralFeedService)
        {
            _centralFeedService = centralFeedService;
        }

        public ActionResult Overview()
        {
            var model = new CentralFeedOverviewModel
            {
                Types = GetTypes(),
            };
            return PartialView("~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml", model);
        }

        public ActionResult List(IntranetActivityTypeEnum? type = null, long? version = null, int page = 1)
        {
            var items = (type == null ? 
                _centralFeedService.GetFeed().OrderByDescending(s => s.SortDate.Date) : 
                _centralFeedService.GetFeed(type.Value))
                .ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(items);

            if (version.HasValue && currentVersion == version.Value)
            {
                return null;
            }

            var take = page * ItemsPerPage;
            var pagedItemsList = items.Take(take).ToList();

            var centralFeedModel = new CentralFeedListModel
            {
                Version = _centralFeedService.GetFeedVersion(items),
                Items = pagedItemsList,
                Type = type,
                BlockScrolling = items.Count < take
            };

            return PartialView("~/App_Plugins/CentralFeed/View/CentralFeedList.cshtml", centralFeedModel);
        }

        public ActionResult Tabs()
        {
            return PartialView("~/App_Plugins/CentralFeed/View/Navigation.cshtml", GetTypes().ToList());
        }

        public JsonResult CacheVersion()
        {
            var version = _centralFeedService.GetFeedVersion(Enumerable.Empty<ICentralFeedItem>());
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<CentralFeedTypeModel> GetTypes()
        {
            foreach (var type in new[] { IntranetActivityTypeEnum.News, IntranetActivityTypeEnum.Ideas, IntranetActivityTypeEnum.Events })
            {
                var settings = _centralFeedService.GetSettings(type);
                yield return new CentralFeedTypeModel
                {
                    Type = type,
                    CreateUrl = settings.CreatePage.Url,
                    TabUrl = settings.OverviewPage.Url,
                    HasSubscribersFilter = settings.HasSubscribersFitler,
                };
            }
        }
    }
}