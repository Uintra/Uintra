using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Updater;
using FsCheck;
using FsCheck.Xunit;
using Uintra.Core.MigrationHistories.Sql;
using Xunit;

namespace Uintra.UnitTests.Migrations
{
    public class MigrationsTests
    {
        [Fact]
        public void IsMigrationItemEqualsToHistory()
        {
            //arrange
            var step = new TestStep(ExecutionResult.Success);
            var positiveCaseStepName = MigrationHandler.StepIdentity(step);
            var positiveCaseVersion = new Version("1.2.3.4");
            var negativeCaseVersion = new Version("4.3.2.1");
            var migrationItem = new MigrationItem(positiveCaseVersion, new TestStep(ExecutionResult.Success));
            var migrationHistory = new MigrationHistory
            {
                Name = positiveCaseStepName,
                Version = positiveCaseVersion.ToString()
            };

            //act
            var positiveCase = MigrationHandler.IsMigrationItemEqualsToHistory(migrationItem, migrationHistory);

            migrationHistory.Version = negativeCaseVersion.ToString();
            var negativeCase1 = MigrationHandler.IsMigrationItemEqualsToHistory(migrationItem, migrationHistory);

            migrationHistory.Version = positiveCaseVersion.ToString();
            migrationHistory.Name = String.Empty;
            var negativeCase2 = MigrationHandler.IsMigrationItemEqualsToHistory(migrationItem, migrationHistory);

            //assert
            Assert.True(positiveCase && !negativeCase1 && !negativeCase2);
        }

        [Property]
        public Property GetMissingSteps(NonNegativeInt present, NonNegativeInt missing)
        {
            bool Predicate()
            {
                //arrange
                var (history, steps) = MigrationTestsContext.GenerateMigrationState(present, missing);

                //act
                var result = MigrationHandler.GetMissingSteps(steps, history, steps[present.Item - 1].Version);

                var originalVersions = steps.Skip(present.Item).Select(x => x.Version.ToString());
                var filteredVersions = result.Select(r => r.Version.ToString());

                //assert
                return originalVersions.SequenceEqual(filteredVersions);
            }

            return new Func<bool>(Predicate).When(present.Item > 0);
        }

        [Property]
        public bool GetSteps_HistoryIsEmpty(NonNegativeInt stepsCount)
        {
            //arrange
            var (_, steps) = MigrationTestsContext.GenerateMigrationState(stepsCount.Item, missingCount: 0);

            //act
            var result = MigrationHandler.GetSteps(steps, allHistory: new List<MigrationHistory>(), lastLegacyMigrationVersion: new Version("1.1.1.1"));

            var originalVersions = steps.Select(x => x.Version.ToString());
            var filteredVersions = result.Select(r => r.Version.ToString());

            //assert
            return originalVersions.SequenceEqual(filteredVersions);
        }

        [Property]
        public Property GetSteps_HistoryIsNotEmpty(NonNegativeInt present, NonNegativeInt missing, NonNegativeInt lastLegacyMigrationVersionNumber)
        {
            bool Predicate()
            {
                //arrange
                var (history, steps) = MigrationTestsContext.GenerateMigrationState(present, missing);
                var lastLegacyMigrationVersion = steps[lastLegacyMigrationVersionNumber.Item - 1];

                //act
                var result = MigrationHandler.GetSteps(steps, history, lastLegacyMigrationVersion.Version);

                var originalVersions = steps.Skip(present.Item).Select(x => x.Version.ToString());
                var filteredVersions = result.Select(r => r.Version.ToString());

                //assert
                return originalVersions.SequenceEqual(filteredVersions);
            }

            return new Func<bool>(Predicate).When(
                present.Item > 0 &&
                lastLegacyMigrationVersionNumber.Item > 0 &&
                lastLegacyMigrationVersionNumber.Item <= present.Item + missing.Item &&
                lastLegacyMigrationVersionNumber.Item >= present.Item);
        }

        [Property]
        public Property TryExecuteSteps_WithFailure(NonNegativeInt stepsCount, NonNegativeInt failurePosition)
        {
            bool Predicate()
            {
                //arrange
                var (_, steps) = MigrationTestsContext.GenerateMigrationState(0, stepsCount);
                steps[failurePosition.Item - 1] = MigrationTestsContext.MigrationItem(steps[failurePosition.Item - 1].Version, ExecutionResult.Failure(null));

                //act
                var (executionHistory, executionResult) = MigrationHandler.TryExecuteSteps(steps);

                var originalVersions = steps.Take(failurePosition.Item).Select(x => x.Version.ToString());
                var executedVersions = executionHistory.Reverse().Select(r => r.Version.ToString());

                //assert
                return executionResult.Type is ExecutionResultType.Failure && originalVersions.SequenceEqual(executedVersions);
            }

            return new Func<bool>(Predicate).When(stepsCount.Item >= failurePosition.Item && failurePosition.Item > 0);
        }

        [Property]
        public bool TryExecuteSteps_WithoutFailure(NonNegativeInt stepsCount)
        {
            //arrange
            var (_, steps) = MigrationTestsContext.GenerateMigrationState(0, stepsCount);

            //act
            var (executionHistory, executionResult) = MigrationHandler.TryExecuteSteps(steps);

            var originalVersions = steps.Select(x => x.Version.ToString());
            var executedVersions = executionHistory.Reverse().Select(r => r.Version.ToString());

            //assert
            return executionResult.Type is ExecutionResultType.Success && originalVersions.SequenceEqual(executedVersions);
        }

        [Property]
        public Property TryUndoSteps_WithFailure(NonNegativeInt stepsCount, NonNegativeInt failurePosition)
        {
            bool Predicate()
            {
                //arrange
                var (_, steps) = MigrationTestsContext.GenerateMigrationState(0, stepsCount);
                steps[failurePosition.Item - 1] =
                    MigrationTestsContext.MigrationItem(steps[failurePosition.Item - 1].Version, ExecutionResult.Failure(null), throwExceptionOnUndo: true);


                var historyStack = new Stack<MigrationItem>(steps);

                //act
                var (executionHistory, executionResult) = MigrationHandler.TryUndoSteps(historyStack);

                var originalVersions = steps.Take(failurePosition.Item - 1).Select(x => x.Version.ToString());
                var executedVersions = executionHistory.Reverse().Select(r => r.Version.ToString());

                //assert
                return executionResult.Type is ExecutionResultType.Failure && originalVersions.SequenceEqual(executedVersions);
            }

            return new Func<bool>(Predicate).When(stepsCount.Item >= failurePosition.Item && failurePosition.Item > 0);
        }

        [Property]
        public bool TryUndoSteps_WithoutFailure(NonNegativeInt stepsCount)
        {
            //arrange 
            var (_, steps) = MigrationTestsContext.GenerateMigrationState(0, stepsCount);


            var historyStack = new Stack<MigrationItem>(steps);

            //act
            var (executionHistory, executionResult) = MigrationHandler.TryUndoSteps(historyStack);

            var originalVersions = steps.Select(x => x.Version.ToString());
            var executedVersions = executionHistory.Select(r => r.Version.ToString());

            //assert
            return executionResult.Type is ExecutionResultType.Success && originalVersions.SequenceEqual(executedVersions);
        }
    }
}