using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Constants;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class DeleteGroupNavigationTabsMigrationStep : IMigrationStep
    {
        public ExecutionResult Execute()
        {
            DeleteNavigationComposition();

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            var navigationComposition = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;

            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDeactivatedGroupPage, navigationComposition);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsMembersPage, navigationComposition);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsEditPage, navigationComposition);
            InstallationStepsHelper.InheritCompositionForPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDocumentsPage, navigationComposition);
        }

        public void DeleteNavigationComposition()
        {
            var navigationComposition = NavigationInstallationConstants.DocumentTypeAliases.NavigationComposition;

            InstallationStepsHelper.DeleteCompositionFromPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDeactivatedGroupPage, navigationComposition);
            InstallationStepsHelper.DeleteCompositionFromPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsMembersPage, navigationComposition);
            InstallationStepsHelper.DeleteCompositionFromPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsEditPage, navigationComposition);
            InstallationStepsHelper.DeleteCompositionFromPage(GroupsInstallationConstants.DocumentTypeAliases.GroupsDocumentsPage, navigationComposition);
        }
    }
}