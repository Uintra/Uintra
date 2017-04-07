//using AutoMapper;
//using uCommunity.Core.Activity.Models;
//using uCommunity.Events;

//namespace Compent.uCommunity.Core.Events
//{
//    public class EventsAutoMapperProfile : Profile
//    {
//        protected override void Configure()
//        {
//            Mapper.CreateMap<Event, EventViewModel>()
//                  .IncludeBase<EventModelBase, EventViewModel>()
//                  .IncludeBase<EventModelBase, EventViewModelBase>()
//                  .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
//                  .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

//            Mapper.CreateMap<Event, EventOverviewItemModel2>()
//                .IncludeBase<EventModelBase, EventOverviewItemModel2>()
//                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el));

//            Mapper.CreateMap<Event, IntranetActivityHeaderModel>()
//                 .IncludeBase<EventModelBase, IntranetActivityHeaderModel>();
//        }
//    }
//}