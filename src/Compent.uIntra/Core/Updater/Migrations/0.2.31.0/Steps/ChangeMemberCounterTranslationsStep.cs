using Localization.Core;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._31._0.Steps
{
    public class ChangeMemberCounterTranslationsStep : IMigrationStep
    {
        private readonly ILocalizationCoreService _localizationCoreService;

        public ChangeMemberCounterTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {
            Test();
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new System.NotImplementedException();
        }

        private void Test()
        {
            var membersCount = _localizationCoreService.GetResourceModel("GroupInfo.MembersCount");
            if (membersCount != null && membersCount.Translations["en-US"].Equals("Members count"))
            {
                membersCount.Translations["en-US"] = "members";
                // _localizationCoreService.Update(membersCount);
            }

            var oneMemberCount = _localizationCoreService.GetResourceModel("GroupInfo.OneMemberCount");
            if (oneMemberCount == null)
            {
                oneMemberCount = new ResourceModel
                {
                    Key = "GroupInfo.OneMemberCount"
                };

                oneMemberCount.Translations.Add("en-US", "member");
                //_localizationCoreService.Create(oneMemberCount);
            }
        }
    }
}
