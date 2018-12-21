using System;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Updater.Migrations._1._2.Steps
{
    public class AddPhoneAndDepartmentToUserStep : IMigrationStep
    {
        private readonly IMemberTypeService _memberTypeService;

        public AddPhoneAndDepartmentToUserStep(IMemberTypeService memberTypeService)
        {
            _memberTypeService = memberTypeService;
        }

        public ExecutionResult Execute()
        {
            var memberType = _memberTypeService.Get(UsersInstallationConstants.DataTypeAliases.Member);

            var phoneProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.Phone,
                Name = UsersInstallationConstants.DataTypePropertyNames.Phone
            };
            var departmentProperty = new PropertyType("Umbraco.Textbox", DataTypeDatabaseType.Nvarchar)
            {
                Alias = UsersInstallationConstants.DataTypePropertyAliases.Department,
                Name = UsersInstallationConstants.DataTypePropertyNames.Department
            };

            var profileTab = UsersInstallationConstants.DataTypeTabAliases.ProfileTabAlias;

            if (!memberType.PropertyTypeExists(phoneProperty.Alias))
            {
                memberType.AddPropertyType(phoneProperty, profileTab);
            }

            if (!memberType.PropertyTypeExists(departmentProperty.Alias))
            {
                memberType.AddPropertyType(departmentProperty, profileTab);
            }

            _memberTypeService.Save(memberType);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}