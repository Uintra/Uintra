using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Extensions;
using uIntra.Core.Exceptions;
using uIntra.Core.MigrationHistories;
using uIntra.Core.MigrationHistories.Sql;
using Umbraco.Core;
using static Compent.uIntra.Core.Updater.ExecutionResult;

namespace Compent.uIntra.Core.Updater
{
    public class MigrationHandler : ApplicationEventHandler
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IMigrationHistoryService _migrationHistoryService;
        private readonly IExceptionLogger _exceptionLogger;
        private static readonly Version LastLegacyMigrationVersion = new Version("0.2.30.0");

        public MigrationHandler()
        {
            _dependencyResolver = DependencyResolver.Current;
            _migrationHistoryService = _dependencyResolver.GetService<IMigrationHistoryService>();
            _exceptionLogger = _dependencyResolver.GetService<IExceptionLogger>();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var allMigrations = GetAllMigrations().OrderBy(m => m.Version);
            var allHistory = _migrationHistoryService.GetAll();
            var allSteps = GetAllSteps(allMigrations);

            var missingSteps = GetSteps(allSteps, allHistory).ToList();

            var history = Enumerable.Empty<(Version, IMigrationStep)>();

            var (executionHistory, executionResult) = TryExecuteSteps(missingSteps);

            if (executionResult.Type is ExecutionResultType.Success)
            {
                history = executionHistory;
            }
            else
            {
                var (undoHistory, undoResult) = TryUndoSteps(executionHistory);
                if (undoResult.Type is ExecutionResultType.Failure)
                {
                    history = undoHistory;
                    _exceptionLogger.Log(undoResult.Exception);
                }
            }

            var reversedHistory = history.Reverse();

            SaveMigrationsHistory(reversedHistory);
        }

        private IEnumerable<IMigration> GetAllMigrations() =>
            _dependencyResolver
                .GetServices(typeof(IMigration))
                .Cast<IMigration>();

        private void SaveMigrationsHistory(IEnumerable<(Version migrationVersion, IMigrationStep step)> steps)
        {
            var stepsList = steps.AsList();
            if (stepsList.Any())
            {
                var history = stepsList.Select(step => (
                    name: StepIdentity(step.step),
                    version: step.migrationVersion));

                _migrationHistoryService.Create(history);
            }
        }

        private static IEnumerable<(Version migrationVersion, IMigrationStep step)> GetAllSteps(IOrderedEnumerable<IMigration> migrations) =>
            migrations.SelectMany(migration => migration.Steps.Select(step => (migration.Version, step)));

        private static IEnumerable<(Version migrationVersion, IMigrationStep step)> GetSteps(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> allSteps,
            IOrderedEnumerable<MigrationHistory> allHistory)
        {
            var allHistoryList = allHistory.AsList();

            var lastHistory = allHistoryList.OrderBy(m => m.CreateDate).FirstOrDefault();

            switch (lastHistory)
            {
                case MigrationHistory history when new Version(history.Version) <= LastLegacyMigrationVersion:
                    return allSteps.SkipWhile(s => s.migrationVersion <= LastLegacyMigrationVersion);
                case MigrationHistory _:
                    var lastHistoryVersion = new Version(lastHistory.Version);
                    return GetMissingSteps(allSteps, allHistoryList, lastHistoryVersion);
                case null:
                    return allSteps;
            }
        }

        private static IEnumerable<(Version migrationVersion, IMigrationStep step)> GetMissingSteps(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> allSteps,
            IEnumerable<MigrationHistory> allHistory, Version lastHistoryVersion)
        {
            var installedSteps = new List<string>(allHistory.Select(h => h.Name));
            var result = allSteps.Where(s => s.migrationVersion >= lastHistoryVersion && !installedSteps.Contains(StepIdentity(s.step)));
            return result;
        }

        private static (Stack<(Version migrationVersion, IMigrationStep step)> executionHistory, ExecutionResult result) TryExecuteSteps(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> steps)
        {
            var executionHistory = new Stack<(Version migrationVersion, IMigrationStep step)>();
            foreach (var step in steps)
            {
                executionHistory.Push(step);
                var stepActionResult = TryExecuteStep(step.step);
                if (stepActionResult.Type is ExecutionResultType.Failure)
                {
                    return (executionHistory, stepActionResult);
                }
            }
            return (executionHistory, Success);
        }


        private static ExecutionResult TryExecuteStep(IMigrationStep migrationStep)
        {
            try
            {
                return migrationStep.Execute();
            }
            catch (Exception e)
            {
                return Failure(e);
            }
        }

        private static (Stack<(Version migrationVersion, IMigrationStep step)> undoHistory, ExecutionResult result) TryUndoSteps(
            Stack<(Version migrationVersion, IMigrationStep step)> executionHistory)
        {
            var undoHistory = new Stack<(Version migrationVersion, IMigrationStep step)>();
            while (executionHistory.Any())
            {
                var step = executionHistory.Pop();
                undoHistory.Push(step);
                var stepActionResult = TryUndoStep(step.step);
                if (stepActionResult.Type is ExecutionResultType.Failure)
                {
                    return (executionHistory, stepActionResult);
                }
            }
            return (undoHistory, Success);
        }

        private static ExecutionResult TryUndoStep(IMigrationStep migrationStep)
        {
            try
            {
                migrationStep.Undo();
            }
            catch (Exception e)
            {
                return Failure(e);
            }

            return Success;
        }

        private static string StepIdentity(IMigrationStep step) => step.GetType().Name;
    }
}