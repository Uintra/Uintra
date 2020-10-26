using Compent.Extensions;
using System;
using TimeZoneConverter;

namespace Uintra20.Infrastructure.Providers
{
    public class ClientTimezoneProvider : IClientTimezoneProvider
    {
        private readonly ICookieProvider _cookieProvider;
        private const string ClientTimezoneCookieAlias = "clientTimezone";


        public ClientTimezoneProvider(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public virtual void SetClientTimezone(string ianaTimezoneId)
        {
            var windowsTimezoneId = TZConvert.IanaToWindows(ianaTimezoneId);
            _cookieProvider.Save(ClientTimezoneCookieAlias, windowsTimezoneId, DateTime.UtcNow.AddMonths(1));
        }

        public virtual TimeZoneInfo ClientTimezone
        {
            get
            {
                var cookieValue = _cookieProvider.Get(ClientTimezoneCookieAlias)?.Value;

                return !cookieValue.HasValue() 
                    ? TimeZoneInfo.Utc 
                    : TimeZoneInfo.FindSystemTimeZoneById(cookieValue);
            }
        }
    }
}