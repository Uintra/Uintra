using System;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class AddFirstTimeLoginStep : IMigrationStep
    {
        private const string FirstLoginPerformedPropertyName = "firstLoginPerformed";
        public ExecutionResult Execute()
        {
            AddFirstLoginPerformedProperty();
            SetupMembers();

            return ExecutionResult.Success;
        }

        public void SetupMembers()
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            var members = memberService.GetAll(0, Int32.MaxValue, out _);

            foreach (var member in members)
            {
                member.SetValue(FirstLoginPerformedPropertyName, (member.LastLoginDate - member.CreateDate).TotalMinutes > 3);
                memberService.Save(member, raiseEvents: false);
            }
        }

        public void AddFirstLoginPerformedProperty()
        {
            var memberTypeService = ApplicationContext.Current.Services.MemberTypeService;
            var memberType = memberTypeService.Get(UsersInstallationConstants.DataTypeAliases.Member);

            var firstLoginProperty = new PropertyType("Umbraco.TrueFalse", DataTypeDatabaseType.Nvarchar)
            {
                Alias = FirstLoginPerformedPropertyName,
                Name = "First login performed"
            };

            if (!memberType.PropertyTypeExists(firstLoginProperty.Alias))
            {
                memberType.AddPropertyType(firstLoginProperty, UsersInstallationConstants.DataTypeTabAliases.MembershipTabAlias);
            }

            memberTypeService.Save(memberType);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}