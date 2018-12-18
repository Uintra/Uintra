using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._1._0.Steps;
using Compent.Uintra.Core.Updater.Migrations._1._1.Steps;

namespace Compent.Uintra.Core.Updater.Migrations._1._1
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("1.1");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return SplitEventLabels.SplitEventLabelsTranslationsUpdateStep();
            }
        }
    }
}