using AutoMapper;
using Compent.uCommunity.Core.News.Entities;
using Compent.uCommunity.Core.News.Models;
using uCommunity.Core.Activity.Models;
using uCommunity.News;

namespace Compent.uCommunity.Core.News
{
    public class NewsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Entities.NewsEntity, NewsExtendedViewModel>()
                .IncludeBase<NewsBase, NewsExtendedViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el))
                .ForMember(dst => dst.CommentsInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<NewsEntity, NewsOverviewItemExtendedViewModel>()
                .IncludeBase<NewsBase, NewsOverviewItemExtendedViewModel>()
                .ForMember(dst => dst.LikesInfo, o => o.MapFrom(el => el));

            Mapper.CreateMap<NewsEntity, IntranetActivityItemHeaderViewModel>()
                 .IncludeBase<NewsBase, IntranetActivityItemHeaderViewModel>();
        }
    }
}