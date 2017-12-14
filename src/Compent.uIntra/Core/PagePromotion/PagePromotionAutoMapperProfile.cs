using AutoMapper;
using Compent.uIntra.Core.PagePromotion.Models;
using uIntra.CentralFeed;
using uIntra.Core.Extensions;
using uIntra.Core.PagePromotion;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.PagePromotion
{
    public class PagePromotionAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IPublishedContent, Entities.PagePromotion>()
               .ForMember(dst => dst.Id, src => src.MapFrom(el => el.GetKey()))
               .ForMember(dst => dst.CreatedDate, src => src.MapFrom(el => el.CreateDate))
               .ForMember(dst => dst.UmbracoCreatorId, src => src.MapFrom(el => el.CreatorId))
               .ForMember(dst => dst.ModifyDate, src => src.MapFrom(el => el.UpdateDate))
               .ForMember(dst => dst.PageAlias, src => src.MapFrom(el => el.DocumentTypeAlias))
               .ForMember(dst => dst.CreatorId, src => src.Ignore())
               .ForMember(dst => dst.Type, src => src.Ignore())
               .ForMember(dst => dst.MediaIds, src => src.Ignore())
               .ForMember(dst => dst.Comments, src => src.Ignore())
               .ForMember(dst => dst.Likes, src => src.Ignore())
               .ForMember(dst => dst.IsPinActual, src => src.Ignore())
               .ForMember(dst => dst.IsHidden, src => src.Ignore())
               .ForMember(dst => dst.IsPinned, src => src.Ignore())
               .ForMember(dst => dst.EndPinDate, src => src.Ignore())
               .ForMember(dst => dst.PublishDate, src => src.Ignore())
               .ForMember(dst => dst.Title, src => src.Ignore())
               .ForMember(dst => dst.Description, src => src.Ignore())
               .ForMember(dst => dst.OwnerId, src => src.Ignore())
               .ForMember(dst => dst.Commentable, src => src.Ignore())
               .ForMember(dst => dst.Likeable, src => src.Ignore())
               .ForMember(dst => dst.IsReadOnly, src => src.Ignore());

            Mapper.CreateMap<PagePromotionConfig, Entities.PagePromotion>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Comments, o => o.Ignore())
                .ForMember(dst => dst.Likes, o => o.Ignore())
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.PageAlias, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.OwnerId, o => o.Ignore())
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Files.ToIntCollection();
                });

            Mapper.CreateMap<Entities.PagePromotion, PagePromotionExtendedItemViewModel>()
                .IncludeBase<PagePromotionBase, PagePromotionItemViewModel>()
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Entities.PagePromotion, ActivityTransferCreateModel>();

            Mapper.CreateMap<Entities.PagePromotion, ActivityTransferModel>()
                .IncludeBase<Entities.PagePromotion, ActivityTransferCreateModel>();
        }
    }
}