using AutoMapper;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Settings;
using Uintra.Features.Navigation.Models;

namespace Uintra.Features.CentralFeed.AutoMapperProfiles
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