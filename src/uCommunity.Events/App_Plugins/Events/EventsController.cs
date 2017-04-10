using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Events.Web;

namespace uCommunity.Events.App_Plugins.Events
{
    public class EventsController: EventsControllerBase
    {
        public EventsController(IEventsService<EventBase, EventModelBase> eventsService, 
            IMediaHelper mediaHelper,
            IIntranetUserService intranetUserService) 
            : base(eventsService, mediaHelper, intranetUserService)
        {
        }

        //public ActionResult CentralFeedItem(ICentralFeedItem item)
        //{
        //    FillLinks();
        //    var activity = item as EventModelBase;
        //    return PartialView("~/App_Plugins/Events/List/ItemView.cshtml", GetOverviewItems(Enumerable.Repeat(activity, 1)).Single());
        //}
    }
}