using System;
using System.Linq;
using System.Web.Mvc;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Umbraco.Web.Mvc;

namespace Uintra.Subscribe.Web
{
    public abstract class SubscribeControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Subscribe/View/SubscribersOverView.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/Subscribe/View/SubscribersList.cshtml";
        protected virtual string IndexViewPath { get; } = "~/App_Plugins/Subscribe/View/SubscribeView.cshtml";
        protected virtual string ActivitySubscribeSettingsViewPath { get; } = "~/App_Plugins/Subscribe/View/ActivitySubscribeSettingsView.cshtml";

        private readonly ISubscribeService _subscribeService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        protected SubscribeControllerBase(
            ISubscribeService subscribeService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _subscribeService = subscribeService;
            _intranetUserService = intranetUserService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public virtual PartialViewResult Index(ISubscribable subscribe, Guid activityId)
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var subscriber = subscribe.Subscribers.SingleOrDefault(s => s.UserId == userId);

            return Index(activityId, subscriber, subscribe.Type);
        }

        public virtual PartialViewResult Overview(Guid activityId)
        {
            return PartialView(OverviewViewPath, new SubscribeOverviewModel { ActivityId = activityId });
        }

        public virtual PartialViewResult List(Guid activityId)
        {
            var subscribs = _subscribeService.Get(activityId).ToList();

            var subscribersNames = subscribs.Count > 0
                ? _intranetUserService.GetMany(subscribs.Select(s => s.UserId)).Select(u => u.DisplayedName)
                : Enumerable.Empty<string>();

            return PartialView(ListViewPath, subscribersNames);
        }

        public virtual ActionResult ActivitySubscribeSettings(ActivitySubscribeSettingsModel model)
        {
            return PartialView(ActivitySubscribeSettingsViewPath, model);
        }

        [HttpPost]
        public virtual PartialViewResult Subscribe(Guid activityId)
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var service = _activitiesServiceFactory.GetService<ISubscribableService>(activityId);
            var subscribable = service.Subscribe(userId, activityId);
            var subscribe = subscribable.Subscribers.Single(s => s.UserId == userId);

            return Index(activityId, subscribe, subscribable.Type);
        }

        [HttpPost]
        public virtual PartialViewResult Unsubscribe(Guid activityId)
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var service = _activitiesServiceFactory.GetService<ISubscribableService>(activityId);
            service.UnSubscribe(userId, activityId);

            return Index(activityId, null);
        }

        [HttpPost]
        public virtual void ChangeNotificationDisabled(SubscribeNotificationDisableUpdateModel model)
        {
            var service = _activitiesServiceFactory.GetService<ISubscribableService>(model.ActivityId);
            service.UpdateNotification(model.Id, model.NewValue);
        }

        public virtual JsonResult Version(Guid activityId)
        {
            var version = _subscribeService.GetVersion(activityId);
            return Json(new { Result = version }, JsonRequestBehavior.AllowGet);
        }

        protected virtual PartialViewResult Index(Guid activityId, Subscribe subscriber, Enum type = null)
        {
            var model = new SubscribeViewModel
            {
                Id = subscriber?.Id,
                UserId = _intranetUserService.GetCurrentUserId(),
                ActivityId = activityId,
                IsSubscribed = subscriber != null,
                IsNotificationDisabled = subscriber?.IsNotificationDisabled ?? false
            };

            if (type != null)
            {
                model.HasNotification = _subscribeService.HasNotification(type);
            }

            return PartialView(IndexViewPath, model);
        }
    }
}