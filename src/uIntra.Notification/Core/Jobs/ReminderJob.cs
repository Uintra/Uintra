using uIntra.Core.Jobs.Models;

namespace uIntra.Notification.Jobs
{
    public class ReminderJob : BaseIntranetJob
    {
        private readonly IReminderJob _reminderJob;

        public ReminderJob(IReminderJob reminderJob)
        {
            _reminderJob = reminderJob;
        }
        public override void Action()
        {
            _reminderJob.Run();
        }
    }
}
