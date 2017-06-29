using AutoMapper;
using Compent.uIntra.Core.News.Models;
using uIntra.Core.Activity;
using uIntra.News;
using uIntra.News.Dashboard;
using uIntra.Search;

namespace Compent.uIntra.Core.News
{
    public class NewsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Entities.News, NewsExtendedViewModel>()
                .IncludeBase<NewsBase, NewsViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Entities.News, NewsExtendedItemViewModel>()
                .IncludeBase<NewsBase, NewsItemViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<Entities.News, NewsBackofficeViewModel>()
                .IncludeBase<NewsBase, NewsBackofficeViewModel>();

            Mapper.CreateMap<Entities.News, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<Entities.News, SearchableActivity>()
                
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishDate))
                .IncludeBase<IntranetActivity, SearchableActivity>();
        }
    }
}