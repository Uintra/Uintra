using AutoMapper;

namespace Uintra20.Core.Tagging
{
    public class UserTagsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<UserTag, LabeledIdentity<Guid>>()
            //    .ForMember(f => f.Text, d => d.MapFrom(i => i.Text));
        }
    }
}