using System;
using System.Web.Http;
using EmailWorker.Web.Helper.Gdpr;
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
        private readonly IEmailGdprService _emailGdprService;

        public QaController(IApplicationSettings applicationSettings, IMonthlyEmailService monthlyEmailService,IReminderJob reminderJob, IEmailGdprService emailGdprService)
        {
            _applicationSettings = applicationSettings;
            _monthlyEmailService = monthlyEmailService;
            _reminderJob = reminderJob;
            _emailGdprService = emailGdprService;
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

        [HttpGet]
        public void Gdpr(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _emailGdprService.SupportGdpr();
            }
        }
    }
}