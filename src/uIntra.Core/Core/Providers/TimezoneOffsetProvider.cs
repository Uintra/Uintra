using System;

namespace uIntra.Core
{
    public class TimezoneOffsetProvider : ITimezoneOffsetProvider
    {
        private readonly ICookieProvider _cookieProvider;
        private string timezoneOffsetCookieAlias = "timezoneOffset";


        public TimezoneOffsetProvider(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider;
        }

        public bool HasTimeZoneOffset()
        {
            var cookie = _cookieProvider.Get(timezoneOffsetCookieAlias);
            return cookie != null;
        }

        public void SetTimezoneOffset(int offsetInMinutes)
        {
            var offset = (-offsetInMinutes).ToString();
            _cookieProvider.Save(timezoneOffsetCookieAlias, offset, DateTime.UtcNow.AddMonths(1));
        }

        public int GetTimezoneOffset()
        {
            var cookie = _cookieProvider.Get(timezoneOffsetCookieAlias);
            var offset = Convert.ToInt32(cookie?.Value);

            return offset;
        }
    }
}
