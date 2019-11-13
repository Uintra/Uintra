using System;

namespace Uintra20.Core.Activity
{
    public interface IActivityPageHelperFactory
    {
        IActivityPageHelper GetHelper(Enum type);
    }
}
