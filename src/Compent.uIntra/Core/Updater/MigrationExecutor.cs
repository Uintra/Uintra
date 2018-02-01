using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        private readonly Version _lastLegacyMigrationVersion = new Version("0.2.30.0");

        public MigrationHandler()
        {
            _dependencyResolver = DependencyResolver.Current;
            _migrationHistoryService = _dependencyResolver.GetService<IMigrationHistoryService>();
            _exceptionLogger = _dependencyResolver.GetService<IExceptionLogger>();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var allMigrations = GetAllMigrations().OrderBy(m => m.Version);
            var getLastMigration = _migrationHistoryService.GetLast();
            var allSteps = GetAllSteps(allMigrations);
            var missingSteps = GetMissingSteps(allSteps, getLastMigration).ToList();

            var (executionResult, stepHistory) = TryExecuteSteps(missingSteps);

            if (executionResult.Type is ExecutionResultType.Failure)
            {
                var undoExecutionResult = UndoSteps(stepHistory.Select(s => s.step));
                if (undoExecutionResult.Type is ExecutionResultType.Failure)
                {
                    _exceptionLogger.Log(undoExecutionResult.Exception);
                }
            }
            else
            {
                SaveMigrationsHistory(stepHistory);
            }
        }

        private IEnumerable<IMigration> GetAllMigrations() =>
            _dependencyResolver
                .GetServices(typeof(IMigration))
                .Cast<IMigration>();

        private IEnumerable<(Version migrationVersion, IMigrationStep step)> GetAllSteps(IOrderedEnumerable<IMigration> migrations)=>
            migrations.SelectMany(migration => migration.Steps.Select(step => (migration.Version, step)));

        private IEnumerable<(Version migrationVersion, IMigrationStep step)> GetMissingSteps(
            IEnumerable<(Version migrationVersion, IMigrationStep step)> steps,
            MigrationHistory lastMigration)
        {

            switch (lastMigration)
            {
                case MigrationHistory history when new Version(history.Version) <= _lastLegacyMigrationVersion:
                    return steps
                        .SkipWhile(s => s.migrationVersion <= new Version(history.Version));
                case MigrationHistory history:
                    return steps
                        .SkipWhile(s => s.step.GetType().ToString() != history.Name)
                        .Skip(1);
                case null:
                    return steps;
            }
        }

        private static (ExecutionResult excutionResult, Stack<(Version migrationVersion, IMigrationStep step)> stepHistory) TryExecuteSteps(
             IEnumerable<(Version migrationVersion, IMigrationStep step)> migrationSteps)
        {
            var stepHistory = new Stack<(Version migrationVersion, IMigrationStep step)>();

            foreach (var migrationStep in migrationSteps)
            {
                var stepExecutionResult = migrationStep.step.Execute();
                switch (stepExecutionResult.Type)
                {
                    case ExecutionResultType.Success:
                        stepHistory.Push(migrationStep);
                        break;
                    case ExecutionResultType.Skipped:
                        break;
                    case ExecutionResultType.Failure:
                        stepHistory.Push(migrationStep);
                        return (stepExecutionResult, stepHistory);
                }
            }
            return (Success, stepHistory);
        }

        private ExecutionResult UndoSteps(IEnumerable<IMigrationStep> migrationSteps)
        {
            try
            {
                foreach (var migrationStep in migrationSteps)
                {
                    migrationStep.Undo();
                }
                return Success;
            }
            catch (Exception e)
            {
                return Failure(e);
            }
        }

        private void SaveMigrationsHistory(IEnumerable<(Version migrationVersion, IMigrationStep step)> versions)
        {
            versions
                .ToList()
                .ForEach( s=> _migrationHistoryService.Create(s.step.GetType().Name, s.migrationVersion));
        }
    }
}