using AutoMapper;

namespace uCommunity.Tagging
{
    public class TagAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<TagEditModel, TagDTO>();
            Mapper.CreateMap<Tag, TagEditModel>();
        }
    }
}