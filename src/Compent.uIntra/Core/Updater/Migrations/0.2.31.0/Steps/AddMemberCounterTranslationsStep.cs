using Localization.Core;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class AddMemberCounterTranslationsStep : IMigrationStep
    {
        private const string GroupInfoMembersCountKey = "GroupInfo.MembersCount.lbl";
        private const string GroupInfoOneMemberCountKey = "GroupInfo.OneMemberCount.lbl";
        private const string EnglishCultureKey = "en-US";

        private readonly ILocalizationCoreService _localizationCoreService;

        public AddMemberCounterTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            AddTranslation(GroupInfoMembersCountKey, "members");
            AddTranslation(GroupInfoOneMemberCountKey, "member");

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            _localizationCoreService.Delete(GroupInfoMembersCountKey);
            _localizationCoreService.Delete(GroupInfoOneMemberCountKey);
        }

        private void AddTranslation(string key, string translation)
        {
            var resourceModel = _localizationCoreService.GetResourceModel(key);
            if (resourceModel.Translations[EnglishCultureKey].Contains(key))
            {
                resourceModel.Translations[EnglishCultureKey] = translation;
                _localizationCoreService.Create(resourceModel);
            }
        }
    }
}
