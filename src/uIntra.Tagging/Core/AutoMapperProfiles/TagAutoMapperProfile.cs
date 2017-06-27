using AutoMapper;
using uIntra.Tagging.Core.Models;

namespace uIntra.Tagging
{
    public class TagAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<TagEditModel, TagDTO>();
            Mapper.CreateMap<Tag, TagEditModel>();
            Mapper.CreateMap<Tag, TagViewModel>();
        }
    }
}