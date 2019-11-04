using System;

namespace Uintra20.Core
{
    public interface IClientTimezoneProvider
    {
        void SetClientTimezone(string ianaTimezoneId);
        TimeZoneInfo ClientTimezone { get; }
    }
}
