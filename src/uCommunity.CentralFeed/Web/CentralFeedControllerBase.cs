using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Enums;
using uCommunity.CentralFeed.Models;
using uCommunity.Core.Activity;
using uCommunity.Core.Extentions;
using Umbraco.Web.Mvc;

namespace uCommunity.CentralFeed.Web
{
    public abstract class CentralFeedControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/CentralFeed/View/CentralFeedOverView.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/CentralFeed/View/CentralFeedList.cshtml";
        protected virtual string NavigationViewPath { get; } = "~/App_Plugins/CentralFeed/View/Navigation.cshtml";

        protected readonly ICentralFeedService _centralFeedService;
        protected readonly ICentralFeedContentHelper _centralFeedContentHelper;
        protected const int ItemsPerPage = 8;

        protected CentralFeedControllerBase(ICentralFeedService centralFeedService, ICentralFeedContentHelper centralFeedContentHelper)
        {
            _centralFeedService = centralFeedService;
            _centralFeedContentHelper = centralFeedContentHelper;
        }

        public virtual ActionResult Overview()
        {
            var tabType = _centralFeedContentHelper.GetTabType(CurrentPage);
            var model = new CentralFeedOverviewModel
            {
                CurrentType = tabType
            };
            return PartialView(OverviewViewPath, model);
        }

        public virtual ActionResult List(CentralFeedListModel model)
        {
            var items = GetCentralFeedItems(model.Type.GetHashCode().ToEnum<IntranetActivityTypeEnum>());

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

        public virtual JsonResult AvailableActivityTypes()
        {
            var activityTypes = _centralFeedService.GetAllSettings().Select(s => (CentralFeedTypeEnum)s.Type);
            var activityTypeModelList = activityTypes.Select(a => new { Id = a.GetHashCode(), Name = a.ToString() }).ToList();
            activityTypeModelList.Insert(0, new { Id = CentralFeedTypeEnum.All.GetHashCode(), Name = CentralFeedTypeEnum.All.ToString() });

            return Json(activityTypeModelList, JsonRequestBehavior.AllowGet);
        }

        protected virtual List<ICentralFeedItem> GetCentralFeedItems(IntranetActivityTypeEnum? type)
        {
            if (type == null)
            {
                var items = _centralFeedService.GetFeed().ToList();
                items.Sort(new CentralFeedComparer());
                return items;
            }

            return _centralFeedService.GetFeed(type.Value).ToList();
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