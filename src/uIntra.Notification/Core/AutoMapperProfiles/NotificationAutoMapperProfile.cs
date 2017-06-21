using System.Web.Helpers;
using AutoMapper;
using uIntra.Core.Extentions;

namespace uIntra.Notification
{
    public class NotificationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Notification, NotificationViewModel>()
                .ForMember(d => d.NotifierName, o => o.Ignore())
                .ForMember(d => d.NotifierPhoto, o => o.Ignore())
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date.ToDateTimeFormat()))
                .ForMember(d => d.Value, o => o.MapFrom(s => Json.Decode(s.Value)));
        }
    }
}