using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Updater;
using FsCheck;
using Uintra.Core.MigrationHistories.Sql;

namespace Uintra.UnitTests.Migrations
{
    public static class MigrationTestsContext
    {
        public static MigrationItem MigrationItem(Version ver, ExecutionResult result,  bool throwExceptionOnUndo = false) =>
            new MigrationItem(ver, new TestStep(result, throwExceptionOnUndo));

        public static MigrationHistory MigrationHistory(Version ver) =>
            new MigrationHistory
            {
                Name = MigrationHandler.StepIdentity(new TestStep(ExecutionResult.Success)),
                Version = ver.ToString()
            };

        public static (List<MigrationHistory> history, List<MigrationItem> allSteps) GenerateMigrationState(int presentCount, NonNegativeInt missingCount) =>
            GenerateMigrationState(presentCount, missingCount.Item);

        public static (List<MigrationHistory> history, List<MigrationItem> allSteps) GenerateMigrationState(NonNegativeInt presentCount, NonNegativeInt missingCount) =>
            GenerateMigrationState(presentCount.Item, missingCount.Item);

        public static (List<MigrationHistory> history, List<MigrationItem> allSteps) GenerateMigrationState(int presentCount, int missingCount)
        {
            var all = GenerateVersionSequence(new Version("0.0.0.0"), new Version("1.1.1.1"))
                .Take(presentCount + missingCount)
                .ToList();

            var steps = all.Select(x => MigrationItem(x, ExecutionResult.Success))
                .ToList();

            var history = all
                .Take(presentCount)
                .Select(MigrationHistory)
                .ToList();

            return (history, steps);
        }

        public static IEnumerable<Version> GenerateVersionSequence(Version inital, Version step)
        {
            yield return inital;

            var current = inital;
            while (true)
            {
                current = new Version(
                    current.Major + step.Major,
                    current.Minor + step.Minor,
                    current.Build + step.Build,
                    current.Revision + step.Revision);
                yield return current;
            }
        }
    }
}