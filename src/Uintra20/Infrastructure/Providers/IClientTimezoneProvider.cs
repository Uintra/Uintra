using System;

namespace Uintra20.Infrastructure.Providers
{
    public interface IClientTimezoneProvider
    {
        void SetClientTimezone(string ianaTimezoneId);
        TimeZoneInfo ClientTimezone { get; }
    }
}
