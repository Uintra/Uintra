using uIntra.Notification;
using Umbraco.Web.WebApi;
using System.Web.Http;

namespace Compent.uIntra.Controllers.Api
{
    public class MonthlyEmailApiController: UmbracoApiController
    {
        private readonly IMonthlyEmailService _monthlyEmailService;

        public MonthlyEmailApiController(IMonthlyEmailService monthlyEmailService)
        {
            _monthlyEmailService = monthlyEmailService;            
        }

        [HttpGet]
        public void SendMonthlyEmail()
        {
            _monthlyEmailService.SendEmail();
        }
    }
}