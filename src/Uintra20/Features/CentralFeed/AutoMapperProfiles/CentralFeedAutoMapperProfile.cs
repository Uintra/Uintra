using AutoMapper;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;
using Uintra20.Features.Navigation.Models;

namespace Uintra20.Features.CentralFeed.AutoMapperProfiles
{
    public class CentralFeedAutoMapperProfile : Profile
    {
        public CentralFeedAutoMapperProfile()
        {
			CreateMap<FeedSettings, FeedTabSettings>();
            CreateMap<ActivityFeedTabModel, ActivityFeedTabViewModel>()
                .ForMember(dst => dst.Title, o => o.Ignore())
                .ForMember(dst => dst.Filters, o => o.Ignore());
        }
    }
}