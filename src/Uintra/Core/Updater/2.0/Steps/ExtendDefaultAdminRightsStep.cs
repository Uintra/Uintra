using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Core.Composing;
using UBaseline.Core.Extensions;
using LightInject;

namespace Uintra.Core.Updater._2._0.Steps
{
    public class ExtendDefaultAdminRightsStep : IMigrationStep
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        private const string _localizationSectionAlias = "localization";
        private const string _notificationSectionAlias = "notificationSettings";
        private const string _sentMailsSectionAlias = "sentMails";

        public ExtendDefaultAdminRightsStep()
        {
            _logger = Current.Factory.EnsureScope(f => f.GetInstance<ILogger>());
            _userService = Current.Factory.EnsureScope(f => f.GetInstance<IUserService>());
            
        }
        public ExecutionResult Execute()
        {
            var adminGroup = _userService.GetUserGroupByAlias(Umbraco.Core.Constants.Security.AdminGroupAlias);
            adminGroup.AddAllowedSection(_localizationSectionAlias);
            _logger.Info<ExtendDefaultAdminRightsStep>("Section [Localization] has been added.");
            adminGroup.AddAllowedSection(_notificationSectionAlias);
            _logger.Info<ExtendDefaultAdminRightsStep>("Section [Notifications] has been added.");
            adminGroup.AddAllowedSection(_sentMailsSectionAlias);
            _logger.Info<ExtendDefaultAdminRightsStep>("Section [Sent Mails] has been added.");

            _userService.Save(adminGroup);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            var adminGroup = _userService.GetUserGroupByAlias(Umbraco.Core.Constants.Security.AdminGroupAlias);
            adminGroup.RemoveAllowedSection(_localizationSectionAlias);
            _logger.Info<ExtendDefaultAdminRightsStep>("Section [Localization] has been removed.");
            adminGroup.RemoveAllowedSection(_notificationSectionAlias);
            _logger.Info<ExtendDefaultAdminRightsStep>("Section [Notifications] has been removed.");
            adminGroup.RemoveAllowedSection(_sentMailsSectionAlias);
            _logger.Info<ExtendDefaultAdminRightsStep>("Section [Sent Mails] has been removed.");
        }
    }
}