using System.Collections.Generic;
using AutoMapper;
using uCommunity.Core;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.Events.Dashboard;

namespace uCommunity.Events
{
    public class EventsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<EventBase, EventsOverviewItemViewModel>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore());

            Mapper.CreateMap<EventBase, EventCreateModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, EventEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore())
                .ForMember(dst => dst.NotifyAllSubscribers, o => o.Ignore())
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
                .ForMember(dst => dst.CreatorId, o => o.Ignore());

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
                .ForMember(dst => dst.CreatorId, o => o.Ignore())                
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            Mapper.CreateMap<EventBase, EventViewModel>()
                .ForMember(dst => dst.OverviewPageUrl, o => o.Ignore())
                .ForMember(dst => dst.EditPageUrl, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, EventBackofficeViewModel>()
                .ForMember(d => d.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            Mapper.CreateMap<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat), el.EndDate.ToString(IntranetConstants.Common.DefaultDateTimeFormat) }));

            Mapper.CreateMap<EventBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.DetailsPageUrl, o => o.Ignore());

            Mapper.CreateMap<EventBackofficeCreateModel, EventBase>()
               .ForMember(d => d.MediaIds, o => o.Ignore())
               .ForMember(d => d.Type, o => o.Ignore())
               .ForMember(d => d.CreatorId, o => o.Ignore())
               .ForMember(d => d.Id, o => o.Ignore())
               .ForMember(d => d.CreatedDate, o => o.Ignore())
               .ForMember(d => d.ModifyDate, o => o.Ignore())
               .ForMember(d => d.Creator, o => o.Ignore())
               .ForMember(d => d.CanSubscribe, o => o.Ignore())
               .ForMember(dst => dst.IsPinned, o => o.Ignore())
               .ForMember(dst => dst.PinDays, o => o.Ignore())
               .ForMember(dst => dst.EndPinDate, o => o.Ignore())
               .AfterMap((dst, src) =>
               {
                   src.MediaIds = dst.Media.ToIntCollection();
               });

            Mapper.CreateMap<EventBackofficeSaveModel, EventBase>()
                .ForMember(d => d.MediaIds, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.CreatorId, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.ModifyDate, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .ForMember(d => d.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.PinDays, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .AfterMap((dst, src) =>
                {
                    src.MediaIds = dst.Media.ToIntCollection();
                });
        }
    }
}