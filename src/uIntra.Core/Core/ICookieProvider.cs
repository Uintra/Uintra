using System.Web;

namespace uIntra.Core
{
    public interface ICookieProvider
    {
        HttpCookie Get(string name);

        void Save(HttpCookie cookie);

        bool Exists(string name);

        void Delete(string name);
    }
}
