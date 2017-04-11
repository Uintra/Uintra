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
                  .IncludeBase<EventModelBase, IntranetEventViewModel>()
                  .IncludeBase<EventModelBase, EventViewModel>()
                  .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, EventOverviewItemModel>()
                .IncludeBase<EventModelBase, EventOverviewItemModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<EventModelBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<EventEditModel, Event>()
                .IncludeBase<EventEditModel, EventModelBase>()
                .ForMember(dst => dst.Type, o => o.Ignore());

            Mapper.CreateMap<EventCreateModel, Event>()
              .IncludeBase<EventCreateModel, EventModelBase>()
              .ForMember(dst => dst.Type, o => o.Ignore());
        }
    }
}