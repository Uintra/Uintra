using AutoMapper;
using Compent.uIntra.Core.News.Models;
using uIntra.Core.Activity;
using uIntra.News;
using uIntra.News.Dashboard;

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

            Mapper.CreateMap<Entities.News, NewsExtendedCreateModel>()
                .IncludeBase<NewsBase, NewsCreateModel>()
                .ForMember(dst => dst.Tags, o => o.Ignore());

            Mapper.CreateMap<Entities.News, NewsExtendedEditModel>()
                .IncludeBase<NewsBase, NewsEditModel>()
                .ForMember(dst => dst.Tags, o => o.Ignore());

            Mapper.CreateMap<Entities.News, NewsBackofficeViewModel>()
                .IncludeBase<NewsBase, NewsBackofficeViewModel>();

            Mapper.CreateMap<Entities.News, IntranetActivityItemHeaderViewModel>()
                .IncludeBase<NewsBase, IntranetActivityItemHeaderViewModel>();

            Mapper.CreateMap<NewsCreateModel, NewsExtendedCreateModel>(MemberList.Source);

            Mapper.CreateMap<NewsEditModel, NewsExtendedEditModel>(MemberList.Source);
        }
    }
}