namespace Uintra.Notification
{
    public interface IEmailBroadcastService
    {
        void IsBroadcastable();
        void Broadcast();
    }
}
