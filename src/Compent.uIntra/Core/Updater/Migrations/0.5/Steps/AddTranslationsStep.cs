using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;
using Uintra.Users.Helpers;

namespace Compent.Uintra.Core.Updater.Migrations._0._5.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private Dictionary<string, string> _translations { get; set; }

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
            _translations = ReflectionHelper.GetUserListTranslations("UserList.Table.{0}.lbl");
            _translations.Add("UserList.Search.Placeholder.lbl", "Search for name, phone number, email or anything else");
            _translations.Add("UserList.DetailsPopup.Title", "profile");
            _translations.Add("UserList.DetailsPopup.Tags.lbl", "Tags subscription");
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
