using AutoMapper;

namespace Uintra20.Features.Navigation.AutoMapperProfiles
{
    public class NavigationAutoMapperProfile : Profile
    {
        public  NavigationAutoMapperProfile()
        {
            //For new Uintra, need to check maybe it would be solved by automapper itself

            //CreateMap<TopNavigationModel, TopNavigationViewModel>()
            //    .ForMember(dst => dst.Items, o => o.MapFrom(m => m.Items.Map<IEnumerable<TopNavigationItemViewModel>>()));




            //Mapper.CreateMap<MenuItemModel, MenuItemViewModel>();
            //Mapper.CreateMap<MenuModel, MenuViewModel>();
            //Mapper.CreateMap<SubNavigationMenuModel, SubNavigationMenuViewModel>();
            //Mapper.CreateMap<TopNavigationModel, TopNavigationViewModel>();

            //Mapper.CreateMap<MyLinkItemModel, MyLinkItemViewModel>();

            //Mapper.CreateMap<SystemLinksModel, SystemLinksViewModel>();
            //Mapper.CreateMap<SystemLinkItemModel, SystemLinkItemViewModel>()
            //    .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Caption))
            //    .ForMember(dst => dst.Url, o => o.MapFrom(el => el.Link))
            //    .ForMember(dst => dst.Target, o => o.MapFrom(el => el.Target));

            //Mapper.CreateMap<UserListLinkModel, UserListLinkViewModel>();

            //Mapper.CreateMap<TopNavigationModel, TopMenuViewModel>()
            //    .ForMember(c => c.NotificationsUrl, o => o.Ignore())
            //    .ForMember(c => c.NotificationList, o => o.Ignore());
        }
    }
}