using AutoMapper;
using Uintra20.Features.LinkPreview.Models;
using Uintra20.Features.LinkPreview.Sql;

namespace Uintra20.Features.LinkPreview.AutoMapperProfiles
{
    public class LinkPreviewAutoMapperProfile : Profile
	{
		public LinkPreviewAutoMapperProfile()
		{
			CreateMap<Models.LinkPreview, LinkPreviewViewModel>();

            CreateMap<Compent.LinkPreview.HttpClient.LinkPreview, LinkPreviewEntity>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.OgDescription, o => o.Ignore())
                .ForMember(dst => dst.Uri, o => o.Ignore())
                .ForMember(dst => dst.MediaId, o => o.Ignore());

        }
	}
}