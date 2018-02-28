using AutoMapper;
using Uintra.CentralFeed.Navigation.Models;

namespace Uintra.CentralFeed
{
    public class CentralFeedAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<FeedSettings, FeedTabSettings>();
            Mapper.CreateMap<ActivityFeedTabModel, ActivityFeedTabViewModel>();
        }
    }
}
