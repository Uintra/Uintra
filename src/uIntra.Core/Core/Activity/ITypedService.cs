using System;

namespace uIntra.Core.Activity
{
    public interface ITypedService
    {
        // TODO: rename to [ Type ]
        Enum ActivityType { get; }
    }
}