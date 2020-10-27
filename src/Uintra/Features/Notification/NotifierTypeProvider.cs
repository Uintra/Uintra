using System;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.Notification
{
    public class NotifierTypeProvider : EnumTypeProviderBase, INotifierTypeProvider
    {
        public NotifierTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}