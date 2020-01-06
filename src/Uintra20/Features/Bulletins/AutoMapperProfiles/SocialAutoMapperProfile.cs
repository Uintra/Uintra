using AutoMapper;
using Compent.Extensions;
using System;
using System.Linq;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Groups.Links;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Bulletins.AutoMapperProfiles
{
    public class SocialAutoMapperProfile : Profile
    {
        public SocialAutoMapperProfile()
        {
            CreateMap<SocialBase, SocialItemViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore());

            CreateMap<SocialBase, SocialEditModel>()
              .ForMember(dst => dst.Links, o => o.Ignore())
              .ForMember(dst => dst.ActivityType, o => o.Ignore())
              .ForMember(dst => dst.MediaRootId, o => o.Ignore())
              .ForMember(dst => dst.NewMedia, o => o.Ignore())
              .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.Select(m=>m.ToString()).JoinWith(",")))
              .ForMember(dst => dst.CanDelete, o => o.Ignore());

            CreateMap<SocialCreateModel, SocialBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.PublishDate, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.Title, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.LinkPreview, o => o.Ignore());

            CreateMap<SocialEditModel, SocialBase>()
                .ForMember(dst => dst.Title, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.PublishDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CreatorId, o => o.Ignore())
                .ForMember(dst => dst.OwnerId, o => o.Ignore())
                .ForMember(dst => dst.IsPinActual, o => o.Ignore())
                .ForMember(dst => dst.LinkPreview, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            CreateMap<SocialBase, SocialViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(el => el.Type))
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.Ignore())
                .ForMember(dst => dst.LightboxPreviewModel, o => o.Ignore());

            CreateMap<Social, SocialPreviewModel>()
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.MediaPreview, o => o.Ignore())
                .ForMember(dst => dst.LikedByCurrentUser, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.Ignore())
                .ForMember(dst => dst.ActivityType, o => o.MapFrom(src => src.Type))
                .ForMember(dst => dst.Dates, o => o.MapFrom(src => src.PublishDate.ToDateTimeFormat().ToEnumerable()));

            //CreateMap<SocialBase, BulletinsBackofficeViewModel>()
            //    .ForMember(dst => dst.PublishDate, o => o.MapFrom(s => s.PublishDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.CreatedDate, o => o.MapFrom(s => s.CreatedDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.ModifyDate, o => o.MapFrom(s => s.ModifyDate.ToIsoUtcString()))
            //    .ForMember(dst => dst.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            CreateMap<SocialBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => el.PublishDate.ToDateFormat().ToEnumerable()));

            CreateMap<SocialBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<SocialBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id));

            //CreateMap<BulletinsBackofficeCreateModel, SocialBase>()
            //    .ForMember(dst => dst.Location, o => o.Ignore())
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.Id, o => o.Ignore())
            //    .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //    .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //    .ForMember(dst => dst.EndPinDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsHidden, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinActual, o => o.Ignore())
            //    .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.OwnerId, o => o.Ignore())
            //    .ForMember(dst => dst.LinkPreview, o => o.Ignore())
            //    .ForMember(dst => dst.LinkPreviewId, o => o.Ignore())
            //    .AfterMap((dst, src) =>
            //    {
            //        src.MediaIds = dst.Media.ToIntCollection();
            //    });

            //CreateMap<BulletinsBackofficeSaveModel, SocialBase>()
            //    .ForMember(dst => dst.Location, o => o.Ignore())
            //    .ForMember(dst => dst.MediaIds, o => o.Ignore())
            //    .ForMember(dst => dst.Type, o => o.Ignore())
            //    .ForMember(dst => dst.CreatorId, o => o.Ignore())
            //    .ForMember(dst => dst.OwnerId, o => o.Ignore())
            //    .ForMember(dst => dst.CreatedDate, o => o.Ignore())
            //    .ForMember(dst => dst.ModifyDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinned, o => o.Ignore())
            //    .ForMember(dst => dst.EndPinDate, o => o.Ignore())
            //    .ForMember(dst => dst.IsHidden, o => o.Ignore())
            //    .ForMember(dst => dst.IsPinActual, o => o.Ignore())
            //    .ForMember(dst => dst.LinkPreview, o => o.Ignore())
            //    .ForMember(dst => dst.LinkPreviewId, o => o.Ignore())
            //    .AfterMap((dst, src) =>
            //    {
            //        src.MediaIds = dst.Media.ToIntCollection();
            //    });



            CreateMap<Social, SocialExtendedViewModel>()
                .IncludeBase<SocialBase, SocialViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            CreateMap<Social, SocialExtendedItemViewModel>()
                .IncludeBase<SocialBase, SocialItemViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            CreateMap<SocialCreateModel, SocialExtendedCreateModel>()
                .ForMember(dst => dst.GroupId, o => o.Ignore())
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => Enumerable.Empty<Guid>()));

            CreateMap<SocialEditModel, SocialExtendedEditModel>()
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(el => Enumerable.Empty<Guid>()));

            CreateMap<Social, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<SocialBase, IntranetActivityItemHeaderViewModel>();

            CreateMap<Social, ActivityTransferCreateModel>();

            CreateMap<Social, ActivityTransferModel>()
                .IncludeBase<Social, ActivityTransferCreateModel>();

            CreateMap<Social, GroupActivityTransferCreateModel>()
                .IncludeBase<Social, ActivityTransferCreateModel>();

            CreateMap<Social, GroupActivityTransferModel>()
                .IncludeBase<Social, GroupActivityTransferCreateModel>();
        }
    }
}