using System;
using System.Web.Http;
using Compent.Uintra.Core.Events;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Core.User;
using Uintra.Events;
using Uintra.Events.Dashboard;
using Uintra.Navigation;

namespace Compent.Uintra.Controllers
{
    public class EventsSectionController : EventsSectionControllerBase
    {
        private readonly IMyLinksService _myLinksService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IEventsService<Event> _eventsService;

        public EventsSectionController(
            IEventsService<Event> eventsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IMediaHelper mediaHelper,
            IMyLinksService myLinksService)
            : base(eventsService, intranetUserService, mediaHelper)
        {
            _myLinksService = myLinksService;
            _intranetUserService = intranetUserService;
            _eventsService = eventsService;
        }

        [HttpDelete]
        public override void Delete(Guid id)
        {
            base.Delete(id);
            _myLinksService.DeleteByActivityId(id);
        }

        protected override EventBase MapToEvent(EventBackofficeCreateModel model)
        {
            var @event = model.Map<Event>();
            @event.CreatorId = _intranetUserService.GetCurrentUserId();
            return @event;
        }
    }
}
