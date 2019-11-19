using AutoMapper;

namespace Uintra20.Features.Tagging.AutoMapperProfiles
{
    public class UserTagsAutoMapperProfile : Profile
    {
		public UserTagsAutoMapperProfile()
        {
            //Mapper.CreateMap<UserTag, LabeledIdentity<Guid>>()
            //    .ForMember(f => f.Text, d => d.MapFrom(i => i.Text));
        }
    }
}