using EmailWorker.Data.Features.Gdpr;
using Uintra.Core.Jobs.Models;

namespace Uintra.Features.Jobs
{
    public class GdprMailsJob : UintraBaseIntranetJob
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