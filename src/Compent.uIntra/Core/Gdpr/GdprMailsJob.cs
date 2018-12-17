using EmailWorker.Web.Helper.Gdpr;
using Uintra.Core.Jobs.Models;

namespace Compent.Uintra.Core.Gdpr
{
    public class GdprMailsJob : BaseIntranetJob
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