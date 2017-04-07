using AutoMapper;
using uCommunity.Core.Activity.Models;
using uCommunity.Events;

namespace Compent.uCommunity.Core.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Event, EventViewModel>()
                  .IncludeBase<EventModelBase, EventViewModel>()
                  .IncludeBase<EventModelBase, EventViewModelBase>()
                  .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, EventOverviewItemModel>()
                .IncludeBase<EventModelBase, EventOverviewItemModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, IntranetActivityHeaderModel>()
                 .IncludeBase<EventModelBase, IntranetActivityHeaderModel>();

            Mapper.CreateMap<EventEditModel, Event>()
                .IncludeBase<EventEditModel, EventModelBase>()
                .ForMember(dst => dst.Type, o => o.Ignore());

            Mapper.CreateMap<EventCreateModel, Event>()
              .IncludeBase<EventCreateModel, EventModelBase>()
              .ForMember(dst => dst.Type, o => o.Ignore());
        }
    }
}