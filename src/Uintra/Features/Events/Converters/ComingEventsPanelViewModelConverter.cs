using System;
using System.Linq;
using UBaseline.Core.Node;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Events.Entities;
using Uintra.Features.Events.Models;
using Uintra.Features.Links;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Events.Converters
{
    public class ComingEventsPanelViewModelConverter : INodeViewModelConverter<ComingEventsPanelModel, ComingEventsPanelViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IEventsService<Event> _eventsService;
        private readonly IActivityLinkService _activityLinkService;

        public ComingEventsPanelViewModelConverter(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IEventsService<Event> eventsService,
            IActivityLinkService activityLinkService)
        {
            _intranetMemberService = intranetMemberService;
            _eventsService = eventsService;
            _activityLinkService = activityLinkService;
        }

        public void Map(ComingEventsPanelModel node, ComingEventsPanelViewModel viewModel)
        {
            var events = _eventsService.GetComingEvents(DateTime.UtcNow).ToArray();

            var ownersDictionary = _intranetMemberService.GetMany(events.Select(e => e.OwnerId)).ToDictionary(c => c.Id);

            var comingEvents = events
                .Take(node.EventsAmount)
                .Select(@event =>
                {
                    var eventViewModel = @event.Map<ComingEventViewModel>();
                    eventViewModel.Owner = ownersDictionary[@event.OwnerId].ToViewModel();
                    eventViewModel.Links = _activityLinkService.GetLinks(@event.Id);
                    return eventViewModel;
                })
                .ToList();

            viewModel.Events = comingEvents;

        }
    }
}