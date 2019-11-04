using System;
using Uintra20.Core.TypeProviders;

namespace Uintra20.Core.Notification
{
    public class NotifierTypeProvider : EnumTypeProviderBase, INotifierTypeProvider
    {
        public NotifierTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}