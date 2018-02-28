using System.Web.Http;
using Uintra.Notification;
using Umbraco.Web.WebApi;

namespace Compent.Uintra.Controllers.Api
{
    public class MonthlyEmailApiController : UmbracoApiController
    {
        private readonly IMonthlyEmailService _monthlyEmailService;

        public MonthlyEmailApiController(IMonthlyEmailService monthlyEmailService)
        {
            _monthlyEmailService = monthlyEmailService;
        }

        [HttpGet]
        public void SendMonthlyEmail()
        {
            _monthlyEmailService.ProcessMonthlyEmail();
        }
    }
}