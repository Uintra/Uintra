using System;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Notification
{
    public class NotifierTypeProvider : EnumTypeProviderBase, INotifierTypeProvider
    {
        public NotifierTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}