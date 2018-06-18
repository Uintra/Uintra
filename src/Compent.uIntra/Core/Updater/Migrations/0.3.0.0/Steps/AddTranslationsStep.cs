using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private const string PopupNotificationOkBtnTextKey = "PopupNotification.OkBtn.Text";
        private const string LightboxGalleryCountOneKey = "LightboxGallery.Count.One.lbl";
        private const string LightboxGalleryCountManyKey = "LightboxGallery.Count.Many.lbl";

        private const string PopupNotificationOkBtnTextTranslation = "Close";
        private const string LightboxGalleryCountOneTranslation = "File added";
        private const string LightboxGalleryCountManyTranslation = "Files added";

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            InstallationStepsHelper.AddTranslation(PopupNotificationOkBtnTextKey, PopupNotificationOkBtnTextTranslation);
            InstallationStepsHelper.AddTranslation(LightboxGalleryCountOneKey, LightboxGalleryCountOneTranslation);
            InstallationStepsHelper.AddTranslation(LightboxGalleryCountManyKey, LightboxGalleryCountManyTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(PopupNotificationOkBtnTextKey);
            _localizationCoreService.Delete(LightboxGalleryCountOneKey);
            _localizationCoreService.Delete(LightboxGalleryCountManyKey);
        }
    }
}
