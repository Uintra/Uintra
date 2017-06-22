using AutoMapper;
using uIntra.Core.Activity;
using uIntra.Search.Core.Entities;

namespace uIntra.Search.Core
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
                    switch (dst.Type)
                    {
                        case IntranetActivityTypeEnum.Events:
                            src.Type = SearchableType.Events;
                            break;
                        case IntranetActivityTypeEnum.News:
                            src.Type = SearchableType.News;
                            break;
                        case IntranetActivityTypeEnum.Ideas:
                            src.Type = SearchableType.Ideas;
                            break;
                    }
                });

            //// Move to separate profile
            //Mapper.CreateMap<Event, SearchableActivity>()
            //    .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
            //    .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
            //    .IncludeBase<IntranetActivity, SearchableActivity>();

            //Mapper.CreateMap<Intranet.Core.News.Entities.News, SearchableActivity>()
            //    .ForMember(d => d.PublishedDate, o => o.MapFrom(s => s.PublishDate))
            //    .IncludeBase<IntranetActivity, SearchableActivity>();

            base.Configure();
        }
    }
}