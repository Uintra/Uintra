using System;
using System.Collections.Generic;
using System.Web.Http;
using uCommunity.Core.Extentions;
using Umbraco.Web.WebApi;

namespace uCommunity.Events.Dashboard
{
    public class EventsSectionController : UmbracoAuthorizedApiController
    {
        private readonly IEventsService<EventBase, EventModelBase> _eventsService;

        public EventsSectionController(IEventsService<EventBase, EventModelBase> eventsService)
        {
            _eventsService = eventsService;
        }

        public IEnumerable<EventBackofficeViewModel> GetAll()
        {
            var events = _eventsService.GetAll(true);
            var result = events.Map<IEnumerable<EventBackofficeViewModel>>();
            return result;
        }

        [HttpPost]
        public EventBackofficeViewModel Create(EventBackofficeCreateModel createModel)
        {
            var eventId = _eventsService.Create(createModel.Map<EventModelBase>());
            var createdModel = _eventsService.Get(eventId);
            var result = createdModel.Map<EventBackofficeViewModel>();
            return result;
        }

        [HttpPost]
        public EventBackofficeViewModel Save(EventBackofficeSaveModel saveModel)
        {
            _eventsService.Save(saveModel.Map<EventModelBase>());
            var updatedModel = _eventsService.Get(saveModel.Id);
            var result = updatedModel.Map<EventBackofficeViewModel>();
            return result;
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            _eventsService.Delete(id);
        }
    }
}