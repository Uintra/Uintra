using System;
using System.Web.Http;
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

        public EventsSectionController(IEventsService<EventBase> eventsService,
            IIntranetUserService<IIntranetUser> intranetUserService, IMediaHelper mediaHelper, IMyLinksService myLinksService)
            : base(eventsService, intranetUserService, mediaHelper)
        {
            _myLinksService = myLinksService;
        }

        [HttpDelete]
        public override void Delete(Guid id)
        {
            base.Delete(id);
            _myLinksService.DeleteByActivityId(id);
        }
    }
}
