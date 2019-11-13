using AutoMapper;

namespace Uintra20.Core.Controls.LightboxGallery
{
    public class LightboxAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<LightboxGalleryPreviewModel, LightboxGalleryPreviewViewModel>()
            //    .ForMember(d => d.Links, o => o.Ignore())
            //    .ForMember(d => d.Medias, o => o.Ignore())
            //    .ForMember(d => d.HiddenImagesCount, o => o.Ignore())
            //    .ForMember(d => d.OtherFiles, o => o.Ignore());
        }
    }
}