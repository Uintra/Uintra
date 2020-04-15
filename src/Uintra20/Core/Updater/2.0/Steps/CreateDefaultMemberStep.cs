using System.Web.Security;
using Uintra20.Infrastructure.Constants;
using UBaseline.Core.Extensions;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Core.Composing;
using LightInject;
using Umbraco.Web.Security.Providers;

namespace Uintra20.Core.Updater._2._0.Steps
{
    public class CreateDefaultMemberStep : IMigrationStep
    {
        private const string DefaultEmail = "admin@testmember.com";
        private const string DefaultName = "admin";
        private const int UmbracoAdminUserId = 0;
        private const string DefaultPassword = "qwerty1234";

        private readonly ILogger _logger;
        private readonly IMemberService _memberService;


        public CreateDefaultMemberStep()
        {
            _logger = Current.Factory.EnsureScope(f => f.GetInstance<ILogger>());
            _memberService = Current.Factory.EnsureScope(f => f.GetInstance<IMemberService>());
        }

        public ExecutionResult Execute()
        {
            _logger.Info<SetupDefaultMediaFoldersStep>("SetupDefaultMediaFoldersStep is running");
            AddDefaultMember();

            return ExecutionResult.Success;
        }

        public void Undo()
        {
        }

        private void AddDefaultMember()
        {
            var member = _memberService.GetByEmail(DefaultEmail);
            if (member != null)
            {
                _logger.Info<SetupDefaultMediaFoldersStep>("Default member already exists");
                return;
            }

            member = _memberService.CreateMember(DefaultName, DefaultEmail, DefaultName, UsersInstallationConstants.DataTypeAliases.Member);
            member.SetValue(UsersInstallationConstants.DataTypePropertyAliases.ProfileFirstName, DefaultName);
            member.SetValue(UsersInstallationConstants.DataTypePropertyAliases.ProfileLastName, DefaultName);
            member.SetValue(UsersInstallationConstants.DataTypePropertyAliases.ProfileRelatedUser, UmbracoAdminUserId);
            member.RawPasswordValue =(Membership.Providers["UmbracoMembershipProvider"] as MembersMembershipProvider).HashPasswordForStorage(DefaultPassword); ;
            
            _memberService.Save(member, raiseEvents: false);

            _memberService.AssignRole(member.Id, UsersInstallationConstants.MemberGroups.GroupWebMaster);
            
            _logger.Info<SetupDefaultMediaFoldersStep>("Default member has been created");
        }
    }
}