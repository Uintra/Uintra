using AutoMapper;
using uIntra.Core.Activity;

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
                    switch (dst.Type.Id)
                    {
                        case (int)IntranetActivityTypeEnum.Events:
                            src.Type = SearchableType.Events;
                            break;
                        case (int)IntranetActivityTypeEnum.News:
                            src.Type = SearchableType.News;
                            break;
                        case (int)IntranetActivityTypeEnum.Ideas:
                            src.Type = SearchableType.Ideas;
                            break;
                    }
                });

            base.Configure();
        }
    }
}