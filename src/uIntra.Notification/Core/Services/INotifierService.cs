using System;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public interface INotifierService
    {
        Enum Type { get;}
        void Notify(NotifierData data);
    }
}
