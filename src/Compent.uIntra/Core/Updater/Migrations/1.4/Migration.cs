using System;
using System.Collections.Generic;
using Compent.Uintra.Core.Updater.Migrations._1._4.Steps;

namespace Compent.Uintra.Core.Updater.Migrations._1._4
{
	public class Migration : IMigration
	{
		private readonly IMigrationStepsResolver _migrationStepsResolver;

		public Version Version => new Version("1.4");

		public Migration(IMigrationStepsResolver migrationStepsResolver)
		{
			_migrationStepsResolver = migrationStepsResolver;
		}


		private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

		public IEnumerable<IMigrationStep> Steps
		{
			get
			{
				yield return Resolve<SetGroupAdminsInSqlTableStep>();
				yield return Resolve<DisableCanPinPermissionForChanging>();
			}
		}
	}
}