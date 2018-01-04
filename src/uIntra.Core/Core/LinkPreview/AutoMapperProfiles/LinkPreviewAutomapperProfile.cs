using AutoMapper;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewAutomapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<LinkPreviewDto, LinkPreview>()
                .ForMember(dst => dst.Description, o => o.Ignore());
        }
    }
}
