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
        private readonly IEventsService<EventBase> _eventsService;

        public EventsSectionController(
            IEventsService<EventBase> eventsService,
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

        [HttpPost]
        public override EventBackofficeViewModel Create(EventBackofficeCreateModel createModel)
        {
            //TODO Hotfix. Need to find correct map for EventBackofficeCreateModel => Event via createModel.Map<EventBase> => return Event (not EventBase type).
            //I will find solution later.
            var creatingEvent = createModel.Map<Event>();
            creatingEvent.CreatorId = _intranetUserService.GetCurrentUserId();
            var eventId = _eventsService.Create(creatingEvent);
        
            var createdEvent = _eventsService.Get(eventId);
            var result = createdEvent.Map<EventBackofficeViewModel>();
            return result;
        }
    }
}
