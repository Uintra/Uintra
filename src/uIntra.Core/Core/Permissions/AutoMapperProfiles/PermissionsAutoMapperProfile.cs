using AutoMapper;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;
using Umbraco.Core.Models;

namespace Uintra.Core.Permissions
{
    public class PermissionsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IMemberGroup, IntranetMemberGroup>()
                .ForMember(dst => dst.Id, o => o.MapFrom(el => el.Id))
                .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Name));

            Mapper.CreateMap<IntranetMemberGroup, MemberGroupViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(el => el.Id))
                .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Name));

            Mapper.CreateMap<PermissionManagementModel, PermissionViewModel>()
                .ForMember(dst => dst.ActionId, o => o.MapFrom(el => el.SettingIdentity.ActionType.ToInt()))
                .ForMember(dst => dst.ActionName, o => o.MapFrom(el => el.SettingIdentity.ActionType.GetDisplayName()))
                .ForMember(dst => dst.ParentActionId, o => o.MapFrom(el => el.ParentActionType.ToNullableInt()))
                .ForMember(dst => dst.ResourceTypeId, o => o.MapFrom(el => el.SettingIdentity.ResourceType.ToNullableInt()))
                .ForMember(dst => dst.ResourceTypeName, o => o.MapFrom(el => el.SettingIdentity.ResourceType.GetDisplayName()))
                .ForMember(dst => dst.Allowed, o => o.MapFrom(el => el.SettingValues.IsAllowed))
                .ForMember(dst => dst.Enabled, o => o.MapFrom(el => el.SettingValues.IsEnabled))
                .ForMember(dst => dst.IntranetMemberGroupId, o => o.MapFrom(el => el.Group.Id));
        }
    }
}