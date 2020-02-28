﻿using Uintra20.Core.Jobs.Models;
using Uintra20.Features.Reminder.Services;

namespace Uintra20.Features.Jobs
{
    public class ReminderJob : Uintra20BaseIntranetJob
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
