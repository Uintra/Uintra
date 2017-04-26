using AutoMapper;
using uCommunity.Core.Activity.Models;
using uCommunity.Events;

namespace Compent.uCommunity.Core.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Event, IntranetEventViewModel>()
                  .IncludeBase<EventBase, IntranetEventViewModel>()
                  .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, EventOverviewItemModel>()
                .IncludeBase<EventBase, EventOverviewItemModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<EventBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<EventEditModel, Event>()
                .ForMember(dst => dst.Type, o => o.Ignore());

            Mapper.CreateMap<EventCreateModel, Event>()
              .ForMember(dst => dst.Type, o => o.Ignore());
        }
    }
}