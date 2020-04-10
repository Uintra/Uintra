using System.Web.Mvc;
using FluentScheduler;
using Uintra20.Core.Jobs;
using Uintra20.Core.Updater;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Uintra20
{
	[RuntimeLevel(MinLevel = RuntimeLevel.Run)]
	public class UintraApplicationComponent : IComponent
	{
		public void Initialize()
		{
			var migrator=new MigrationExecutor();
			migrator.Run();
			
			JobManager.JobFactory = DependencyResolver.Current.GetService<IJobFactory>();
			JobManager.Initialize(new JobsRegistry());
        }
		public void Terminate()
		{

		}
	}
}