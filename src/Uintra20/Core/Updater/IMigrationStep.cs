
namespace Uintra20.Core.Updater
{
    public interface IMigrationStep
    {
        ExecutionResult Execute();
        void Undo();
    }
}