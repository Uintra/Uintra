using Localization.Core;
using uIntra.Core.Installer;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class AddMemberCounterTranslationsStep : IMigrationStep
    {
        private const string GroupInfoMembersCountKey = "GroupInfo.MembersCount.lbl";
        private const string GroupInfoOneMemberCountKey = "GroupInfo.OneMemberCount.lbl";

        private const string GroupInfoMembersCountTranslation = "members";
        private const string GroupInfoOneMemberCountTranslation = "member";

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddMemberCounterTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            InstallationStepsHelper.AddTranslation(GroupInfoMembersCountKey, GroupInfoMembersCountTranslation);
            InstallationStepsHelper.AddTranslation(GroupInfoOneMemberCountKey, GroupInfoOneMemberCountTranslation);

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(GroupInfoMembersCountKey);
            _localizationCoreService.Delete(GroupInfoOneMemberCountKey);
        }
    }
}
