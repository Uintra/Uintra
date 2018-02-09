using AutoMapper;
using Uintra.Core.LinkPreview.Sql;

namespace Uintra.Core.LinkPreview
{
    public class LinkPreviewAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Compent.LinkPreview.HttpClient.LinkPreview, LinkPreviewEntity>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.OgDescription, o => o.Ignore())
                .ForMember(dst => dst.Uri, o => o.Ignore());
        }
    }
}