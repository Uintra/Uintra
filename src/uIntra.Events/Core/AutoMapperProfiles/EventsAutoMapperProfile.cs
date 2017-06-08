using System.Collections.Generic;
using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Events.Dashboard;

namespace uIntra.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<EventBase, ComingEventViewModel>();

            Mapper.CreateMap<EventBase, EventItemViewModel>()
                .ForMember(dst => dst.ShortDescription, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore());

            Mapper.CreateMap<EventBase, EventCreateModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Users, o => o.Ignore())
                .ForMember(dst => dst.CanEditCreator, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, EventEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore())
                .ForMember(dst => dst.NotifyAllSubscribers, o => o.Ignore())
                .ForMember(dst => dst.Users, o => o.Ignore())
                .ForMember(dst => dst.CanEditCreator, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventCreateModel, EventBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .AfterMap((src, dst) =>
                 {
                     dst.StartDate = src.StartDate.ToUniversalTime();
                     dst.EndDate = src.EndDate.ToUniversalTime();
                 });

            Mapper.CreateMap<EventEditModel, EventBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                    dst.StartDate = src.StartDate.ToUniversalTime();
                    dst.EndDate = src.EndDate.ToUniversalTime();
                });

            Mapper.CreateMap<EventBase, EventViewModel>()
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, EventBackofficeViewModel>()
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate.ToIsoUtcString()))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate.ToIsoUtcString()))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate.ToIsoUtcString()))
                .ForMember(d => d.ModifyDate, o => o.MapFrom(s => s.ModifyDate.ToIsoUtcString()))
                .ForMember(dst => dst.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.ToDateTimeFormat(), el.EndDate.ToDateTimeFormat() }));

            Mapper.CreateMap<EventBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id));

            Mapper.CreateMap<EventBackofficeCreateModel, EventBase>()
               .ForMember(dst => dst.MediaIds, o => o.Ignore())
               .ForMember(dst => dst.Type, o => o.Ignore())
               .ForMember(dst => dst.Id, o => o.Ignore())
               .ForMember(dst => dst.CreatedDate, o => o.Ignore())
               .ForMember(dst => dst.ModifyDate, o => o.Ignore())
               .ForMember(dst => dst.Creator, o => o.Ignore())
               .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
               .ForMember(dst => dst.IsPinned, o => o.Ignore())
               .ForMember(dst => dst.EndPinDate, o => o.Ignore())
               .AfterMap((src, dst) =>
               {
                   dst.MediaIds = src.Media.ToIntCollection();
               });

            Mapper.CreateMap<EventBackofficeSaveModel, EventBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())

                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });
        } 
    }
}