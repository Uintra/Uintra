
namespace Uintra.Core.Updater
{
    public interface IMigrationStep
    {
        ExecutionResult Execute();
        void Undo();
    }
}