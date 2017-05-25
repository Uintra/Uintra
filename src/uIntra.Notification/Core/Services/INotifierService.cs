using uIntra.Notification.Core.Configuration;
using uIntra.Notification.Core.Entities.Base;

namespace uIntra.Notification.Core.Services
{
    public interface INotifierService
    {
        NotifierTypeEnum Type { get;}
        void Notify(NotifierData data);
    }
}
