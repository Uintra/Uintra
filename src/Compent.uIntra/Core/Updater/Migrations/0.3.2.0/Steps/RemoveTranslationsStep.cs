using Localization.Core;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._2._0.Steps
{
    public class RemoveTranslationsStep : IMigrationStep
    {
        private readonly ILocalizationCoreService _localizationCoreService;

        public RemoveTranslationsStep(ILocalizationCoreService localizationCoreService)
        {
            _localizationCoreService = localizationCoreService;
        }

        public ExecutionResult Execute()
        {

            _localizationCoreService.Delete("GroupEdit.GroupImage.note");
            _localizationCoreService.Delete("GroupCreate.GroupImage.note");

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            
        }
    }
}