using System;
using System.Web;

namespace Uintra.Core
{
    public interface ICookieProvider
    {
        HttpCookie Get(string name);

        void Save(HttpCookie cookie);

        void Save(string name, string value, DateTime expireDate);

        bool Exists(string name);

        void Delete(string name);
    }
}
