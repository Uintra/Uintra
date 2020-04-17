using AutoMapper;
using Uintra20.Features.LinkPreview.Models;
using Uintra20.Features.LinkPreview.Sql;

namespace Uintra20.Features.LinkPreview.AutoMapperProfiles
{
    public class LinkPreviewAutoMapperProfile : Profile
    {
        public LinkPreviewAutoMapperProfile()
        {
            CreateMap<Compent.LinkPreview.HttpClient.LinkPreview, LinkPreviewEntity>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.OgDescription, o => o.Ignore())
                .ForMember(dst => dst.Uri, o => o.Ignore())
                .ForMember(dst => dst.MediaId, o => o.Ignore());

            CreateMap<Models.LinkPreview, LinkPreviewViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Uri, o => o.MapFrom(s => s.Uri))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.FaviconUri, o => o.MapFrom(s => s.FaviconUri))
                .ForMember(d => d.ImageUri, o => o.MapFrom(s => s.ImageUri))
                .ReverseMap()
                .ForAllOtherMembers(o => o.Ignore());
        }
    }
}