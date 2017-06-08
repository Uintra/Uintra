using System;
using System.Collections.Generic;
using System.Web.Http;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.Events.Dashboard
{
    public class EventsSectionController : UmbracoAuthorizedApiController
    {
        private readonly IEventsService<EventBase> _eventsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public EventsSectionController(IEventsService<EventBase> eventsService, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _eventsService = eventsService;
            _intranetUserService = intranetUserService;
        }

        public IEnumerable<EventBackofficeViewModel> GetAll()
        {
            var events = _eventsService.GetAll(true);
            foreach (var @event in events)
            {
                @event.CreatorId = _intranetUserService.GetCreator(@event).Id;
            }

            var result = events.Map<IEnumerable<EventBackofficeViewModel>>();
            return result;
        }

        [HttpPost]
        public EventBackofficeViewModel Create(EventBackofficeCreateModel createModel)
        {
            var eventId = _eventsService.Create(createModel.Map<EventBase>());
            var createdModel = _eventsService.Get(eventId);
            var result = createdModel.Map<EventBackofficeViewModel>();
            result.CreatorId = _intranetUserService.GetCreator(createdModel).Id;
            return result;
        }

        [HttpPost]
        public EventBackofficeViewModel Save(EventBackofficeSaveModel saveModel)
        {
            _eventsService.Save(saveModel.Map<EventBase>());
            var updatedModel = _eventsService.Get(saveModel.Id);
            var result = updatedModel.Map<EventBackofficeViewModel>();
            result.CreatorId = _intranetUserService.GetCreator(updatedModel).Id;
            return result;
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _eventsService.Delete(id);
        }
    }
}