using System;
using System.Web.Mvc;
using Compent.Uintra.Core.Updater;
using Compent.Uintra.Core.Updater.Migrations._1._3.Steps;
using Uintra.Core.MigrationHistories;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.User;
using Umbraco.Core;

namespace Compent.Uintra.Controllers.Api
{
    public class PermissionsController : PermissionsControllerBase
    {
        private readonly IMigrationHistoryService _migrationHistoryService;
        public PermissionsController(
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsService permissionsService,
            IPermissionResourceTypeProvider resourceTypeProvider,
            IPermissionActionTypeProvider actionTypeProvider,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IMigrationHistoryService migrationHistoryService)
            : base(intranetMemberGroupProvider, 
                permissionsService, 
                resourceTypeProvider, 
                actionTypeProvider, 
                intranetMemberService)
        {
            _migrationHistoryService = migrationHistoryService;
        }
        [HttpGet]
        public override GroupPermissionsViewModel Get(int memberGroupId)
        {
            ApplyPermissionMigration();

            return base.Get(memberGroupId);
        }

        public void ApplyPermissionMigration()
        {
            var migrationVersion = new Version("1.3");

            if(!_migrationHistoryService.Exists(typeof(SetupDefaultMemberGroupsPermissionsStep).Name, migrationVersion))
            {
                var step = DependencyResolver.Current.GetService<SetupDefaultMemberGroupsPermissionsStep>();

                var migrationItem = new MigrationItem(migrationVersion, step);

                var (executionHistory, executionResult) = MigrationHandler.TryExecuteSteps(migrationItem.AsEnumerableOfOne());
                if (executionResult.Type is ExecutionResultType.Success)
                {
                    _migrationHistoryService.Create(MigrationHandler.ToMigrationHistory(executionHistory));
                }
            }
        }
    }
}
