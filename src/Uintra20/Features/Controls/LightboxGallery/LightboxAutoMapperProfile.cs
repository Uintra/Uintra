using AutoMapper;

namespace Uintra20.Features.Controls.LightboxGallery
{
    public class LightboxAutoMapperProfile : Profile
    {
	    public LightboxAutoMapperProfile()
        {
            //Mapper.CreateMap<LightboxGalleryPreviewModel, LightboxGalleryPreviewViewModel>()
            //    .ForMember(d => d.Links, o => o.Ignore())
            //    .ForMember(d => d.Medias, o => o.Ignore())
            //    .ForMember(d => d.HiddenImagesCount, o => o.Ignore())
            //    .ForMember(d => d.OtherFiles, o => o.Ignore());
        }
    }
}