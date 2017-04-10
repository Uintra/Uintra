using System.Collections.Generic;
using AutoMapper;
using uCommunity.Core;
using uCommunity.Core.Activity.Models;
using uCommunity.Core.Extentions;
using uCommunity.News.Dashboard;

namespace uCommunity.News
{
    public class NewsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewsModelBase, NewsOverviewItemViewModel>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore());

            Mapper.CreateMap<NewsBase, NewsCreateModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<NewsBase, NewsEditModel>()
              .ForMember(dst => dst.MediaRootId, o => o.Ignore())
              .ForMember(dst => dst.NewMedia, o => o.Ignore())
              .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<NewsModelBase, NewsEditModel>()
                .IncludeBase<NewsBase, NewsEditModel>();

            Mapper.CreateMap<NewsCreateModel, NewsModelBase>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore());

            Mapper.CreateMap<NewsEditModel, NewsModelBase>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Creator, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            Mapper.CreateMap<NewsModelBase, NewsViewModel>()
                .ForMember(dst => dst.OverviewPageUrl, o => o.Ignore())
                .ForMember(dst => dst.EditPageUrl, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<NewsModelBase, NewsBackofficeViewModel>()
                .ForMember(d => d.Media, o => o.MapFrom(s => StringExtentions.JoinToString(s.MediaIds, ",")));

            Mapper.CreateMap<NewsModelBase, IntranetActivityDetailsHeaderViewModel>()
           .ForMember(dst => dst.Dates, o => o.MapFrom(el => new List<string> { el.PublishDate.ToString(IntranetConstants.Common.DefaultDateFormat) }));

            Mapper.CreateMap<NewsModelBase, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsModelBase, IntranetActivityDetailsHeaderViewModel>();

            Mapper.CreateMap<NewsBackofficeCreateModel, NewsModelBase>()
                .ForMember(d => d.MediaIds, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.CreatorId, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.ModifyDate, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .AfterMap((dst, src) =>
                {
                    src.MediaIds = dst.Media.ToIntCollection();
                });

            Mapper.CreateMap<NewsBackofficeSaveModel, NewsModelBase>()
                .ForMember(d => d.MediaIds, o => o.Ignore())
                .ForMember(d => d.Type, o => o.Ignore())
                .ForMember(d => d.CreatorId, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.ModifyDate, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .AfterMap((dst, src) =>
                {
                    src.MediaIds = dst.Media.ToIntCollection();
                });
        }
    }
}