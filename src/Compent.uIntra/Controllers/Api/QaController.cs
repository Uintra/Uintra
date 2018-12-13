using System;
using System.Web.Http;
using Uintra.Core.ApplicationSettings;
using Uintra.Notification;
using Umbraco.Web.WebApi;

namespace Compent.Uintra.Controllers.Api
{
    public class QaController : UmbracoApiController
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IMonthlyEmailService _monthlyEmailService;
        private readonly IReminderJob _reminderJob;

        public QaController(IApplicationSettings applicationSettings, IMonthlyEmailService monthlyEmailService,IReminderJob reminderJob)
        {
            _applicationSettings = applicationSettings;
            _monthlyEmailService = monthlyEmailService;
            _reminderJob = reminderJob;
        }

        [HttpGet]
        public void SendMonthlyEmail(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _monthlyEmailService.CreateAndSendMail();
            }
        }

        [HttpGet]
        public void RunRemainder(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _reminderJob.Run();
            }
        }
    }
}