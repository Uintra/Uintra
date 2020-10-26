using System;

namespace Uintra.Infrastructure.Providers
{
    public interface IClientTimezoneProvider
    {
        void SetClientTimezone(string ianaTimezoneId);
        TimeZoneInfo ClientTimezone { get; }
    }
}
