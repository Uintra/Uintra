using AutoMapper;
using uCommunity.Navigation.Core;

namespace Compent.uCommunity.Core.Navigation
{
    public class NavigationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<TopNavigationModel, TopMenuViewModel>()
                .ForMember(c => c.NotificationsUrl, o => o.Ignore());
        }
    }
}