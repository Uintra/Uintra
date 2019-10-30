using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Notification
{
    public class NotifierTypeProvider : EnumTypeProviderBase, INotifierTypeProvider
    {
        public NotifierTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
