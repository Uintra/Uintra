using AutoMapper;
using Compent.uCommunity.Core.News.Entities;
using Compent.uCommunity.Core.News.Models;
using uCommunity.Core.Activity.Models;
using uCommunity.News;
using uCommunity.News.Dashboard;

namespace Compent.uCommunity.Core.News
{
    public class NewsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NewsEntity, NewsExtendedViewModel>()
                .IncludeBase<NewsBase, NewsViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<NewsEntity, NewsOverviewItemExtendedViewModel>()
                .IncludeBase<NewsBase, NewsItemViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<NewsEntity, NewsExtendedCreateModel>()
                .IncludeBase<NewsBase, NewsCreateModel>()
                .ForMember(dst => dst.Tags, o => o.Ignore());

            Mapper.CreateMap<NewsEntity, NewsExtendedEditModel>()
                .IncludeBase<NewsBase, NewsEditModel>()
                .ForMember(dst => dst.Tags, o => o.MapFrom(el => el.Tags));

            Mapper.CreateMap<NewsEntity, NewsBackofficeViewModel>()
                .IncludeBase<NewsBase, NewsBackofficeViewModel>();

            Mapper.CreateMap<NewsEntity, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsBase, IntranetActivityItemHeaderViewModel>();
        }
    }
}