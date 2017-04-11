using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Entities;

namespace uCommunity.Notification.Core.Services
{
    public interface INotifierService
    {
        NotifierTypeEnum Type { get;}
        void Notify(NotifierData data);
    }
}
