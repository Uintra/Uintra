using System;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Subscribe
{
    public class SubscribeController : SurfaceController
    {
        private readonly ISubscribeService _subscribeService;
        private readonly IIntranetUserService _intranetUserService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public SubscribeController(
            ISubscribeService subscribeService,
            IIntranetUserService intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _subscribeService = subscribeService;
            _intranetUserService = intranetUserService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public PartialViewResult Index(ISubscribable subscribe, Guid activityId)
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var subscriber = subscribe.Subscribers.SingleOrDefault(s => s.UserId == userId);

            return Index(activityId, subscriber, subscribe.Type);
        }

        public PartialViewResult Overview(Guid activityId)
        {
            return PartialView("~/App_Plugins/Subscribe/View/SubscribersOverView.cshtml", new SubscribeOverviewModel { ActivityId = activityId });
        }

        public PartialViewResult List(Guid activityId)
        {
            var subscribs = _subscribeService.Get(activityId).ToList();

            var subscribersNames = subscribs.Count > 0
                ? _intranetUserService.GetMany(subscribs.Select(s => s.UserId)).Select(u => u.DisplayedName)
                : Enumerable.Empty<string>();

            return PartialView("~/App_Plugins/Subscribe/View/SubscribersList.cshtml", subscribersNames);
        }

        [HttpPost]
        public PartialViewResult Subscribe(Guid activityId, IntranetActivityTypeEnum type)
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var service = _activitiesServiceFactory.GetService(type);
            var subscribeService = (ISubscribableService)service;
            var subscribe = subscribeService.Subscribe(userId, activityId);

            return Index(activityId, subscribe);
        }

        [HttpPost]
        public PartialViewResult Unsubscribe(Guid activityId)
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var service = _activitiesServiceFactory.GetService(activityId);
            var subscribeService = (ISubscribableService)service;
            subscribeService.UnSubscribe(userId, activityId);

            return Index(activityId, null);
        }

        [HttpPost]
        public void ChangeNotificationDisabled(SubscribeNotificationDisableUpdateModel model)
        {
            var service = _activitiesServiceFactory.GetService(model.Type);
            var subscribeService = (ISubscribableService)service;
            subscribeService.UpdateNotification(model.Id, model.NewValue);
        }

        public JsonResult Version(Guid activityId)
        {
            var version = _subscribeService.GetVersion(activityId);
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }

        private PartialViewResult Index(Guid activityId, Subscribe subscriber, IntranetActivityTypeEnum? type = null)
        {
            var model = new SubscribeViewModel
            {
                Id = subscriber?.Id,
                UserId = _intranetUserService.GetCurrentUser().Id,
                ActivityId = activityId,
                IsSubscribed = subscriber != null,
                IsNotificationDisabled = subscriber?.IsNotificationDisabled ?? false
            };

            if (type.HasValue)
            {
                model.HasNotification = _subscribeService.HasNotification(type.Value);
            }

            return PartialView("~/App_Plugins/Subscribe/View/SubscribeView.cshtml", model);
        }
    }
}