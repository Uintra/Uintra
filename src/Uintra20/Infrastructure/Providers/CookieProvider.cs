using System;
using System.Web;
using Compent.Extensions;
using Uintra20.Infrastructure.ApplicationSettings;

namespace Uintra20.Infrastructure.Providers
{
    public class CookieProvider : ICookieProvider
    {
	    private readonly IApplicationSettings _applicationSettings;

        public CookieProvider(IApplicationSettings applicationSettings)
        {
	        _applicationSettings = applicationSettings;
        }

        public virtual HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        public virtual void Save(HttpCookie cookie)
        {
            cookie.Domain = GetDomain();
            cookie.Secure = _applicationSettings.UmbracoUseSSL;
            cookie.HttpOnly = true;
            HttpContext.Current.Request.Cookies.Add(cookie);
        }

        public virtual void Save(string name, string value, DateTime expireDate)
        {
            var cookie = new HttpCookie(name)
            {
                Name = name,
                Value = value,
                Expires = expireDate,
                HttpOnly = true
            };

            Save(cookie);
        }

        public virtual bool Exists(string name)
        {
            var cookie = Get(name);
            return cookie != null && cookie.Value.HasValue();
        }

        public virtual void Delete(string name)
        {
            var cookie = Get(name);
            if (cookie != null)
            {
                cookie.Expires = DateTime.UtcNow.AddDays(-1);
            }
        }

        protected virtual string GetDomain()
        {
            return HttpContext.Current.Request.Url.Host;
        }
    }
}