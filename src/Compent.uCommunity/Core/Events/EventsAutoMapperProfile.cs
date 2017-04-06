using AutoMapper;
using uCommunity.Core.Activity.Models;
using uCommunity.Events;

namespace Compent.uCommunity.Core.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Event, IntranetActivityHeaderModel>()
                 .IncludeBase<EventModelBase, IntranetActivityHeaderModel>();
        }
    }
}