using System;
using System.Collections.Generic;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Features.Media;
using Uintra.Features.Media.Models;

namespace Uintra.Features.Events
{
    public interface IEventsService<out TEvent> : IIntranetActivityService<TEvent> where TEvent : EventBase
    {
        IEnumerable<TEvent> GetPastEvents();

        IEnumerable<TEvent> GetComingEvents(DateTime fromDate);

        bool CanHide(Guid id);

        bool CanHide(IIntranetActivity activity);

        void Hide(Guid id);

        bool CanEditSubscribe(Guid activityId);

        bool CanSubscribe(Guid activityId);

        MediaSettings GetMediaSettings();
    }
}
