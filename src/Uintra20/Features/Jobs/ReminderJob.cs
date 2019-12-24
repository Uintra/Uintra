using Uintra20.Core.Jobs.Models;
using Uintra20.Features.Reminder;

namespace Uintra20.Features.Jobs
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
