using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compent.Extensions;
using Examine;
using Uintra.Core.Exceptions;
using Uintra.Core.MigrationHistories;
using Uintra.Core.MigrationHistories.Sql;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using static Compent.Uintra.Core.Updater.ExecutionResult;

namespace Compent.Uintra.Core.Updater
{
    public class MigrationHandler : ApplicationEventHandler
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IMigrationHistoryService _migrationHistoryService;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IMediaService _mediaService;
        private readonly IContentTypeService _contentTypeService;

        public static readonly Version LastLegacyMigrationVersion = new Version("0.2.30.0");

        public MigrationHandler()
        {
            _dependencyResolver = DependencyResolver.Current;
            _migrationHistoryService = _dependencyResolver.GetService<IMigrationHistoryService>();
            _exceptionLogger = _dependencyResolver.GetService<IExceptionLogger>();
            _mediaService = _dependencyResolver.GetService<IMediaService>();
            _contentTypeService = _dependencyResolver.GetService<IContentTypeService>();
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
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

                if (executionHistory.Any()) RebuildCacheAndRecycleApp();
            }
            else
            {
                _exceptionLogger.Log(executionResult.Exception);
                var (undoHistory, undoResult) = TryUndoSteps(executionHistory);
                if (undoResult.Type is ExecutionResultType.Failure)
                {
                    undoHistory
                        .Pipe(ToMigrationHistory)
                        .Do(_migrationHistoryService.Create);
                    _exceptionLogger.Log(undoResult.Exception);

                    if (undoHistory.Any()) RebuildCacheAndRecycleApp();
                }
            }

            RebuildExamineIndex();
        }

        private IOrderedEnumerable<IMigration> GetAllMigrations() =>
            _dependencyResolver
                .GetServices(typeof(IMigration))
                .Cast<IMigration>()
                .OrderBy(m => m.Version);

        public static IEnumerable<(string name, Version version)> ToMigrationHistory(Stack<MigrationItem> items) =>
            items
                .Reverse()
                .Select(step => (name: StepIdentity(step.Step), version: step.Version));

        public static IEnumerable<MigrationItem> GetAllSteps(IOrderedEnumerable<IMigration> migrations) =>
            migrations.SelectMany(migration => migration.Steps.Select(step => new MigrationItem(migration.Version, step)));

        public static IEnumerable<MigrationItem> GetSteps(IEnumerable<MigrationItem> allSteps, List<MigrationHistory> allHistory, Version lastLegacyMigrationVersion)
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
                    return GetMissingSteps(allSteps.Where(s => s.Version > lastLegacyMigrationVersion), allHistory, lastHistoryVersion);
                case null:
                    return allSteps;
            }
        }

        public static IEnumerable<MigrationItem> GetMissingSteps(
            IEnumerable<MigrationItem> allSteps,
            IEnumerable<MigrationHistory> allHistory,
            Version lastHistoryVersion) => allSteps
            .Where(migrationItem =>
                !allHistory.Any(historyItem => IsMigrationItemEqualsToHistory(migrationItem, historyItem)));


        public static bool IsMigrationItemEqualsToHistory(MigrationItem item, MigrationHistory history) =>
            history.Name == StepIdentity(item.Step) && new Version(history.Version) == item.Version;

        public static (Stack<MigrationItem> executionHistory, ExecutionResult result) TryExecuteSteps(IEnumerable<MigrationItem> steps)
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

            return (executionHistory, Success);
        }


        public static ExecutionResult TryExecuteStep(IMigrationStep migrationStep)
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

        public static (Stack<MigrationItem> undoHistory, ExecutionResult result) TryUndoSteps(Stack<MigrationItem> executionHistory)
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

            return (undoHistory, Success);
        }

        public static ExecutionResult TryUndoStep(IMigrationStep migrationStep)
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

        public static string StepIdentity(IMigrationStep step) => step.GetType().Name;

        private static void RebuildCacheAndRecycleApp()
        {
            ApplicationContext.Current.Services.MediaService.RebuildXmlStructures();
            ApplicationContext.Current.Services.MemberService.RebuildXmlStructures();

            HttpRuntime.UnloadAppDomain();
        }

        private static void RebuildExamineIndex()
        {
            ExamineManager.Instance.IndexProviderCollection[Umbraco.Core.Constants.Examine.InternalIndexer].RebuildIndex();
        }
    }


    public struct MigrationItem
    {
        public Version Version { get; }
        public IMigrationStep Step { get; }

        public MigrationItem(Version version, IMigrationStep step)
        {
            Version = version;
            Step = step;
        }
    }
}