using System;

namespace Uintra.Core.Activity
{
    public interface IActivityPageHelperFactory
    {
        IActivityPageHelper GetHelper(Enum type);
    }
}