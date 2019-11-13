using AutoMapper;

namespace Uintra20.Core.Subscribe
{
    public class SubscribeSettingAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<ISubscribeSettings, ActivitySubscribeSettingDto>()
            //    .ForMember(dst => dst.ActivityId, o => o.MapFrom(s => s.Id));
        }
    }
}