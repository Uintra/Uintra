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

        public QaController(IApplicationSettings applicationSettings, IMonthlyEmailService monthlyEmailService)
        {
            _applicationSettings = applicationSettings;
            _monthlyEmailService = monthlyEmailService;
        }

        //http://uintra.local.compent.dk/umbraco/api/qa/SendMonthlyEmail?qakey=f39e4bdc-de8b-4ace-b159-3b5fd99d9401
        [HttpGet]        
        public void SendMonthlyEmail(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _monthlyEmailService.CreateAndSendMail();
            }
        }
    }
}