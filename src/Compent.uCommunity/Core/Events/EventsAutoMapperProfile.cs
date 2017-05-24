using AutoMapper;
using uCommunity.Core.Activity.Models;
using uCommunity.Events;

namespace Compent.uCommunity.Core.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Event, EventExtendedViewModel>()
                  .IncludeBase<EventBase, EventViewModel>()
                  .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el))
                  .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, EventOverviewItemModel>()
                .IncludeBase<EventBase, EventItemViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Event, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<EventBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<Event, EventExtendedActivityCreateModel>()
                .IncludeBase<EventBase, EventCreateModel>()
                .ForMember(dst => dst.Tags, o => o.Ignore());

            Mapper.CreateMap<Event, EventExtendedEditModel>()
                .IncludeBase<EventBase, EventEditModel>()
                .ForMember(dst => dst.Tags, o => o.MapFrom(el => el.Tags));

            Mapper.CreateMap<EventEditModel, Event>()
                .IncludeBase<EventEditModel, EventBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Likes, o => o.Ignore())
                .ForMember(dst => dst.Comments, o => o.Ignore())
                .ForMember(dst => dst.Subscribers, o => o.Ignore())
                .ForMember(dst => dst.Tags, o => o.Ignore());

            Mapper.CreateMap<EventCreateModel, Event>()
                .IncludeBase<EventCreateModel, EventBase>()
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Likes, o => o.Ignore())
                .ForMember(dst => dst.Comments, o => o.Ignore())
                .ForMember(dst => dst.Subscribers, o => o.Ignore())
                .ForMember(dst => dst.Tags, o => o.Ignore());
        }
    }
}