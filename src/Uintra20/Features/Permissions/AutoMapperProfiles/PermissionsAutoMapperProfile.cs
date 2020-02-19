using AutoMapper;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models;

namespace Uintra20.Features.Permissions.AutoMapperProfiles
{
    public class PermissionsAutoMapperProfile : Profile
    {
	    public PermissionsAutoMapperProfile()
        {
            CreateMap<IMemberGroup, IntranetMemberGroup>()
                .ForMember(dst => dst.Id, o => o.MapFrom(el => el.Id))
                .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Name));

            CreateMap<IntranetMemberGroup, MemberGroupViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(el => el.Id))
                .ForMember(dst => dst.Name, o => o.MapFrom(el => el.Name));

            CreateMap<PermissionManagementModel, PermissionViewModel>()
                .ForMember(dst => dst.ActionId, o => o.MapFrom(el => el.SettingIdentity.Action.ToInt()))
                .ForMember(dst => dst.ActionName, o => o.MapFrom(el => el.SettingIdentity.Action.GetDisplayName()))
                .ForMember(dst => dst.ParentActionId, o => o.MapFrom(el => el.ParentActionType.ToNullableInt()))
                .ForMember(dst => dst.ResourceTypeId, o => o.MapFrom(el => el.SettingIdentity.ResourceType.ToInt()))
                .ForMember(dst => dst.ResourceTypeName, o => o.MapFrom(el => el.SettingIdentity.ResourceType.GetDisplayName()))
                .ForMember(dst => dst.Allowed, o => o.MapFrom(el => el.SettingValues.IsAllowed))
                .ForMember(dst => dst.Enabled, o => o.MapFrom(el => el.SettingValues.IsEnabled))
                .ForMember(dst => dst.IntranetMemberGroupId, o => o.MapFrom(el => el.Group.Id));
        }
    }
}