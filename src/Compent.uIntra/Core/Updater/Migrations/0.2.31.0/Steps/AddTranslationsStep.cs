using Localization.Core;
using uIntra.Core.Installer;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private const string AttachedDocumentsKey = "LightboxGallery.AttachedDocuments.lbl";
        private const string GroupInfoMembersCountKey = "GroupInfo.MembersCount.lbl";
        private const string GroupInfoOneMemberCountKey = "GroupInfo.OneMemberCount.lbl";
        private const string ActivityPinnedKey = "Activity.Pinned.lbl";

        private const string AttachedDocumentsTranslation = "Attached documents:";
        private const string GroupInfoMembersCountTranslation = "members";
        private const string GroupInfoOneMemberCountTranslation = "member";
        private const string ActivityPinnedTranslation = "Important";

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            InstallationStepsHelper.AddTranslation(AttachedDocumentsKey, AttachedDocumentsTranslation);
            InstallationStepsHelper.AddTranslation(GroupInfoMembersCountKey, GroupInfoMembersCountTranslation);
            InstallationStepsHelper.AddTranslation(GroupInfoOneMemberCountKey, GroupInfoOneMemberCountTranslation);
            InstallationStepsHelper.AddTranslation(ActivityPinnedKey, ActivityPinnedTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(AttachedDocumentsKey);
            _localizationCoreService.Delete(GroupInfoMembersCountKey);
            _localizationCoreService.Delete(GroupInfoOneMemberCountKey);
            _localizationCoreService.Delete(ActivityPinnedKey);
        }
    }
}
