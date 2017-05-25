using System;

namespace uIntra.Notification.Exceptions
{
    public class ReminderIsAlreadyDeliveredException : ApplicationException
    {
        public ReminderIsAlreadyDeliveredException(Guid id)
            : base($"Can not set reminder {id} as delivered, because it is delivered already")
        {

        }
    }
}
