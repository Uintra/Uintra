using System;
using Uintra20.Core.Activity.Helpers;

namespace Uintra20.Core.Activity.Factories
{
    public interface IActivityPageHelperFactory
    {
        IActivityPageHelper GetHelper(Enum type);
    }
}
