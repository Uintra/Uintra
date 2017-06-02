using System.Collections.Generic;
using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.News.Dashboard;

namespace uIntra.News
{
    public class NewsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewsBase, NewsItemViewModel>()
                .ForMember(dst => dst.ShortDescription, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Expired, o => o.Ignore())
                .ForMember(dst => dst.LightboxGalleryPreviewInfo, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore());

            Mapper.CreateMap<NewsBase, NewsCreateModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Users, o => o.Ignore())
                .ForMember(dst => dst.CanEditCreator, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<NewsBase, NewsEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Users, o => o.Ignore())
                .ForMember(dst => dst.CanEditCreator, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<NewsCreateModel, NewsBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.PublishDate = src.PublishDate.ToUniversalTime();
                    dst.UnpublishDate = src.UnpublishDate?.ToUniversalTime();
                });

            Mapper.CreateMap<NewsEditModel, NewsBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                    dst.PublishDate = src.PublishDate.ToUniversalTime();
                    dst.UnpublishDate = src.UnpublishDate?.ToUniversalTime();
                    dst.UmbracoCreatorId = null;
                });

            Mapper.CreateMap<NewsBase, NewsViewModel>()
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.HeaderInfo, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => el.MediaIds.JoinToString(",")));

            Mapper.CreateMap<NewsBase, NewsBackofficeViewModel>()
                .ForMember(dst => dst.CreatorId, o => o.MapFrom(el => el.Creator.Id))
                .ForMember(dst => dst.Media, o => o.MapFrom(s => s.MediaIds.JoinToString(",")));

            Mapper.CreateMap<NewsBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.PublishDate.ToDateFormat() }));

            Mapper.CreateMap<NewsBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsBase, IntranetActivityDetailsHeaderViewModel>()
                .ForMember(dst => dst.ActivityId, o => o.MapFrom(el => el.Id));

            Mapper.CreateMap<NewsBackofficeCreateModel, NewsBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.PinDays, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            Mapper.CreateMap<NewsBackofficeSaveModel, NewsBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .ForMember(dst => dst.IsPinned, o => o.Ignore())
                .ForMember(dst => dst.PinDays, o => o.Ignore())
                .ForMember(dst => dst.EndPinDate, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });
        }
    }
}