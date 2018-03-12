using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;
using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class AddTranslationsStep : IMigrationStep
    {
        private const string AttachedDocumentsKey = "LightboxGallery.AttachedDocuments.lbl";
        private const string GroupInfoMembersCountKey = "GroupInfo.MembersCount.lbl";
        private const string GroupInfoOneMemberCountKey = "GroupInfo.OneMemberCount.lbl";
        private const string ActivityPinnedKey = "Activity.Pinned.lbl";
        private const string GroupMembersDeleteTextKey = "GroupMembers.Delete.Text.lbl";
        private const string ActivityLocationKey = "Activity.Location.lbl";
        private const string PagePromotionLocationKey = "PagePromotion";
        private const string SearchTagKey = "Search.Tag";

        private const string AttachedDocumentsTranslation = "Attached documents:";
        private const string GroupInfoMembersCountTranslation = "members";
        private const string GroupInfoOneMemberCountTranslation = "member";
        private const string ActivityPinnedTranslation = "Important";
        private const string GroupMembersDeleteTextOldTranslation = "Delete";
        private const string GroupMembersDeleteTextNewTranslation = "Are you sure?";
        private const string ActivityLocationTranslation = "Location";
        private const string PagePromotionTranslation = "Promotion";
        private const string SearchTagTranslation = "Tag";

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
            InstallationStepsHelper.AddTranslation(ActivityLocationKey, ActivityLocationTranslation);
            InstallationStepsHelper.AddTranslation(PagePromotionLocationKey, PagePromotionTranslation);
            InstallationStepsHelper.AddTranslation(SearchTagKey, SearchTagTranslation);

            InstallationStepsHelper.UpdateTranslation(GroupMembersDeleteTextKey, GroupMembersDeleteTextOldTranslation, GroupMembersDeleteTextNewTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(AttachedDocumentsKey);
            _localizationCoreService.Delete(GroupInfoMembersCountKey);
            _localizationCoreService.Delete(GroupInfoOneMemberCountKey);
            _localizationCoreService.Delete(ActivityPinnedKey);
            _localizationCoreService.Delete(ActivityLocationKey);
            _localizationCoreService.Delete(PagePromotionLocationKey);
            _localizationCoreService.Delete(SearchTagKey);

            InstallationStepsHelper.UpdateTranslation(GroupMembersDeleteTextKey, GroupMembersDeleteTextNewTranslation, GroupMembersDeleteTextOldTranslation);
        }
    }
}
