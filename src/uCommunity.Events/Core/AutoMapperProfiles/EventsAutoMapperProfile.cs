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
            Mapper.CreateMap<EventModelBase, EventsOverviewItemViewModel>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore());

            Mapper.CreateMap<EventBase, EventCreateModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<EventModelBase, EventEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore())
                .ForMember(dst => dst.NotifyAllSubscribers, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<EventCreateModel, EventModelBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.Teaser, o => o.Ignore());

            Mapper.CreateMap<EventEditModel, EventModelBase>()
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
                .ForMember(dst => dst.Teaser, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            Mapper.CreateMap<EventModelBase, EventViewModel>()
                .ForMember(dst => dst.OverviewPageUrl, o => o.Ignore())
                .ForMember(dst => dst.EditPageUrl, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<EventModelBase, EventBackofficeViewModel>()
                .ForMember(d => d.Media, o => o.MapFrom(s => StringExtentions.JoinToString(s.MediaIds, ",")));

            Mapper.CreateMap<EventModelBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.ToString(IntranetConstants.Common.DefaultDateFormat), el.EndDate.ToString(IntranetConstants.Common.DefaultDateFormat) }));

            Mapper.CreateMap<EventModelBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<EventModelBase, IntranetActivityDetailsHeaderViewModel>();

            Mapper.CreateMap<EventBackofficeCreateModel, EventModelBase>()
               .ForMember(d => d.MediaIds, o => o.Ignore())
               .ForMember(d => d.Type, o => o.Ignore())
               .ForMember(d => d.CreatorId, o => o.Ignore())
               .ForMember(d => d.Id, o => o.Ignore())
               .ForMember(d => d.CreatedDate, o => o.Ignore())
               .ForMember(d => d.ModifyDate, o => o.Ignore())
               .ForMember(d => d.Creator, o => o.Ignore())
               .AfterMap((dst, src) =>
               {
                   src.MediaIds = dst.Media.ToIntCollection();
               });

            Mapper.CreateMap<EventBackofficeSaveModel, EventModelBase>()
                .ForMember(d => d.MediaIds, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.CreatorId, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.ModifyDate, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .AfterMap((dst, src) =>
                {
                    src.MediaIds = dst.Media.ToIntCollection();
                });
        }
    }
}