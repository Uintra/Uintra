using System;
using System.Web;
using uIntra.Core.Extentions;

namespace uIntra.Core
{
    public class CookieProvider : ICookieProvider
    {
        public HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public void Save(HttpCookie cookie)
        {
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public bool Exists(string name)
        {
            var cookie = Get(name);
            return cookie != null && cookie.Value.IsNotNullOrEmpty();
        }

        public void Delete(string name)
        {
            var cookie = Get(name);
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}
