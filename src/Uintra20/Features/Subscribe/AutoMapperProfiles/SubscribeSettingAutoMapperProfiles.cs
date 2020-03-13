using AutoMapper;

namespace Uintra20.Features.Subscribe.AutoMapperProfiles
{
    public class SubscribeSettingAutoMapperProfiles : Profile
    {
        public SubscribeSettingAutoMapperProfiles()
        {
            CreateMap<ISubscribeSettings, ActivitySubscribeSettingDto>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(s => s.Id));
        }
    }
}