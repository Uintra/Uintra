using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.CentralFeed.Models;
using Umbraco.Web.Mvc;

namespace uCommunity.CentralFeed
{
    public class CentralFeedControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/CentralFeed/View/CentralFeedList.cshtml";
        protected virtual string NavigationViewPath { get; } = "~/App_Plugins/CentralFeed/View/Navigation.cshtml";

        protected readonly ICentralFeedService _centralFeedService;
        protected const int ItemsPerPage = 8;

        protected CentralFeedControllerBase(ICentralFeedService centralFeedService)
        {
            _centralFeedService = centralFeedService;
        }

        public virtual ActionResult Overview()
        {
            var model = new CentralFeedOverviewModel
            {
                Types = GetTypes(),
            };
            return PartialView(OverviewViewPath, model);
        }

        public virtual ActionResult List(CentralFeedListModel model)
        {
            var items = (model.Type == null ?
                _centralFeedService.GetFeed().OrderByDescending(s => s.SortDate.Date) :
                _centralFeedService.GetFeed(model.Type.Value))
                .ToList();

            var currentVersion = _centralFeedService.GetFeedVersion(items);

            if (model.Version.HasValue && currentVersion == model.Version.Value)
            {
                return null;
            }

            var take = model.Page * ItemsPerPage;
            var pagedItemsList = items.Take(take).ToList();

            var centralFeedModel = new CentralFeedListViewModel
            {
                Version = _centralFeedService.GetFeedVersion(items),
                Items = pagedItemsList,
                Settings = _centralFeedService.GetAllSettings(),
                Type = model.Type,
                BlockScrolling = items.Count < take
            };

            return PartialView(ListViewPath, centralFeedModel);
        }

        public virtual ActionResult Tabs()
        {
            return PartialView(NavigationViewPath, GetTypes().ToList());
        }

        public virtual JsonResult CacheVersion()
        {
            var version = _centralFeedService.GetFeedVersion(Enumerable.Empty<ICentralFeedItem>());
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }

        protected virtual IEnumerable<CentralFeedTypeModel> GetTypes()
        {
            var allSettings = _centralFeedService.GetAllSettings();
            foreach (var singleSetting in allSettings)
            {
                yield return new CentralFeedTypeModel
                {
                    Type = singleSetting.Type,
                    CreateUrl = singleSetting.CreatePage.Url,
                    TabUrl = singleSetting.OverviewPage.Url,
                    HasSubscribersFilter = singleSetting.HasSubscribersFitler,
                };
            }
        }
    }
}