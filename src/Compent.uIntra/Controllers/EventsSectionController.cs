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
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public EventsSectionController(
            IEventsService<Event> eventsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IMediaHelper mediaHelper,
            IMyLinksService myLinksService)
            : base(eventsService, intranetMemberService, mediaHelper)
        {
            _myLinksService = myLinksService;
            _intranetMemberService = intranetMemberService;
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
            @event.CreatorId = _intranetMemberService.GetCurrentMemberId();
            return @event;
        }
    }
}
