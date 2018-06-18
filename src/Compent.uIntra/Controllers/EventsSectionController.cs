using System;
using System.Web.Http;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Events.Dashboard;
using uIntra.Navigation;

namespace Compent.uIntra.Controllers
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
