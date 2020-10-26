using System;

namespace Uintra20.Core.Updater
{
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
}