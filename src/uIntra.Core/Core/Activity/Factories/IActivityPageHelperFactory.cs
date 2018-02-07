using System;

namespace uIntra.Core.Activity
{
    public interface IActivityPageHelperFactory
    {
        IActivityPageHelper GetHelper(Enum type);
    }
}