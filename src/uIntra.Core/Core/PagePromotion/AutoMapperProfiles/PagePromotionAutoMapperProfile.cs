using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using Umbraco.Core.Models;

namespace uIntra.Core.PagePromotion
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
                .ForMember(dst => dst.CreatorId, src => src.Ignore()).ForMember(dst => dst.Type, src => src.Ignore())
                .ForMember(dst => dst.MediaIds, src => src.Ignore())
                //.ForMember(dst => dst.Comments, src => src.Ignore())
                //.ForMember(dst => dst.Likes, src => src.Ignore())
                .ForMember(dst => dst.IsPinActual, src => src.Ignore())
                .ForMember(dst => dst.IsHidden, src => src.Ignore()).ForMember(dst => dst.IsPinned, src => src.Ignore())
                .ForMember(dst => dst.EndPinDate, src => src.Ignore())
                .ForMember(dst => dst.PublishDate, src => src.Ignore()).ForMember(dst => dst.Title, src => src.Ignore())
                .ForMember(dst => dst.Description, src => src.Ignore())
                .ForMember(dst => dst.OwnerId, src => src.Ignore());
               //.ForMember(dst => dst.Commentable, src => src.Ignore())
               //.ForMember(dst => dst.Likeable, src => src.Ignore())
               //.ForMember(dst => dst.IsReadOnly, src => src.Ignore());

            //Mapper.CreateMap<PagePromotionBase, PagePromotionItemViewModel>()
            //    .ForMember(dst => dst.LikesInfo, src => src.MapFrom(el => el))
            //    .ForMember(dst => dst.CommentsInfo, src => src.MapFrom(el => el))
            //    .ForMember(dst => dst.ActivityType, src => src.MapFrom(el => el.Type))
            //    .ForMember(dst => dst.Likeable, src => src.MapFrom(el => el.Likeable))
            //    .ForMember(dst => dst.Comentable, src => src.MapFrom(el => el.Commentable))
            //    .ForMember(dst => dst.ActivityType, src => src.MapFrom(el => el.Type))
            //    .ForMember(dst => dst.MediaIds, src => src.Ignore())
            //    .ForMember(dst => dst.LightboxGalleryPreviewInfo, src => src.Ignore())
            //    .ForMember(dst => dst.HeaderInfo, src => src.Ignore())
            //    .ForMember(dst => dst.Links, src => src.Ignore());

            Mapper.CreateMap<PagePromotionBase, IntranetActivityItemHeaderViewModel>()
               .ForMember(dst => dst.ActivityId, src => src.MapFrom(el => el.Id))
               .ForMember(dst => dst.Dates, src => src.MapFrom(el => el.PublishDate.ToDateFormat().ToEnumerableOfOne()))
               .ForMember(dst => dst.Owner, src => src.Ignore())
               .ForMember(dst => dst.Links, src => src.Ignore());
        }
    }
}
