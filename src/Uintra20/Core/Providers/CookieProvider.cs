﻿using System;
using System.Web;
using Compent.Extensions;
using Uintra20.Core.ApplicationSettings;

namespace Uintra20.Core
{
    public class CookieProvider : ICookieProvider
    {
        private readonly HttpContext _httpContext;
        private readonly IApplicationSettings _applicationSettings;

        public CookieProvider(HttpContext httpContext, IApplicationSettings applicationSettings)
        {
            _httpContext = httpContext;
            _applicationSettings = applicationSettings;
        }

        public virtual HttpCookie Get(string name)
        {
            return _httpContext.Request.Cookies[name];
        }

        public virtual void Save(HttpCookie cookie)
        {
            cookie.Domain = GetDomain();
            cookie.Secure = _applicationSettings.UmbracoUseSSL;
            cookie.HttpOnly = true;
            _httpContext.Response.Cookies.Add(cookie);
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
            return _httpContext.Request.Url.Host;
        }
    }
}