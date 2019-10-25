using System.Web.Http;
using Uintra.Notification;
using Umbraco.Web.WebApi;

namespace Compent.Uintra.Controllers.Api
{
    public class MonthlyEmailApiController : UmbracoApiController
    {
        private readonly IEmailBroadcastService _emailBroadcastService;

        public MonthlyEmailApiController(IEmailBroadcastService emailBroadcastService)
        {
            _emailBroadcastService = emailBroadcastService;
        }

        [HttpGet]
        public void SendMonthlyEmail()
        {
            _emailBroadcastService.ProcessEmail();
        }
    }
}