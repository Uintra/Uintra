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

        public MigrationHandler()
        {
            _dependencyResolver = DependencyResolver.Current;
            _migrationHistoryService = _dependencyResolver.GetService<IMigrationHistoryService>();
            _exceptionLogger = _dependencyResolver.GetService<IExceptionLogger>();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var allMigrations = GetAllMigrations();
            var migrationsSequence = GetMissingMigrations(allMigrations).ToList();
            var migrationStepsSequence = migrationsSequence.SelectMany(m => m.Steps);

            var (executionResult, stepHistory) = TryExecuteSteps(migrationStepsSequence);

            if (executionResult.Type is ExecutionResultType.Failure)
            {
                var undoExecutionResult = UndoSteps(stepHistory);
                if (undoExecutionResult.Type is ExecutionResultType.Failure)
                {
                    _exceptionLogger.Log(undoExecutionResult.Exception);
                }
            }
            else
            {
                SaveMigrationsHistory(migrationsSequence.Select(m => m.Version));
            }
        }

        private IEnumerable<IMigration> GetAllMigrations() =>
            _dependencyResolver
                .GetServices(typeof(IMigration))
                .Cast<IMigration>();

        private IEnumerable<IMigration> GetMissingMigrations(IEnumerable<IMigration> migrations)
        {
            var orderedMigrations = migrations.OrderBy(m => m.Version);

            switch (_migrationHistoryService.GetLast())
            {
                case MigrationHistory version:
                    var lastVersion = new Version(version.Version);
                    return orderedMigrations.SkipWhile(m => m.Version <= lastVersion);
                case null:
                    return orderedMigrations;
            }
        }

        private (ExecutionResult excutionResult, Stack<IMigrationStep> stepHistory) TryExecuteSteps(IEnumerable<IMigrationStep> migrationSteps)
        {
            var stepHistory = new Stack<IMigrationStep>();

            foreach (var migrationStep in migrationSteps)
            {
                var stepExecutionResult = migrationStep.Execute();
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

        private void SaveMigrationsHistory(IEnumerable<Version> versions)
        {
            versions
                .Select(ver => ver.ToString())
                .ToList()
                .ForEach(_migrationHistoryService.Create);
        }
    }
}