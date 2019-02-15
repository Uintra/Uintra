using System.Linq;
using AutoMapper;
using Uintra.Core.Extensions;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Models
{
    public class PermissionsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<PermissionManagementModel, PermissionViewModel>()
                .ForMember(dst => dst.ActionId, o => o.MapFrom(el => el.SettingIdentity.ActionType.ToInt()))
                .ForMember(dst => dst.ActionName, o => o.MapFrom(el => el.SettingIdentity.ActionType.ToString()))
                .ForMember(dst => dst.ActivityTypeId, o => o.MapFrom(el => el.SettingIdentity.ActivityType.Map(EnumExtensions.ToInt).ToNullable()))
                .ForMember(dst => dst.ActivityTypeName, o => o.MapFrom(el => el.SettingIdentity.ActivityType.Map(toString).FirstOrDefault()))
                .ForMember(dst => dst.Allowed, o => o.MapFrom(el => el.SettingValues.IsAllowed))
                .ForMember(dst => dst.Enabled, o => o.MapFrom(el => el.SettingValues.IsEnabled))
                .ForMember(dst => dst.IntranetMemberGroupId, o => o.MapFrom(el => el.Group.Id));
        }
    }
}