using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Uintra.Core.Extensions;
using Uintra.Core.Media;
using Uintra.Core.User;
using Umbraco.Web.WebApi;

namespace Uintra.Events.Dashboard
{
    public abstract class EventsSectionControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IEventsService<EventBase> _eventsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IMediaHelper _mediaHelper;

        protected EventsSectionControllerBase(IEventsService<EventBase> eventsService, IIntranetMemberService<IIntranetMember> intranetMemberService, IMediaHelper mediaHelper)
        {
            _eventsService = eventsService;
            _intranetMemberService = intranetMemberService;
            _mediaHelper = mediaHelper;
        }

        public virtual IEnumerable<EventBackofficeViewModel> GetAll()
        {
            var events = _eventsService.GetAll(true);
            var result = events.Map<IEnumerable<EventBackofficeViewModel>>().OrderByDescending(e => e.ModifyDate);
            return result;
        }

        [HttpPost]
        public virtual EventBackofficeViewModel Create(EventBackofficeCreateModel createModel)
        {
            var creatingEvent = MapToEvent(createModel);
            var eventId = _eventsService.Create(creatingEvent);

            var createdEvent = _eventsService.Get(eventId);
            var result = createdEvent.Map<EventBackofficeViewModel>();
            return result;
        }

        protected virtual EventBase MapToEvent(EventBackofficeCreateModel model)
        {
            var @event = model.Map<EventBase>();
            @event.CreatorId = _intranetMemberService.GetCurrentMemberId();
            return @event;
        }

        [HttpPost]
        public virtual EventBackofficeViewModel Save(EventBackofficeSaveModel saveModel)
        {
            var @event = _eventsService.Get(saveModel.Id);
            @event = Mapper.Map(saveModel, @event);
            _eventsService.Save(@event);
            _mediaHelper.RestoreMedia(@event.MediaIds);

            var updatedEvent = _eventsService.Get(saveModel.Id);
            var result = updatedEvent.Map<EventBackofficeViewModel>();
            return result;
        }

        [HttpDelete]
        public virtual void Delete(Guid id)
        {
            _eventsService.Delete(id);
        }
    }
}