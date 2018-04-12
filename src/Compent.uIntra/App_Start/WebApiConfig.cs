using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;

namespace Compent.Uintra
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable elmah
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
        }
    }
}