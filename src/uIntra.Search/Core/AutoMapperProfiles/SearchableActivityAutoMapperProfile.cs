using System.Web;
using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;

namespace uIntra.Search
{
    public class SearchableActivityAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetActivity, SearchableActivity>()
                .ForMember(dst => dst.Url, o => o.Ignore())
                .ForMember(dst => dst.StartDate, o => o.Ignore())
                .ForMember(dst => dst.EndDate, o => o.Ignore())
                .ForMember(dst => dst.PublishedDate, o => o.Ignore())
                .ForMember(dst => dst.Type, o => o.Ignore())
                .ForMember(dst => dst.Description, o => o.MapFrom(el => el.Description))
                .AfterMap((dst, src) =>
                {
                    var searchableTypeProvider = HttpContext.Current.GetService<ISearchableTypeProvider>();
                    switch (dst.Type.Id)
                    {
                        case (int)IntranetActivityTypeEnum.Events:
                            src.Type = searchableTypeProvider.Get(SearchableType.Events.ToInt());
                            break;
                        case (int)IntranetActivityTypeEnum.News:
                            src.Type = searchableTypeProvider.Get(SearchableType.News.ToInt());
                            break;
                        case (int)IntranetActivityTypeEnum.Ideas:
                            src.Type = searchableTypeProvider.Get(SearchableType.Ideas.ToInt());
                            break;
                    }
                });

            base.Configure();
        }
    }
}