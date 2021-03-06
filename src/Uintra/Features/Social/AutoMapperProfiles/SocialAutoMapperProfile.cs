﻿using AutoMapper;
using Compent.Extensions;
using System.Linq;
using Uintra.Core.Activity.Models.Headers;
using Uintra.Features.CentralFeed.Models;
using Uintra.Features.Groups.Links;
using Uintra.Features.Social.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Social.AutoMapperProfiles
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
                .ForMember(dst => dst.IsReadOnly, o => o.Ignore())
                .ForMember(d => d.LinkPreview, o => o.MapFrom(s => s.LinkPreview));
                

            CreateMap<SocialBase, SocialEditModel>()
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.TagIdsData, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.Select(m => m.ToString()).JoinWith(",")))
                .ForMember(dst => dst.CanDelete, o => o.Ignore())
                .ForMember(d => d.LinkPreview, o => o.MapFrom(s => s.LinkPreview))
                .ForMember(d => d.LinkPreviewId, o => o.MapFrom(s => s.LinkPreviewId));


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
                .ForMember(dst => dst.LinkPreview, o => o.MapFrom(s => s.LinkPreview))
                .ForMember(d => d.LinkPreviewId, o => o.MapFrom(s => s.LinkPreviewId));

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
                .ForMember(dst => dst.LinkPreview, o => o.MapFrom(s => s.LinkPreview))
                .ForMember(d => d.LinkPreviewId, o => o.MapFrom(s => s.LinkPreviewId))
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


            CreateMap<SocialBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Links, o => o.Ignore())
                .ForMember(dst => dst.Owner, o => o.Ignore())
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => el.PublishDate.ToDateFormat().ToEnumerable()));

            CreateMap<SocialBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<SocialBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id));

            CreateMap<Entities.Social, SocialExtendedViewModel>()
                .IncludeBase<SocialBase, SocialViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            CreateMap<Entities.Social, SocialExtendedItemViewModel>()
                .IncludeBase<SocialBase, SocialItemViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            CreateMap<Entities.Social, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<SocialBase, IntranetActivityItemHeaderViewModel>();

            CreateMap<Entities.Social, ActivityTransferCreateModel>();

            CreateMap<Entities.Social, ActivityTransferModel>()
                .IncludeBase<Entities.Social, ActivityTransferCreateModel>();

            CreateMap<Entities.Social, GroupActivityTransferCreateModel>()
                .IncludeBase<Entities.Social, ActivityTransferCreateModel>();

            CreateMap<Entities.Social, GroupActivityTransferModel>()
                .IncludeBase<Entities.Social, GroupActivityTransferCreateModel>();
        }
    }
}