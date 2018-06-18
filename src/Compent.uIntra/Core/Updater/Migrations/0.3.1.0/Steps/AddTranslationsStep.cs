using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._1._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private readonly Dictionary<string, string> _translations = new Dictionary<string, string>()
        {
            { "LightboxGallery.AttachedFiles.lbl", "Attached files" }
        };

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            foreach (var translation in _translations)
            {
                InstallationStepsHelper.AddTranslation(translation.Key, translation.Value);
            }

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            foreach (var translationKey in _translations.Keys)
            {
                _localizationCoreService.Delete(translationKey);
            }         
        }
    }
}
