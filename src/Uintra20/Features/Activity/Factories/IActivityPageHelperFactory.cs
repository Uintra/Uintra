using System;
using Uintra20.Features.Activity.Helpers;

namespace Uintra20.Features.Activity.Factories
{
    public interface IActivityPageHelperFactory
    {
        IActivityPageHelper GetHelper(Enum type);
    }
}
