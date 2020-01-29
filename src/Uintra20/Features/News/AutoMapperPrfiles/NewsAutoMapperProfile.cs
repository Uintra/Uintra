using AutoMapper;
using Compent.Extensions;
using Uintra20.Core.Activity.Models;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.News.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.AutoMapperPrfiles
{
    public class NewsAutoMapperProfile : Profile
    {
        public NewsAutoMapperProfile()
        {
            CreateMap<NewsBase, NewsViewModel>()
                .ForMember(dst => dst.UnpublishDate, o => o.MapFrom(s => s.UnpublishDate))
                .ForMember(dst => dst.OwnerId, o => o.MapFrom(s => s.OwnerId))
                .ForMember(dst => dst.IsPinned, o => o.MapFrom(s => s.IsPinned))
                .ForMember(dst => dst.EndPinDate, o => o.MapFrom(s => s.EndPinDate))
                .ForMember(dst => dst.Location, o => o.MapFrom(s => s.Location))
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")))
                .ForMember(dst => dst.LikesInfo, o => o.Ignore())
                .ForMember(dst => dst.CommentsInfo, o => o.Ignore());

            CreateMap<Entities.News, NewsViewModel>()
                .IncludeBase<NewsBase, NewsViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            //CreateMap<Entities.News, NewsExtendedItemViewModel>()
            //    .IncludeBase<NewsBase, NewsItemViewModel>()
            //    .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
            //    .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el));
            
            CreateMap<Entities.News, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsBase, IntranetActivityItemHeaderViewModel>();

            CreateMap<Entities.News, ActivityTransferCreateModel>();

            CreateMap<Entities.News, ActivityTransferModel>()
                .IncludeBase<Entities.News, ActivityTransferCreateModel>();

            CreateMap<Entities.News, GroupActivityTransferCreateModel>()
                .IncludeBase<Entities.News, ActivityTransferCreateModel>();

            CreateMap<Entities.News, GroupActivityTransferModel>()
                .IncludeBase<Entities.News, GroupActivityTransferCreateModel>();

            CreateMap<Entities.News, IntranetActivityPreviewModelBase>()
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.MediaPreview, o => o.Ignore())
                .ForMember(dst => dst.LikedByCurrentUser, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(src => src.Type));

            CreateMap<Entities.News, IntranetActivityDetailsViewModel>()
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.MediaPreview, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(src => src.Type));


            //CreateMap<NewsBase, NewsItemViewModel>()
            //    .ForMember(dst => dst.Links, o => o.Ignore())
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Expired, o => o.Ignore())
            //    .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
            //    .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
            //    .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
            //    .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            CreateMap<NewsBase, NewsCreateModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.PinAllowed, o => o.Ignore())
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")))
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty))
                .AfterMap((s, d) =>
                {
                    int i = 0;
                });

            CreateMap<NewsBase, NewsEditModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.PinAllowed, o => o.Ignore())
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")))
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => string.Empty));

            CreateMap<NewsCreateModel, NewsBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore());

            CreateMap<NewsEditModel, NewsBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            //CreateMap<NewsBase, NewsBackofficeViewModel>()
            //    .ForMember(dst => dst.PublishDate, o => o.MapFrom(s => s.PublishDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.UnpublishDate, o => o.MapFrom(s => s.UnpublishDate.HasValue ? s.UnpublishDate.Value.ToIsoUtcString() : string.Empty))
            //    .ForMember(dst => dst.CreatedDate, o => o.MapFrom(s => s.CreatedDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.ModifyDate, o => o.MapFrom(s => s.ModifyDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            CreateMap<NewsBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => el.PublishDate.ToDateFormat().ToEnumerable()));

            CreateMap<NewsBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id));

            //CreateMap<NewsBackofficeCreateModel, NewsBase>()
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.Id, o => o.Ignore())
            //    .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //    .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //    .ForMember(dst => dst.EndPinDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinActual, o => o.Ignore())
            //    .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
            //    .AfterMap((src, dst) =>
            //    {
            //        dst.MediaIds = src.Media.ToIntCollection();
            //    });

            //CreateMap<NewsBackofficeSaveModel, NewsBase>()
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //    .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //    .ForMember(dst => dst.EndPinDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinActual, o => o.Ignore())
            //    .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
            //    .AfterMap((src, dst) =>
            //    {
            //        dst.MediaIds = src.Media.ToIntCollection();
            //    });
        }
    }
}