using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core;

namespace uIntra.Notification.DefaultImplementation
{
    public class NotifierTypeProvider : IntranetTypeProviderBase<NotifierTypeEnum>, INotifierTypeProvider
    {
    }
}
