using System.Linq;
using System.Runtime.Caching;
using LightInject;
using UBaseline.Core.Extensions;
using Uintra20.Features.Permissions.Interfaces;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;

namespace Uintra20.Core.Updater._2._0.Steps
{
    public class SetupDefaultMemberGroupsStep : IMigrationStep
    {
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly ILogger _logger;

        public SetupDefaultMemberGroupsStep()
        {
            _logger = Current.Factory.EnsureScope(f => f.GetInstance<ILogger>());
            _intranetMemberGroupService = Current.Factory.EnsureScope(s=>s.GetInstance<IIntranetMemberGroupService>());
        }

        public ExecutionResult Execute()
        {        
           _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("SetupDefaultMemberGroupsStep is running");
            var memberGroups = _intranetMemberGroupService.GetAll();

            if (!memberGroups.Any(mg=>mg.Name=="UiUser"))
            {
                _intranetMemberGroupService.Create("UiUser");
                _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("UI User role has been created");
            }
            
            if (!memberGroups.Any(mg=>mg.Name=="UiPublisher"))
            {
                _intranetMemberGroupService.Create("UiPublisher");
                _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("UI Publisher role has been created");
            }
            
            if (!memberGroups.Any(mg=>mg.Name=="WebMaster"))
            {
                _intranetMemberGroupService.Create("WebMaster");
                _logger.Info<SetupDefaultMemberGroupsPermissionsStep>("Web Master role has been created");
            }
            
            MemoryCache.Default.Trim(100);
            
            return ExecutionResult.Success;
        }
        
        public void Undo()
        {
        }
    }
}