using System.Reflection;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._1._0.Steps
{
    public class UpdateGridPageLayoutTemplateStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            var layoutEmbeddedResourceFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._3._1._0.PreValues.GridPageLayout.cshtml";
            InstallationStepsHelper.SetGridPageLayoutTemplateContent(layoutEmbeddedResourceFileName);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
        }
    }
}