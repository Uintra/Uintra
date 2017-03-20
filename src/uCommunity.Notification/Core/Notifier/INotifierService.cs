namespace uCommunity.Notification.Notifier
{
    public interface INotifierService
    {
        NotifierTypeEnum Type { get;}
        void Notify(NotifierData data);
    }
}
