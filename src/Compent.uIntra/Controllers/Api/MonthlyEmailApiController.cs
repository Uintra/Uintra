using System.Web.Http;
using Uintra.Notification;
using Uintra.Notification.Jobs;
using Umbraco.Web.WebApi;

namespace Compent.Uintra.Controllers.Api
{
    public class MonthlyEmailApiController : UmbracoApiController
    {
        private readonly IEmailBroadcastService<MonthlyMailBroadcast> _monthlyBroadcastService;
        private readonly IEmailBroadcastService<WeeklyMailBroadcast> _weeklyBroadcastService;
        public MonthlyEmailApiController(
            IEmailBroadcastService<MonthlyMailBroadcast> monthlyBroadcastService, 
            IEmailBroadcastService<WeeklyMailBroadcast> weeklyBroadcastService)
        {
            _monthlyBroadcastService = monthlyBroadcastService;
            _weeklyBroadcastService = weeklyBroadcastService;
        }

        [HttpGet]
        public void SendMonthlyEmail()
        {
            _monthlyBroadcastService.IsBroadcastable();
        }

        [HttpGet]
        public void SendWeeklyEmail()
        {
            _weeklyBroadcastService.IsBroadcastable();
        }
    }
}