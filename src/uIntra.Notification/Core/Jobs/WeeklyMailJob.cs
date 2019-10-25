using Uintra.Core.Jobs.Models;

namespace Uintra.Notification.Jobs
{
    public class WeeklyMailJob : BaseIntranetJob
    {
        private readonly IEmailBroadcastService<WeeklyMailBroadcast> broadcastService;

        public WeeklyMailJob(IEmailBroadcastService<WeeklyMailBroadcast> broadcastService) =>
            this.broadcastService = broadcastService;

        public override void Action() =>
            broadcastService.IsBroadcastable();
    }
}