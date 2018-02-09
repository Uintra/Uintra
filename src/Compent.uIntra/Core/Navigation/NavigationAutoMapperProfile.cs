using AutoMapper;
using Uintra.Navigation;

namespace Compent.Uintra.Core.Navigation
{
    public class NavigationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<TopNavigationModel, TopMenuViewModel>()
                .ForMember(c => c.NotificationsUrl, o => o.Ignore())
                .ForMember(c => c.NotificationList, o => o.Ignore());
        }
    }
}