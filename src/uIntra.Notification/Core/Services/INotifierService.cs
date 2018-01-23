using System;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public interface INotifierService
    {
        Enum Type { get;}
        void Notify(NotifierData data);
    }
}
