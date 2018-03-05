using System;

namespace Uintra.Core.Activity
{
    public interface ITypedService
    {
        // TODO: rename to [ Type ]
        Enum ActivityType { get; }
    }
}