using AutoMapper;
using Uintra.Core.Extensions;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<PermissionManagementModel, PermissionViewModel>()
                .ForMember(dst => dst.ActionId, o => o.MapFrom(el => el.SettingIdentity.ActionType.ToInt()))
                .ForMember(dst => dst.ActivityTypeId, o => o.MapFrom(el => el.SettingIdentity.ActivityType.Map(EnumExtensions.ToInt).ToNullable()))
                .ForMember(dst => dst.IsAllowed, o => o.MapFrom(el => el.SettingValues.IsAllowed))
                .ForMember(dst => dst.IsEnabled, o => o.MapFrom(el => el.SettingValues.IsEnabled))
                .ForMember(dst => dst.IntranetMemberGroupId, o => o.MapFrom(el => el.Group.Id));
        }
    }
}