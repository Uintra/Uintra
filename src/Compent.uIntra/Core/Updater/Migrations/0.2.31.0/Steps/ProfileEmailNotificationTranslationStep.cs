using Uintra.Core.Installer;

namespace Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class ChangeProfileEmailNotificationTranslationStep : IMigrationStep
    {
        private const string ProfileEmailNotificationsKey = "Profile.EmailNotifications.lbl";
        private const string ProfileEmailNotificationsTranslation = "Send me email notifications";
        private const string OldProfileEmailNotificationsTranslation = "Do not send me email notifications";


        public ExecutionResult Execute()
        {
            InstallationStepsHelper.UpdateTranslation(ProfileEmailNotificationsKey, OldProfileEmailNotificationsTranslation, ProfileEmailNotificationsTranslation);
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            InstallationStepsHelper.UpdateTranslation(ProfileEmailNotificationsKey, ProfileEmailNotificationsTranslation, OldProfileEmailNotificationsTranslation);
        }
    }
}
