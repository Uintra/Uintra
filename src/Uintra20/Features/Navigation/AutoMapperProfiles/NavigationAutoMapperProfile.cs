using System.Collections.Generic;
using AutoMapper;
using UBaseline.Core.Navigation;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Navigation.AutoMapperProfiles
{
    public class NavigationAutoMapperProfile : Profile
    {
        public  NavigationAutoMapperProfile()
        {
            //For new Uintra, need to check maybe it would be solved by automapper itself

            CreateMap<TopNavigationModel, TopNavigationViewModel>()
                .ForMember(dst => dst.Items, o => o.MapFrom(m => m.Items.Map<IEnumerable<TopNavigationItemViewModel>>()));


            CreateMap<TreeNavigationItemModel, MenuItemViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dst => dst.IsActive, o => o.MapFrom(src => src.IsActive))
                .ForMember(dst => dst.IsClickable, o => o.MapFrom(src => src.IsActive))
                .ForMember(dst => dst.IsHeading, o => o.MapFrom(src => false))
                .ForMember(dst => dst.IsHomePage, o => o.MapFrom(src => src.Level == 1))
                .ForMember(dst => dst.Name, o => o.MapFrom(src => src.Title))
                .ForMember(dst => dst.Url, o => o.MapFrom(src => src.Url))
                .ForMember(dst => dst.Children, o => o.MapFrom(src => src.Children));

            CreateMap<MenuItemModel, MenuItemViewModel>();
            CreateMap<MenuModel, MenuViewModel>();
            CreateMap<TreeNavigationItemModel, MenuViewModel>()
                .ForMember(dst => dst.MenuItems, o => o.MapFrom(src => src.Children));
            CreateMap<SubNavigationMenuModel, SubNavigationMenuViewModel>();
            //CreateMap<TopNavigationModel, TopNavigationViewModel>();

            //Mapper.CreateMap<MyLinkItemModel, MyLinkItemViewModel>();

            CreateMap<SystemLinksModel, SystemLinksViewModel>();
            CreateMap<SystemLinkItemModel, SystemLinkItemViewModel>()
                .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Caption))
                .ForMember(dst => dst.Url, o => o.MapFrom(el => el.Link))
                .ForMember(dst => dst.Target, o => o.MapFrom(el => el.Target));

            CreateMap<UserListLinkModel, UserListLinkViewModel>();

            //CreateMap<TopNavigationModel, TopMenuViewModel>()
            //    .ForMember(c => c.NotificationsUrl, o => o.Ignore())
            //    .ForMember(c => c.NotificationList, o => o.Ignore());
        }
    }
}