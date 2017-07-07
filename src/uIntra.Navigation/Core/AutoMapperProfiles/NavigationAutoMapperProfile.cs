using AutoMapper;

namespace uIntra.Navigation
{
    public class NavigationAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MenuItemModel, MenuItemViewModel>();
            Mapper.CreateMap<MenuModel, MenuViewModel>();
            Mapper.CreateMap<SubNavigationMenuModel, SubNavigationMenuViewModel>();
            Mapper.CreateMap<TopNavigationModel, TopNavigationViewModel>();

            Mapper.CreateMap<MyLinkItemModel, MyLinkItemViewModel>();

            Mapper.CreateMap<SystemLinksModel, SystemLinksViewModel>();
            Mapper.CreateMap<SystemLinkItemModel, SystemLinkItemViewModel>()
                .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Caption))
                .ForMember(dst => dst.Url, o => o.MapFrom(el => el.Link))
                .ForMember(dst => dst.Target, o => o.MapFrom(el => el.Target));
        }
    }
}
