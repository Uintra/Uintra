namespace Compent.Uintra.Core.Updater
{
    public class GroupStep: IMigrationStep
    {
        public ExecutionResult Execute()
        {
            return new ExecutionResult(ExecutionResultType.Success);
        }

        public void Undo()
        {
            
        }
    }
}