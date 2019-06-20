
using System;

namespace Uintra.Core
{
    public interface IClientTimezoneProvider
    {
        void SetClientTimezone(string ianaTimezoneId);
        TimeZoneInfo ClientTimezone { get; }
    }
}
