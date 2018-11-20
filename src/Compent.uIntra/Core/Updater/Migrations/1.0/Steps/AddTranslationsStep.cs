using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._1._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            InstallationStepsHelper.UpdateTranslation("Comments.Empty.lbl", "Comments", "Comment");

            return ExecutionResult.Success;
        }

        public void Undo()
        {

        }
    }
}
