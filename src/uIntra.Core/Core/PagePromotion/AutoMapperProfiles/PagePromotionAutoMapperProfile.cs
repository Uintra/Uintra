using AutoMapper;
using Extensions;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Umbraco.Core.Models;

namespace Uintra.Core.PagePromotion
{
    public class PagePromotionAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, PagePromotionBase>()
                .ForMember(dst => dst.Id, src => src.MapFrom(el => el.GetGuidKey()))
                .ForMember(dst => dst.CreatedDate, src => src.MapFrom(el => el.CreateDate))
                .ForMember(dst => dst.UmbracoCreatorId, src => src.MapFrom(el => el.CreatorId))
                .ForMember(dst => dst.ModifyDate, src => src.MapFrom(el => el.UpdateDate))
                .ForMember(dst => dst.PageAlias, src => src.MapFrom(el => el.DocumentTypeAlias))
                .ForMember(dst => dst.CreatorId, src => src.Ignore())
                .ForMember(dst => dst.Type, src => src.Ignore())
                .ForMember(dst => dst.MediaIds, src => src.Ignore())
                .ForMember(dst => dst.IsPinActual, src => src.Ignore())
                .ForMember(dst => dst.IsHidden, src => src.Ignore())
                .ForMember(dst => dst.IsPinned, src => src.Ignore())
                .ForMember(dst => dst.EndPinDate, src => src.Ignore())
                .ForMember(dst => dst.PublishDate, src => src.Ignore())
                .ForMember(dst => dst.Title, src => src.Ignore())
                .ForMember(dst => dst.Description, src => src.Ignore())
                .ForMember(dst => dst.OwnerId, src => src.Ignore())
                .ForMember(dst => dst.Location, src => src.Ignore());

            Mapper.CreateMap<PagePromotionBase, PagePromotionItemViewModel>()
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            Mapper.CreateMap<PagePromotionBase, IntranetActivityItemHeaderViewModel>()
               .ForMember(dst => dst.ActivityId, src => src.MapFrom(el => el.Id))
               .ForMember(dst => dst.Dates, src => src.MapFrom(el => el.PublishDate.ToDateFormat().ToEnumerable()))
               .ForMember(dst => dst.Owner, src => src.Ignore())
               .ForMember(dst => dst.Links, src => src.Ignore());
        }
    }
}
