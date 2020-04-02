using AutoMapper;
using Compent.Extensions;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.News.Entities;
using Uintra20.Features.Social.Entities;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Feed.Mappers
{
    public class FeedMapperProfile : Profile
    {
        public FeedMapperProfile()
        {
            CreateMap<Social, IntranetActivityPreviewModelBase>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ActivityType, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.IsPinActual, o => o.MapFrom(s => s.IsPinActual))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.Dates, o => o.MapFrom(s => s.PublishDate.ToDateFormat().ToEnumerable()))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<News, IntranetActivityPreviewModelBase>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ActivityType, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.IsPinActual, o => o.MapFrom(s => s.IsPinActual))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForMember(d => d.Dates, o => o.MapFrom(s => s.PublishDate.ToDateFormat().ToEnumerable()))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<Event, IntranetActivityPreviewModelBase>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ActivityType, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.IsPinActual, o => o.MapFrom(s => s.IsPinActual))
                .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
                .ForAllOtherMembers(o => o.Ignore());
        }
    }
}