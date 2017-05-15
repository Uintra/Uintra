using AutoMapper;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.DefaultImplementation;

namespace uCommunity.Navigation.AutoMapperProfiles
{
    public class NavigationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MenuItemModel, MenuItemViewModel>();
            Mapper.CreateMap<MenuModel, MenuViewModel>();
            Mapper.CreateMap<SubNavigationMenuModel, SubNavigationMenuViewModel>();
            Mapper.CreateMap<TopNavigationModel, TopNavigationViewModel>();

            Mapper.CreateMap<MyLinksModel, MyLinksViewModel>()
                .ForMember(dst => dst.PageName, o => o.Ignore())
                .ForMember(dst => dst.IsLinked, o => o.Ignore());

            Mapper.CreateMap<MyLinkItemModel, MyLinkItemViewModel>();
            Mapper.CreateMap<MyLink, MyLinkItemModel>();
            Mapper.CreateMap<SystemLinksModel, SystemLinksViewModel>();
            Mapper.CreateMap<SystemLinkItemModel, SystemLinkItemViewModel>();
        }
    }
}
