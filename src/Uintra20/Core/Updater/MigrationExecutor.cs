using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compent.Extensions;
using Google;
using LightInject;
using UBaseline.Core.Extensions;
using Uintra20.Core.Updater.Sql;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web.Composing;

namespace Uintra20.Core.Updater
{
    public class MigrationExecutor
    {
        private readonly ILogger _logger;
        private readonly IMigrationHistoryService _migrationHistoryService;
        public static readonly Version LastLegacyMigrationVersion = new Version("2.0.0.0");

        public MigrationExecutor()
        {
            _logger = Current.Factory.EnsureScope(f => f.GetInstance<ILogger>());
            _migrationHistoryService = Current.Factory.EnsureScope(f => f.GetInstance<IMigrationHistoryService>());
        }

        public void Run()
        {
            var allHistory = _migrationHistoryService.GetAll();
            var allSteps = GetAllMigrations().Pipe(GetAllSteps);
            var missingSteps = GetSteps(allSteps, allHistory, LastLegacyMigrationVersion);

            var (executionHistory, executionResult) = TryExecuteSteps(missingSteps);

            if (executionResult.Type is ExecutionResultType.Success)
            {
                executionHistory
                    .Pipe(ToMigrationHistory)
                    .Do(_migrationHistoryService.Create);
            }
            else
            {
                _logger.Error<MigrationExecutor>(executionResult.Exception);

                var (undoHistory, undoResult) = TryUndoSteps(executionHistory);
                if (undoResult.Type is ExecutionResultType.Failure)
                {
                    undoHistory
                        .Pipe(ToMigrationHistory)
                        .Do(_migrationHistoryService.Create);
                    _logger.Error<MigrationExecutor>(undoResult.Exception);
                }
            }
        }

        private IOrderedEnumerable<IMigration> GetAllMigrations() =>
            Current.Factory.EnsureScope(f => f.GetAllInstances<IMigration>())
                .OrderBy(m => m.Version);

        private static IEnumerable<(string name, Version version)> ToMigrationHistory(Stack<MigrationItem> items) =>
            items
                .Reverse()
                .Select(step => (name: StepIdentity(step.Step), version: step.Version));

        private static IEnumerable<MigrationItem> GetAllSteps(IOrderedEnumerable<IMigration> migrations) =>
            migrations.SelectMany(migration =>
                migration.Steps.Select(step => new MigrationItem(migration.Version, step)));

        private static IEnumerable<MigrationItem> GetSteps(
            IEnumerable<MigrationItem> allSteps,
            List<MigrationHistory> allHistory,
            Version lastLegacyMigrationVersion)
        {
            var lastHistoryVersion = allHistory
                .Select(h => Version.Parse(h.Version))
                .OrderByDescending(h => h)
                .FirstOrDefault();

            switch (lastHistoryVersion)
            {
                case Version version when version <= lastLegacyMigrationVersion:
                    return allSteps.Where(s => s.Version > lastHistoryVersion);
                case Version _:
                    return GetMissingSteps(allSteps.Where(s => s.Version > lastLegacyMigrationVersion), allHistory);
                case null:
                    return allSteps;
                default:
                    return allSteps;
            }
        }

        private static IEnumerable<MigrationItem> GetMissingSteps(
            IEnumerable<MigrationItem> allSteps,
            IEnumerable<MigrationHistory> allHistory) => allSteps
            .Where(migrationItem =>
                !allHistory.Any(historyItem => IsMigrationItemEqualsToHistory(migrationItem, historyItem)));


        private static bool IsMigrationItemEqualsToHistory(MigrationItem item, MigrationHistory history) =>
            history.Name == StepIdentity(item.Step) && new Version(history.Version) == item.Version;

        private static (Stack<MigrationItem> executionHistory, ExecutionResult result) TryExecuteSteps(
            IEnumerable<MigrationItem> steps)
        {
            var executionHistory = new Stack<MigrationItem>();
            foreach (var step in steps)
            {
                executionHistory.Push(step);
                var stepActionResult = TryExecuteStep(step.Step);
                if (stepActionResult.Type is ExecutionResultType.Failure)
                {
                    return (executionHistory, stepActionResult);
                }
            }

            return (executionHistory, ExecutionResult.Success);
        }


        private static ExecutionResult TryExecuteStep(IMigrationStep migrationStep)
        {
            try
            {
                return migrationStep.Execute();
            }
            catch (Exception e)
            {
                return ExecutionResult.Failure(e);
            }
        }

        private static (Stack<MigrationItem> undoHistory, ExecutionResult result) TryUndoSteps(
            Stack<MigrationItem> executionHistory)
        {
            var undoHistory = new Stack<MigrationItem>();
            while (executionHistory.Any())
            {
                var step = executionHistory.Pop();
                undoHistory.Push(step);
                var stepActionResult = TryUndoStep(step.Step);
                if (stepActionResult.Type is ExecutionResultType.Failure)
                {
                    return (executionHistory, stepActionResult);
                }
            }

            return (undoHistory, ExecutionResult.Success);
        }

        private static ExecutionResult TryUndoStep(IMigrationStep migrationStep)
        {
            try
            {
                migrationStep.Undo();
            }
            catch (Exception e)
            {
                return ExecutionResult.Failure(e);
            }

            return ExecutionResult.Success;
        }

        private static string StepIdentity(IMigrationStep step) => step.GetType().Name;
    }
}