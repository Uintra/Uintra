using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;
using Uintra.Users;
using Uintra.Users.Helpers;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._3._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private Dictionary<string, string> _translations { get; set; }

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
            _translations = ReflectionHelper.GetUserListTranslations("UserList.Table.{0}.lbl");
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
