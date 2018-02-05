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

            var missingSteps = GetMissingSteps(allSteps, allHistory).ToList();

            var (stepHistory, executionResult) = IterateSteps(missingSteps, TryExecute);

            var reversedExecutionHistory = stepHistory.Reverse();

            if (executionResult.Type is ExecutionResultType.Failure)
            {
                var (_, undoExecutionResult) = IterateSteps(reversedExecutionHistory, TryUndo);
                if (undoExecutionResult.Type is ExecutionResultType.Failure)
                {
                    _exceptionLogger.Log(undoExecutionResult.Exception);
                }
            }
            else
            {
                SaveMigrationsHistory(reversedExecutionHistory);
            }
        }

        private IEnumerable<IMigration> GetAllMigrations() =>
            _dependencyResolver
                .GetServices(typeof(IMigration))
                .Cast<IMigration>();

        private void SaveMigrationsHistory(IEnumerable<(Version migrationVersion, IMigrationStep step)> steps)
        {
            var history = steps.Select(step => (
                name: StepIdentity(step.step),
                version: step.migrationVersion));

            _migrationHistoryService.Create(history);
        }

        private static (Stack<(Version migrationVersion, IMigrationStep step)>, ExecutionResult stepActionResult) IterateSteps(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> steps,
            Func<IMigrationStep, ExecutionResult> stepAction)
        {
            var history = new Stack<(Version migrationVersion, IMigrationStep step)>();
            var stepActionResult = Success;
            foreach (var step in steps)
            {
                history.Push(step);
                stepActionResult = stepAction(step.step);
                if (stepActionResult.Type is ExecutionResultType.Failure) break;
            }
            return (history, stepActionResult);
        }

        private static IEnumerable<(Version migrationVersion, IMigrationStep step)> GetAllSteps(IOrderedEnumerable<IMigration> migrations) =>
            migrations.SelectMany(migration => migration.Steps.Select(step => (migration.Version, step)));

        private static IEnumerable<(Version migrationVersion, IMigrationStep step)> GetMissingSteps(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> allSteps,
            IOrderedEnumerable<MigrationHistory> allHistory)
        {
            var allHistoryList = allHistory.AsList();

            switch (allHistoryList.FirstOrDefault())
            {
                case MigrationHistory history when new Version(history.Version) <= LastLegacyMigrationVersion:
                    return allSteps.SkipWhile(s => s.migrationVersion <= LastLegacyMigrationVersion);
                case MigrationHistory _:
                    return GetMissingStepsd(allSteps, allHistoryList);
                case null:
                    return allSteps;
            }
        }

        private static IEnumerable<(Version migrationVersion, IMigrationStep step)> GetMissingStepsd(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> allSteps,
            IEnumerable<MigrationHistory> allHistory)
        {
            var installedSteps = new HashSet<string>(allHistory.Select(h => h.Name));
            return allSteps.Where(s => !installedSteps.Contains(StepIdentity(s.step)));
        }

        private static ExecutionResult TryUndo(IMigrationStep migrationStep)
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

        private static ExecutionResult TryExecute(IMigrationStep migrationStep)
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

        private static string StepIdentity(IMigrationStep step) => step.GetType().Name;
    }
}