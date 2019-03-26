using System;

namespace Uintra.Core
{
    public class TimezoneOffsetProvider : ITimezoneOffsetProvider
    {
        private readonly ICookieProvider _cookieProvider;
        private const string TimezoneOffsetCookieAlias = "timezoneOffset";


        public TimezoneOffsetProvider(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public void SetTimezoneOffset(int offsetInMinutes)
        {
            var offset = (-offsetInMinutes).ToString();
            _cookieProvider.Save(TimezoneOffsetCookieAlias, offset, DateTime.UtcNow.AddMonths(1));
        }

        public TimeSpan GetTimezoneOffset()
        {
            var cookie = _cookieProvider.Get(TimezoneOffsetCookieAlias);
            var offset = new TimeSpan(int.Parse(cookie.Value));

            return offset;
        }
    }
}
