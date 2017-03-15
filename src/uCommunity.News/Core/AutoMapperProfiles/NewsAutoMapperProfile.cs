using AutoMapper;
using uCommunity.Core.Extentions;
using uCommunity.News.Dashboard;

namespace uCommunity.News
{
    public class NewsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<News, NewsOverviewItemModel>()
                .ForMember(dst => dst.MediaIds, o => o.Ignore());

            Mapper.CreateMap<News, NewsCreateModel>()
                .ForMember(dst => dst.AllowedMediaExtentions, o => o.Ignore())
                .ForMember(dst => dst.Users, o => o.Ignore())
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<News, NewsEditModel>()
                .IncludeBase<News, NewsCreateModel>()
                .ForMember(d => d.CreatorId, o => o.MapFrom(el => el.Creator.Id));

            Mapper.CreateMap<NewsCreateModel, News>()
                .ForMember(dst => dst.Id, o => o.Ignore())
                .ForMember(dst => dst.MediaIds, o => o.Ignore())
                .ForMember(dst => dst.IsHidden, o => o.Ignore())
                .ForMember(dst => dst.UmbracoCreatorId, o => o.Ignore())
                .ForMember(dst => dst.CreatedDate, o => o.Ignore())
                .ForMember(dst => dst.ModifyDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore());

            Mapper.CreateMap<NewsEditModel, News>()
                .IncludeBase<NewsCreateModel, News>()
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .ForMember(d => d.MediaIds, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    dst.MediaIds = src.Media.ToIntCollection();
                });

            Mapper.CreateMap<News, NewsModel>()
                .ForMember(dst => dst.OverviewPageUrl, o => o.Ignore())
                .ForMember(dst => dst.EditPageUrl, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.CanEdit, o => o.Ignore())
                .ForMember(dst => dst.Media, o => o.MapFrom(el => StringExtentions.JoinToString(el.MediaIds, ",")));

            Mapper.CreateMap<News, NewsBackofficeViewModel>()
                .ForMember(d => d.Media, o => o.MapFrom(s => StringExtentions.JoinToString(s.MediaIds, ",")));

            Mapper.CreateMap<NewsBackofficeCreateModel, News>()
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

            Mapper.CreateMap<NewsBackofficeSaveModel, News>()
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