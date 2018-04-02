using System;
using System.Collections.Generic;
using Compent.uIntra.Core.Updater.Migrations._0._2._32._0.Steps;
using Compent.Uintra.Core.Updater;

namespace Compent.uIntra.Core.Updater.Migrations._0._2._32._0
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("0.2.32.0");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return Resolve<NotificationSettingsMigrationStep>();        
            }
        }
    }
}