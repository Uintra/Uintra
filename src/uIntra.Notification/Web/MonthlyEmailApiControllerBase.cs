using System;
using uIntra.Core.ApplicationSettings;
using Umbraco.Web.WebApi;

namespace uIntra.Notification
{
    public class MonthlyEmailApiControllerBase: UmbracoApiController
    {
        private readonly IMonthlyEmailService _monthlyEmailService;

        private readonly IApplicationSettings _applicationSettings;

        public MonthlyEmailApiControllerBase(IMonthlyEmailService monthlyEmailService, 
            IApplicationSettings applicationSettings)
        {
            _monthlyEmailService = monthlyEmailService;
            _applicationSettings = applicationSettings;
        }

        [System.Web.Http.HttpGet]
        public void SendMonthlyEmail()
        {
            if (DateTime.Now.Day == _applicationSettings.MonthlyEmailJobDay)
            {
                _monthlyEmailService.SendEmail();
            }

        }
    }
}
