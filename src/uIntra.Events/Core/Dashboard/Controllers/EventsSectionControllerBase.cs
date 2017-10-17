using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using uIntra.Core.Extensions;
using uIntra.Core.Media;
using uIntra.Core.User;
using Umbraco.Web.WebApi;

namespace uIntra.Events.Dashboard
{
    public abstract class EventsSectionControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IEventsService<EventBase> _eventsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMediaHelper _mediaHelper;

        protected EventsSectionControllerBase(IEventsService<EventBase> eventsService, IIntranetUserService<IIntranetUser> intranetUserService, IMediaHelper mediaHelper)
        {
            _eventsService = eventsService;
            _intranetUserService = intranetUserService;
            _mediaHelper = mediaHelper;
        }

        public virtual IEnumerable<EventBackofficeViewModel> GetAll()
        {
            var events = _eventsService.GetAll(true);
            foreach (var @event in events)
            {
                @event.CreatorId = _intranetUserService.Get(@event).Id;
            }

            var result = events.Map<IEnumerable<EventBackofficeViewModel>>();
            return result;
        }

        [HttpPost]
        public virtual EventBackofficeViewModel Create(EventBackofficeCreateModel createModel)
        {
            var eventId = _eventsService.Create(createModel.Map<EventBase>());
            var createdModel = _eventsService.Get(eventId);
            var result = createdModel.Map<EventBackofficeViewModel>();
            result.CreatorId = _intranetUserService.Get(createdModel).Id;
            return result;
        }

        [HttpPost]
        public virtual EventBackofficeViewModel Save(EventBackofficeSaveModel saveModel)
        {
            var @event = _eventsService.Get(saveModel.Id);
            @event = Mapper.Map(saveModel, @event);
            _eventsService.Save(@event);
            _mediaHelper.RestoreMedia(@event.MediaIds);

            var updatedModel = _eventsService.Get(saveModel.Id);
            var result = updatedModel.Map<EventBackofficeViewModel>();
            result.CreatorId = _intranetUserService.Get(updatedModel).Id;
            return result;
        }

        [HttpDelete]
        public virtual void Delete(Guid id)
        {
            _eventsService.Delete(id);
        }
    }
}