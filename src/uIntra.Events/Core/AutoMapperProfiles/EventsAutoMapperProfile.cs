using System.Collections.Generic;
using AutoMapper;
using Compent.Extensions;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Events.Dashboard;

namespace Uintra.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<EventBase, ComingEventViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.GetEventDateTimeString(el.EndDate) }));

            Mapper.CreateMap<EventBase, EventItemViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            Mapper.CreateMap<EventBase, EventCreateModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.OwnerId, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, EventEditModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.NotifyAllSubscribers, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventCreateModel, EventBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            Mapper.CreateMap<EventEditModel, EventBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            Mapper.CreateMap<EventBase, EventViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, EventBackofficeViewModel>()
                .ForMember(dst => dst.StartDate, o => o.MapFrom(s => s.StartDate.ToIsoUtcString()))
                .ForMember(dst => dst.EndDate, o => o.MapFrom(s => s.EndDate.ToIsoUtcString()))
                .ForMember(dst => dst.PublishDate, o => o.MapFrom(s => s.PublishDate.ToIsoUtcString()))
                .ForMember(dst => dst.CreatedDate, o => o.MapFrom(s => s.CreatedDate.ToIsoUtcString()))
                .ForMember(dst => dst.ModifyDate, o => o.MapFrom(s => s.ModifyDate.ToIsoUtcString()))
                .ForMember(dst => dst.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => el.PublishDate.ToDateTimeFormat().ToEnumerable()));

            Mapper.CreateMap<EventBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id))
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.GetEventDateTimeString(el.EndDate) }));

            Mapper.CreateMap<EventBackofficeCreateModel, EventBase>()
               .ForMember(dst => dst.MediaIds, o => o.Ignore())
               .ForMember(dst => dst.Type, o => o.Ignore())
               .ForMember(dst => dst.Id, o => o.Ignore())
               .ForMember(dst => dst.CreatedDate, o => o.Ignore())
               .ForMember(dst => dst.ModifyDate, o => o.Ignore())
               .ForMember(dst => dst.IsPinned, o => o.Ignore())
               .ForMember(dst => dst.EndPinDate, o => o.Ignore())
               .ForMember(dst => dst.IsPinActual, o => o.Ignore())
               .ForMember(dst => dst.CreatorId, o => o.Ignore())
               .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
               .AfterMap((src, dst) =>
               {
                   dst.MediaIds = src.Media.ToIntCollection();
               });

            Mapper.CreateMap<EventBackofficeSaveModel, EventBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });
        }
    }
}