using System.Linq;
using System.Web.Mvc;
using FluentScheduler;
using Uintra.Core.Jobs;
using Uintra.Core.Updater;
using Umbraco.Core;
using Umbraco.Core.Composing;
using uSync8.BackOffice;
using uSync8.BackOffice.Configuration;

namespace Uintra
{
	public class UintraApplicationComponent : IComponent
	{
		//private readonly uSyncService _uSyncService;
		//private uSyncSettings _settings;
		//public UintraApplicationComponent(uSyncService uSyncService)
		//{
		//	_uSyncService = uSyncService;
		//	_settings = Current.Configs.uSync();
		//}

		public void Initialize()
		{
			//_uSyncService.Import(_settings.RootFolder, false, 
			//	summary => 
			//	{
			//		var postImportHandler = summary.Handlers.First(i => i.Name.Equals("Post Import"));
			//		if (postImportHandler.Status == HandlerStatus.Complete)
			//		{
			//			var migrator = new MigrationExecutor();
			//			migrator.Run();
			//		}
			//	});

			var migrator = new MigrationExecutor();
			migrator.Run();

			JobManager.JobFactory = DependencyResolver.Current.GetService<IJobFactory>();
			JobManager.Initialize(new JobsRegistry());
        }
		public void Terminate()
		{

		}
	}
}