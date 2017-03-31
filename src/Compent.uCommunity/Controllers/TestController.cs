using System;
using System.Diagnostics;
using System.Linq;
using uCommunity.Core.User;
using Umbraco.Web.WebApi;

namespace Compent.uCommunity.Controllers
{
    public class TestController:UmbracoApiController
    {
        private readonly IIntranetUserService _intranetUserService;

        public TestController(IIntranetUserService intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public string Get()
        {
            var sw = Stopwatch.StartNew();
            //var a = _intranetUserService.GetByRole(IntranetRolesEnum.WebMaster).ToList();
            //var b = _intranetUserService.GetByRole(IntranetRolesEnum.UiUser).ToList();
            //var c = _intranetUserService.GetByRole(IntranetRolesEnum.UiPublisher).ToList();
            sw.Stop();

            //return sw.Elapsed.TotalSeconds.ToString();

            return DateTimeOffset.UtcNow.AddSeconds(15).ToString();
        }
    }
}