using AutoMapper;
using Uintra20.Core.LinkPreview.Sql;

namespace Uintra20.Core.LinkPreview
{
    public class LinkPreviewAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<LinkPreview, LinkPreviewViewModel>();

            Mapper.CreateMap<Compent.LinkPreview.HttpClient.LinkPreview, LinkPreviewEntity>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.OgDescription, o => o.Ignore())
                .ForMember(dst => dst.Uri, o => o.Ignore())
                .ForMember(dst => dst.MediaId, o => o.Ignore());
        }
    }
}