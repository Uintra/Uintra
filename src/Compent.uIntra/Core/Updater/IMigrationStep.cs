
using System;

namespace Compent.Uintra.Core.Updater
{
    public interface IMigrationStep
    {
        ExecutionResult Execute();
        void Undo();
    }

    public class ExecutionResult
    {
        public ExecutionResultType Type { get; }
        public Exception Exception { get; }

        public ExecutionResult(ExecutionResultType type, Exception exception = null)
        {
            Type = type;
            Exception = exception;
        }

        public static ExecutionResult Success => new ExecutionResult(ExecutionResultType.Success);
        public static ExecutionResult Failure(Exception exception) => new ExecutionResult(ExecutionResultType.Failure, exception);
    }

    public enum ExecutionResultType
    {
        Success,
        Failure
    }
}