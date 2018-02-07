using AutoMapper;
using uIntra.Core.LinkPreview.Sql;

namespace uIntra.Core.LinkPreview
{
    public class LinkPreviewAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Compent.LinkPreview.Client.LinkPreview, LinkPreviewEntity>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.OgDescription, o => o.Ignore())
                .ForMember(dst => dst.Uri, o => o.Ignore());
        }
    }
}