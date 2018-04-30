using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private readonly Dictionary<string, string> _translations = new Dictionary<string, string>()
        {
            {"PopupNotification.OkBtn.Text", "Close"},
            {"LightboxGallery.Count.One.lbl", "File added"},
            {"LightboxGallery.Count.Many.lbl", "Files added"},
            {"PopupNotification.ProfileLink.Title", "Go to user profile"},
            {"SubscribeSettings.Title.lbl", "Event participation"},
            {"Events.Description.placeholder", "Description"},
            {
                "Pin.AcceptPinText.lbl",
                "Note: Enabling this will mark the post as \"Important\". It will be pinned to the top until you manually unpin it. To automatically unpin the post set the date below, the post will remain being marked \"Important\""
            },
            {"GroupRoom.Navigation.All.lbl", "Feed"},
            {"Profile.EmailNotifications.Title.lbl", "Notifications"},
            {"GroupDocument.Upload.lbl", "Upload new group document"},
            {"DocumentsList.Empty.lbl", "Documents list is empty"},
            {"GroupEdit.GroupImage.note", "Group image"}

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
