using System;
using System.Collections.Generic;
using System.Linq;
using BCLExtensions.Trees;
using Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps;
using static BCLExtensions.Trees.TreeExtensions;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("0.0.0.1");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                var steps = new IMigrationStep[]
                {
                    _migrationStepsResolver.Resolve<CoreInstallationStep>(),
                    _migrationStepsResolver.Resolve<UsersInstallationStep>(),
                    _migrationStepsResolver.Resolve<NotificationInstallationStep>(),
                    _migrationStepsResolver.Resolve<BulletinsInstallationStep>(),
                    _migrationStepsResolver.Resolve<NewsInstallationStep>(),
                    _migrationStepsResolver.Resolve<EventsInstallationStep>(),
                    _migrationStepsResolver.Resolve<GroupsInstallationStep>(),
                    _migrationStepsResolver.Resolve<SearchInstallationStep>(),
                    _migrationStepsResolver.Resolve<PanelsInstallationStep>(),
                    _migrationStepsResolver.Resolve<NavigationInstallationStep>(),
                    _migrationStepsResolver.Resolve<HeadingInstallationStep>(),
                    _migrationStepsResolver.Resolve<MediaSearchInstallationStep>(),
                    _migrationStepsResolver.Resolve<AggregateStep>()
                };
                return steps.AsEnumerable();
            }
        }
    }
}