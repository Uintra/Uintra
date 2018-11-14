using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using static Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants.UsersInstallationConstants;

namespace Compent.Uintra.Core.Updater.Migrations._1._0.Steps
{
    public class UpdateRelatedUserPropertyStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var relatedUserDataType =
                dataTypeService.GetDataTypeDefinitionByName(DataTypeNames.MemberUserPicker);
            if (relatedUserDataType != null && !relatedUserDataType.PropertyEditorAlias.Equals(
                DataTypePropertyEditors.MemberUserPicker))
            {
                relatedUserDataType.PropertyEditorAlias = DataTypePropertyEditors.MemberUserPicker;
                dataTypeService.Save(relatedUserDataType);
            }
            return ExecutionResult.Success;
        }

        public void Undo()
        {
        }
    }
}