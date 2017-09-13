using AutoMapper;

namespace uIntra.CentralFeed
{
    public class CentralFeedAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<FeedTabModel, FeedTabViewModel>()
                .ForMember(d => d.Url, o => o.MapFrom(el => el.Content.Url));
            Mapper.CreateMap<FeedSettings, FeedTabSettings>();
        }
    }
}
