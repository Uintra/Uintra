using AutoMapper;

namespace Uintra.Core.Controls.LightboxGallery
{
    public class LightboxAutoMapperProfile: Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<LightboxGalleryPreviewModel, LightboxGalleryPreviewViewModel>()
                .ForMember(d => d.Links, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.OtherFiles, o => o.Ignore())
                .ForMember(d => d.TotalFileCount, o => o.Ignore())
                .ForMember(d => d.IsAttachedFileIconShown, o => o.Ignore());
        }
    }
}