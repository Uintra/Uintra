using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Compent.Extensions;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Events.Models;
using Uintra20.Features.Groups.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Events.AutoMapperProfiles
{
    public class EventsAutoMapperProfile : Profile
    {
        public EventsAutoMapperProfile()
        {
            //Mapper.CreateMap<Event, EventExtendedViewModel>()
            //    .IncludeBase<EventBase, EventViewModel>()
            //    .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
            //    .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el))
            //    .ForMember(dst => dst.SubscribeInfo, o => o.MapFrom(el => el))
            //    .ForMember(dst => dst.SubscribeNotes, o => o.MapFrom(s => s.SubscribeNotes.ReplaceLineBreaksForHtml()));

            //Mapper.CreateMap<EventEditModel, EventExtendedEditModel>()
            //    .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));

            //Mapper.CreateMap<EventCreateModel, EventExtendedCreateModel>()
            //    .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));


            //Mapper.CreateMap<Event, EventExtendedItemModel>()
            //    .IncludeBase<EventBase, EventItemViewModel>()
            //    .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
            //    .ForMember(dst => dst.IsSubscribed, o => o.Ignore());

            //Mapper.CreateMap<Event, EventExtendedEditModel>()
            //    .IncludeBase<EventBase, EventEditModel>()
            //    .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty))
            //    .ForMember(dst => dst.CanHide, o => o.Ignore());

            //CreateMap<Event, EventExtendedCreateModel>()
            //    .IncludeBase<EventBase, EventCreateModel>()
            //    .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));

            CreateMap<Event, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<EventBase, IntranetActivityItemHeaderViewModel>();

            CreateMap<Event, IntranetActivityDetailsHeaderViewModel>()
                .IncludeBase<EventBase, IntranetActivityDetailsHeaderViewModel>();

            //CreateMap<EventEditModel, Event>()
            //    .IncludeBase<EventEditModel, EventBase>()
            //    .ForMember(dst => dst.GroupId, o => o.Ignore())
            //    .ForMember(dst => dst.Id, o => o.Ignore())
            //    .ForMember(dst => dst.IsHidden, o => o.Ignore())
            //    .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //    .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.Likes, o => o.Ignore())
            //    .ForMember(dst => dst.Comments, o => o.Ignore())
            //    .ForMember(dst => dst.Subscribers, o => o.Ignore())
            //    .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            //CreateMap<EventCreateModel, Event>()
            //    .IncludeBase<EventCreateModel, EventBase>()
            //    .ForMember(dst => dst.GroupId, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.Likes, o => o.Ignore())
            //    .ForMember(dst => dst.Comments, o => o.Ignore())
            //    .ForMember(dst => dst.Subscribers, o => o.Ignore())
            //    .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.SubscribeNotes, o => o.Ignore());

            CreateMap<Event, ActivityTransferCreateModel>();

            CreateMap<Event, ActivityTransferModel>()
                .IncludeBase<Event, ActivityTransferCreateModel>();

            CreateMap<Event, GroupActivityTransferCreateModel>()
                .IncludeBase<Event, ActivityTransferCreateModel>();

            CreateMap<Event, GroupActivityTransferModel>()
                .IncludeBase<Event, GroupActivityTransferCreateModel>();

            CreateMap<Event, IntranetActivityPreviewModelBase>()
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.MediaPreview, o => o.Ignore())
                .ForMember(dst => dst.LikedByCurrentUser, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.GroupInfo, o => o.Ignore())
                .ForMember(dst => dst.IsGroupMember, o => o.Ignore())
                .ForMember(dst => dst.CurrentMemberSubscribed, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(src => src.Type))
                .ForMember(dst => dst.Dates, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    var startDate = src.StartDate.ToDateTimeFormat();
                    string endDate;

                    if (src.StartDate.Date == src.EndDate.Date)
                    {
                        endDate = src.EndDate.ToTimeFormat();
                    }
                    else
                    {
                        endDate = src.EndDate.ToDateTimeFormat();
                    }

                    dst.Dates = new[] { startDate, endDate };
                });

            //Mapper.CreateMap<EventCreateModel, EventExtendedCreateModel>()
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
            //    .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore());

            //Mapper.CreateMap<EventEditModel, EventExtendedEditModel>()
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
            //    .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty))
            //    .ForMember(dst => dst.CanEditSubscribe, o => o.Ignore());

            //Mapper.CreateMap<EventBackofficeCreateModel, Event>()
            //   .IncludeBase<EventBackofficeCreateModel, EventBase>()
            //   .ForMember(dst => dst.GroupId, o => o.Ignore())
            //   .ForMember(dst => dst.Type, o => o.Ignore())
            //   .ForMember(dst => dst.Likes, o => o.Ignore())
            //   .ForMember(dst => dst.Comments, o => o.Ignore())
            //   .ForMember(dst => dst.Subscribers, o => o.Ignore())
            //   .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
            //   .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //   .ForMember(dst => dst.SubscribeNotes, o => o.Ignore());



            CreateMap<Event, ComingEventViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.GetEventDateTimeString(el.EndDate) }));

            //CreateMap<EventBase, EventItemViewModel>()
            //    .ForMember(dst => dst.Links, o => o.Ignore())
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
            //    .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
            //    .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
            //    .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            //CreateMap<Event, EventCreateModel>()
            //    .ForMember(dst => dst.PinAllowed, o => o.Ignore())
            //    .ForMember(dst => dst.NewMedia, o => o.Ignore())
            //    .ForMember(dst => dst.OwnerId, o => o.Ignore())
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
            //    .ForMember(dst => dst.TagIdsData, o => o.Ignore())
            //    .ForMember(dst => dst.GroupId, o => o.Ignore())
            //    .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            //CreateMap<EventBase, EventEditModel>()
            //    .ForMember(dst => dst.PinAllowed, o => o.Ignore())
            //    .ForMember(dst => dst.NewMedia, o => o.Ignore())
            //    .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
            //    .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
            //    .ForMember(dst => dst.TagIdsData, o => o.Ignore())
            //    .ForMember(dst => dst.NotifyAllSubscribers, o => o.Ignore())
            //    .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            CreateMap<EventCreateModel, Event>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.GroupId, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Likes, o => o.Ignore())
                .ForMember(dst => dst.Comments, o => o.Ignore())
                .ForMember(dst => dst.Subscribers, o => o.Ignore())
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            CreateMap<EventEditModel, Event>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.GroupId, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
                .ForMember(dst => dst.Likes, o => o.Ignore())
                .ForMember(dst => dst.Comments, o => o.Ignore())
                .ForMember(dst => dst.Subscribers, o => o.Ignore())
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            CreateMap<Event, EventViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.CanSubscribe, o => o.Ignore())
                .ForMember(dst => dst.SubscribeNotes, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.Tags, o => o.Ignore())
                .ForMember(dst => dst.AvailableTags, o => o.Ignore())
                .ForMember(dst => dst.LightboxPreviewModel, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .ForMember(dst => dst.StartDateString, o => o.Ignore())
                .ForMember(dst => dst.EndDateString, o => o.Ignore())
                .ForMember(dst => dst.FullEventTime, o => o.Ignore())
                .ForMember(dst => dst.EventDate, o => o.Ignore())
                .ForMember(dst => dst.EventMonth, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")))
                .AfterMap((src, dst) =>
                {
                    var startDate = src.StartDate.ToEventDetailsDateTimeFormat();
                    string endDate;

                    if (src.StartDate.Date == src.EndDate.Date)
                    {
                        endDate = src.EndDate.ToEventDetailsTimeFormat();
                    }
                    else
                    {
                        endDate = src.EndDate.ToEventDetailsDateTimeFormat();
                    }
                    
                    dst.StartDateString = startDate;
                    dst.EndDateString = src.EndDate.ToEventDetailsDateTimeFormat();
                    dst.EventDate = src.StartDate.WithUserOffset().Day;
                    dst.EventMonth = src.StartDate.WithUserOffset().ToString("MMM");

                    dst.FullEventTime = $"{startDate} - {endDate}";
                }); ;

            //Mapper.CreateMap<EventBase, EventBackofficeViewModel>()
            //    .ForMember(dst => dst.StartDate, o => o.MapFrom(s => s.StartDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.EndDate, o => o.MapFrom(s => s.EndDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.PublishDate, o => o.MapFrom(s => s.PublishDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.CreatedDate, o => o.MapFrom(s => s.CreatedDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.ModifyDate, o => o.MapFrom(s => s.ModifyDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            CreateMap<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => el.PublishDate.ToDateTimeFormat().ToEnumerable()));

            CreateMap<EventBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<EventBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id))
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.StartDate.GetEventDateTimeString(el.EndDate) }));

            //Mapper.CreateMap<EventBackofficeCreateModel, EventBase>()
            //   .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //   .ForMember(dst => dst.Type, o => o.Ignore())
            //   .ForMember(dst => dst.Id, o => o.Ignore())
            //   .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //   .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //   .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //   .ForMember(dst => dst.EndPinDate, o => o.Ignore())
            //   .ForMember(dst => dst.IsPinActual, o => o.Ignore())
            //   .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //   .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
            //   .AfterMap((src, dst) =>
            //   {
            //       dst.MediaIds = src.Media.ToIntCollection();
            //   });

            //Mapper.CreateMap<EventBackofficeSaveModel, EventBase>()
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //    .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //    .ForMember(dst => dst.EndPinDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinActual, o => o.Ignore())
            //    .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
            //    .AfterMap((src, dst) =>
            //    {
            //        dst.MediaIds = src.Media.ToIntCollection();
            //    });
        }
    }
}