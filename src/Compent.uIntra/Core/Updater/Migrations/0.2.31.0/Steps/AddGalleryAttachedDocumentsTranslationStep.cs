using Localization.Core;
using uIntra.Core.Installer;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class AddGalleryAttachedDocumentsTranslationStep : IMigrationStep
    {
        private const string AttachedDocumentsKey = "LightboxGallery.AttachedDocuments.lbl";
        private const string AttachedDocumentsTranslation = "Attached documents:";

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddGalleryAttachedDocumentsTranslationStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            InstallationStepsHelper.AddTranslation(AttachedDocumentsKey, AttachedDocumentsTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(AttachedDocumentsKey);
        }
    }
}
