using Uintra.Notification.Jobs;

namespace Uintra.Notification
{
    public interface IEmailBroadcastService<T> where T : IMailBroadcast
    {
        void IsBroadcastable();
        void Broadcast();
    }
}
