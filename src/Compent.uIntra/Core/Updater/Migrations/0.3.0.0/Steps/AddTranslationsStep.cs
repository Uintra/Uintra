using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private const string PopupNotificationOkBtnTextKey = "PopupNotification.OkBtn.Text";

        private const string PopupNotificationOkBtnTextTranslation = "Close";

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            InstallationStepsHelper.AddTranslation(PopupNotificationOkBtnTextKey, PopupNotificationOkBtnTextTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(PopupNotificationOkBtnTextKey);
        }
    }
}
