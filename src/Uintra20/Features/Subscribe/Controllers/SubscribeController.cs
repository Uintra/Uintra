using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Subscribe.Controllers
{
    public class SubscribeController : UBaselineApiController
    {
        private readonly ISubscribeService _subscribeService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        protected SubscribeController(
            ISubscribeService subscribeService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _subscribeService = subscribeService;
            _intranetMemberService = intranetMemberService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        [HttpGet]
        public virtual IEnumerable<string> List(Guid activityId)
        {
            var subscribs = _subscribeService.Get(activityId).ToList();

            var subscribersNames = subscribs.Count > 0
                ? _intranetMemberService.GetMany(subscribs.Select(s => s.UserId)).Select(u => u.DisplayedName)
                : Enumerable.Empty<string>();

            return subscribersNames;
        }

        [HttpPost]
        public virtual IEnumerable<string> Subscribe(Guid activityId)
        {
            var userId = _intranetMemberService.GetCurrentMemberId();
            var service = _activitiesServiceFactory.GetService<ISubscribableService>(activityId);
            service.Subscribe(userId, activityId);

            return List(activityId);
        }

        [HttpPost]
        public virtual IEnumerable<string> Unsubscribe(Guid activityId)
        {
            var userId = _intranetMemberService.GetCurrentMemberId();
            var service = _activitiesServiceFactory.GetService<ISubscribableService>(activityId);
            service.UnSubscribe(userId, activityId);

            return List(activityId);
        }

        [HttpPost]
        public virtual IHttpActionResult ChangeNotificationDisabled(Guid activityId, bool value)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            var subscribeService = (ISubscribableService) service;
            var currentUserId = _intranetMemberService.GetCurrentMemberId();

            var activity = (ISubscribable)service.Get(activityId);
            var subscribe = activity.Subscribers.FirstOrDefault(x => x.UserId == currentUserId);

            if (subscribe == null)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            subscribeService.UpdateNotification(subscribe.Id, value);

            return Ok();
        }
    }
}