using System;
using System.Web;

namespace uCommunity.Core
{
    public class TimezoneOffsetProvider : ITimezoneOffsetProvider
    {
        private readonly HttpContext _httpContext;
        private string timezoneOffsetCookieAlias = "timezoneOffset";

        public TimezoneOffsetProvider(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetTimezoneOffset(int offsetInMinutes)
        {
            HttpCookie cookie = new HttpCookie(timezoneOffsetCookieAlias);
            cookie.Value = (-offsetInMinutes).ToString();
            _httpContext.Response.Cookies.Add(cookie);
        }

        public int GetTimezoneOffset()
        {
            var cookie = _httpContext.Request.Cookies.Get(timezoneOffsetCookieAlias);
            var offset = Convert.ToInt32(cookie?.Value);

            return offset;
        }
    }
}
