using System;
using Compent.Uintra.Core.Updater;

namespace Uintra.UnitTests.Migrations
{
    public class TestStep : IMigrationStep
    {
        private readonly ExecutionResult _executionResult;
        private readonly bool _throwExceptionOnUndo;

        public TestStep(ExecutionResult executionResult, bool throwExceptionOnUndo = false)
        {
            _executionResult = executionResult;
            _throwExceptionOnUndo = throwExceptionOnUndo;
        }

        public ExecutionResult Execute()
        {
            return _executionResult;
        }

        public void Undo()
        {
            if (_throwExceptionOnUndo) throw new Exception();
        }
    }
}
