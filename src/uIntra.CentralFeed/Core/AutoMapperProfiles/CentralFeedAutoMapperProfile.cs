using AutoMapper;

namespace uIntra.CentralFeed
{
    public class CentralFeedAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<CentralFeedTabModel, CentralFeedTabViewModel>()
                .ForMember(d => d.Url, o => o.MapFrom(el => el.Content.Url));
        }
    }
}
