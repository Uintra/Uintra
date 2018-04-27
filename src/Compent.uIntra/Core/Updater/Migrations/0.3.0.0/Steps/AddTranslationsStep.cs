using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private const string PopupNotificationOkBtnTextKey = "PopupNotification.OkBtn.Text";
        private const string LightboxGalleryCountOneKey = "LightboxGallery.Count.One.lbl";
        private const string LightboxGalleryCountManyKey = "LightboxGallery.Count.Many.lbl";
        private const string ProfileLinkTitleKey = "PopupNotification.ProfileLink.Title";
        private const string SubscribeSettingsTitleKey = "SubscribeSettings.Title.lbl";
        private const string SubscribeSettingsSubscribeNotesKey = "SubscribeSettings.SubscribeNotes.lbl";
        private const string EventsDescriptionPlaceholderKey = "Events.Description.placeholder";
        private const string PinAcceptPinTextKey = "Pin.AcceptPinText.lbl";
        private const string GroupRoomNavigationAllKey = "GroupRoom.Navigation.All.lbl";

        private const string PopupNotificationOkBtnTextTranslation = "Close";
        private const string LightboxGalleryCountOneTranslation = "File added";
        private const string LightboxGalleryCountManyTranslation = "Files added";
        private const string ProfileLinkTitleTranslation = "Go to user profile";
        private const string SubscribeSettingsTitleTranslation = "Event participation";
        private const string SubscribeSettingsSubscribeNotesTranslation = "SubscribeSettings.SubscribeNotes.lbl";
        private const string EventsDescriptionPlaceholderTranslation = "Description";
        private const string PinAcceptPinTextTranslation = "Note: Enabling this will mark the post as \"Important\". It will be pinned to the top until you manually unpin it. To automatically unpin the post set the date below, the post will remain being marked \"Important\"";
        private const string GroupRoomNavigationAllTranslation= "Feed";

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
            InstallationStepsHelper.AddTranslation(ProfileLinkTitleKey, ProfileLinkTitleTranslation);
            InstallationStepsHelper.AddTranslation(SubscribeSettingsTitleKey, SubscribeSettingsTitleTranslation);
            InstallationStepsHelper.AddTranslation(SubscribeSettingsSubscribeNotesKey, SubscribeSettingsSubscribeNotesTranslation);
            InstallationStepsHelper.AddTranslation(EventsDescriptionPlaceholderKey, EventsDescriptionPlaceholderTranslation);
            InstallationStepsHelper.AddTranslation(PinAcceptPinTextKey, PinAcceptPinTextTranslation);
            InstallationStepsHelper.AddTranslation(GroupRoomNavigationAllKey, GroupRoomNavigationAllTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(PopupNotificationOkBtnTextKey);
            _localizationCoreService.Delete(LightboxGalleryCountOneKey);
            _localizationCoreService.Delete(LightboxGalleryCountManyKey);
            _localizationCoreService.Delete(ProfileLinkTitleKey);
            _localizationCoreService.Delete(SubscribeSettingsTitleKey);
            _localizationCoreService.Delete(SubscribeSettingsSubscribeNotesKey);
            _localizationCoreService.Delete(EventsDescriptionPlaceholderKey);
            _localizationCoreService.Delete(PinAcceptPinTextKey);
            _localizationCoreService.Delete(GroupRoomNavigationAllKey);
        }
    }
}
