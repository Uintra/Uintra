using System;
using System.Collections.Generic;
using System.Linq;
using BCLExtensions.Trees;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps;
using static BCLExtensions.Trees.TreeExtensions;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("0.0.0.1");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return Resolve<CoreInstallationStep>();
                yield return Resolve<UsersInstallationStep>();
                yield return Resolve<NotificationInstallationStep>();
                yield return Resolve<BulletinsInstallationStep>();
                yield return Resolve<NewsInstallationStep>();
                yield return Resolve<EventsInstallationStep>();
                yield return Resolve<GroupsInstallationStep>();
                yield return Resolve<SearchInstallationStep>();
                yield return Resolve<PanelsInstallationStep>();
                yield return Resolve<NavigationInstallationStep>();
                yield return Resolve<HeadingInstallationStep>();
                yield return Resolve<MediaSearchInstallationStep>();
                yield return Resolve<AggregateStep>();
            }
        }
    }
}