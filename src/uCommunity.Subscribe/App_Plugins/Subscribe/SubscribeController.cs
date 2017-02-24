using System;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.App_Plugins.Core.Activity;
using uCommunity.Subscribe.App_Plugins.Subscribe.Model;
using uCommunity.Core.App_Plugins.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Subscribe.App_Plugins.Subscribe
{
    public class SubscribeController : SurfaceController
    {
        private readonly ISubscribeService _subscribeService;
        private readonly IIntranetUserService _intranetUserService;

        public SubscribeController(
            ISubscribeService subscribeService,
            IIntranetUserService intranetUserService)
        {
            _subscribeService = subscribeService;
            _intranetUserService = intranetUserService;
        }

        public PartialViewResult Index(Guid activityId, IntranetActivityTypeEnum type)
        {
            var userId = GetCurrentUserId();
            var subscribe = _subscribeService.Get(activityId, userId);

            var model = new SubscribeViewModel
            {
                Id = subscribe?.Id,
                UserId = userId,
                ActivityId = activityId,
                IsSubscribed = subscribe != null,
                Type = type,
                HasNotification = HasNotification(type),
                IsNotificationDisabled = subscribe?.IsNotificationDisabled ?? false
            };

            return PartialView("~/App_Plugins/Subscribe/View/SubscribeView.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult Subscribe(Guid activityId, IntranetActivityTypeEnum type)
        {
            var userId = GetCurrentUserId();
            var subscribe = _subscribeService.Subscribe(userId, activityId);
            var model = new SubscribeViewModel
            {
                Id = subscribe.Id,
                UserId = userId,
                ActivityId = activityId,
                IsSubscribed = true,
                Type = type,
                HasNotification = HasNotification(type),
                IsNotificationDisabled = subscribe.IsNotificationDisabled
            };

            //FillActivityCache(activityId, type);
            return PartialView("~/App_Plugins/Subscribe/View/SubscribeView.cshtml", model);
        }

        [HttpPost]
        public PartialViewResult Unsubscribe(Guid activityId, IntranetActivityTypeEnum type)
        {
            var userId = GetCurrentUserId();
            _subscribeService.Unsubscribe(userId, activityId);
            var model = new SubscribeViewModel
            {
                UserId = userId,
                ActivityId = activityId,
                IsSubscribed = false,
                Type = type,
                HasNotification = HasNotification(type)
            };

            //FillActivityCache(activityId, type);
            return PartialView("~/App_Plugins/Subscribe/View/SubscribeView.cshtml", model);
        }

        public PartialViewResult Overview(Guid activityId)
        {
            return PartialView("~/App_Plugins/Subscribe/View/SubscribersOverView.cshtml", new SubscribeOverviewModel { ActivityId = activityId });
        }

        public PartialViewResult List(Guid activityId)
        {
            var subscribs = _subscribeService.Get(activityId).ToList();

            var subscribersNames = subscribs.Count > 0
                ? _intranetUserService.GetFullNamesByIds(subscribs.Select(s => s.UserId))
                : Enumerable.Empty<string>();

            return PartialView("~/App_Plugins/Subscribe/View/SubscribersList.cshtml", subscribersNames);
        }

        [HttpPost]
        public void ChangeNotificationDisabled(SubscribeNotificationDisableUpdateModel model)
        {
            _subscribeService.UpdateNotificationDisabled(model.Id, model.NewValue);
        }

        public JsonResult Version(Guid activityId)
        {
            var version = _subscribeService.GetVersion(activityId);
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }

        private Guid GetCurrentUserId()
        {
            return _intranetUserService.GetCurrentUserId();
        }
        

        private bool HasNotification(IntranetActivityTypeEnum type)
        {
            return type == IntranetActivityTypeEnum.Events;
        }
    }
}