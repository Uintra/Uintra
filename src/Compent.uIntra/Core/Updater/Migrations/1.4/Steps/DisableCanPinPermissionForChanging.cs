using System;
using System.Linq;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;

namespace Compent.Uintra.Core.Updater.Migrations._1._4.Steps
{
	public class DisableCanPinPermissionForChanging : IMigrationStep
	{
		private readonly IPermissionsService _permissionsService;
		private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;

		public DisableCanPinPermissionForChanging(
			IPermissionsService permissionsService,
			IIntranetMemberGroupProvider intranetMemberGroupProvider)
		{
			_permissionsService = permissionsService;
			_intranetMemberGroupProvider = intranetMemberGroupProvider;
		}
		public ExecutionResult Execute()
		{
			var uiUserGroup = _intranetMemberGroupProvider.All.FirstOrDefault(g => g.Name.ToLowerInvariant() == "uiuser");
			if (uiUserGroup == null)
			{
				return ExecutionResult.Success;
			}

			_permissionsService.Save(PermissionUpdateModel.Of(
				uiUserGroup,
				new PermissionSettingValues(false, false),
				new PermissionSettingIdentity(PermissionActionEnum.CanPin, PermissionResourceTypeEnum.Events)));

			_permissionsService.Save(PermissionUpdateModel.Of(
				uiUserGroup,
				new PermissionSettingValues(false, false),
				new PermissionSettingIdentity(PermissionActionEnum.CanPin, PermissionResourceTypeEnum.News)));

			return ExecutionResult.Success;
		}

		public void Undo()
		{
			throw new NotImplementedException();
		}
	}
}