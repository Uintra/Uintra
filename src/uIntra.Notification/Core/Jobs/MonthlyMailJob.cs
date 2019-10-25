using Uintra.Core.Jobs.Models;

namespace Uintra.Notification.Jobs
{
    public class MonthlyMailJob : BaseIntranetJob
    {
        private readonly IEmailBroadcastService<MonthlyMailBroadcast> broadcastService;

        public MonthlyMailJob(IEmailBroadcastService<MonthlyMailBroadcast> broadcastService) =>
            this.broadcastService = broadcastService;

        public override void Action() =>
            broadcastService.IsBroadcastable();
    }
}
