using uIntra.Notification.Base;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface INotifierService
    {
        NotifierTypeEnum Type { get;}
        void Notify(NotifierData data);
    }
}
