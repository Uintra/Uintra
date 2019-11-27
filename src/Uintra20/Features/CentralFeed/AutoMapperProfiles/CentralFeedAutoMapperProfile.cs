using AutoMapper;
using Uintra20.Core.Feed;
using Uintra20.Features.CentralFeed.Entities;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.CentralFeed.AutoMapperProfiles
{
    public class CentralFeedAutoMapperProfile : Profile
    {
        public CentralFeedAutoMapperProfile()
        {
			CreateMap<FeedSettings, FeedTabSettings>();
			CreateMap<ActivityFeedTabModel, ActivityFeedTabViewModel>();
		}
    }
}