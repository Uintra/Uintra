using System.Web.Helpers;
using AutoMapper;
using uCommunity.Notification.Models;

namespace uCommunity.Notification.Profiles
{
    public class NotificationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Sql.Notification, NotificationViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.IsNotified, o => o.MapFrom(s => s.IsNotified))
                .ForMember(d => d.IsViewed, o => o.MapFrom(s => s.IsViewed))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)));
        }
    }
}