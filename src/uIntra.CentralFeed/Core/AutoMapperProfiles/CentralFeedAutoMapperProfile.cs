using AutoMapper;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class CentralFeedAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<FeedSettings, FeedTabSettings>()
                .ForMember(d => d.Type, d => d.MapFrom(i => new IntranetType {Id = i.Type.ToInt(), Name = i.Type.ToString()}));
            Mapper.CreateMap<ActivityFeedTabModel, ActivityFeedTabViewModel>();
        }
    }
}
