using EmailWorker.Data.Features.Gdpr;
using Uintra20.Core.Jobs.Models;

namespace Uintra20.Features.Jobs
{
    public class GdprMailsJob : Uintra20BaseIntranetJob
    {
        private readonly IEmailGdprService _emailGdprService;

        public GdprMailsJob(IEmailGdprService  emailGdprService)
        {
            _emailGdprService = emailGdprService;
        }

        public override void Action()
        {
            _emailGdprService.SupportGdpr();
        }
    }
}