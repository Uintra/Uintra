using Uintra.Core.Jobs.Models;
using Uintra.Features.Reminder.Services;

namespace Uintra.Features.Jobs
{
    public class ReminderJob : UintraBaseIntranetJob
    {
        private readonly IReminderRunner _reminderRunner;

        public ReminderJob(IReminderRunner reminderRunner)
        {
            _reminderRunner = reminderRunner;
        }
        public override void Action()
        {
            _reminderRunner.Run();
        }
    }
}
