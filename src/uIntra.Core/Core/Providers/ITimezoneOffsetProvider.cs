
using System;

namespace Uintra.Core
{
    public interface ITimezoneOffsetProvider
    {
        void SetTimezoneOffset(int offsetInMinutes);
        TimeSpan GetTimezoneOffset();
    }
}
