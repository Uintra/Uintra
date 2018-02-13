using AutoMapper;

namespace Uintra.Subscribe
{
    public class SubscribeSettingAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ISubscribeSettings, ActivitySubscribeSettingDto>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(s => s.Id));
        }
    }
}