using System;
using System.Linq;
using System.Web.Http;
using EmailWorker.Data.Features.Gdpr;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.MonthlyMail;
using Uintra20.Features.Reminder.Services;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Controllers
{
    [RoutePrefix("api/qa")]
    public class QaController : ApiController
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IMonthlyEmailService _monthlyEmailService;
        private readonly IReminderRunner _reminderJob;
        private readonly IEmailGdprService _emailGdprService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;

        public QaController(IApplicationSettings applicationSettings, IMonthlyEmailService monthlyEmailService, IReminderRunner reminderJob, IEmailGdprService emailGdprService,
            IIntranetMemberService<IntranetMember> intranetMemberService, IGroupService groupService, IGroupMemberService groupMemberService)
        {
            _applicationSettings = applicationSettings;
            _monthlyEmailService = monthlyEmailService;
            _reminderJob = reminderJob;
            _emailGdprService = emailGdprService;
            _intranetMemberService = intranetMemberService;
            _groupService = groupService;
            _groupMemberService = groupMemberService;
        }

        [HttpGet]
        [Route("monthly-email")]
        public void SendMonthlyEmail(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _monthlyEmailService.CreateAndSendMail();
            }
        }

        [Route("reminder")]
        [HttpGet]
        public void RunRemainder(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _reminderJob.Run();
            }
        }

        [HttpGet]
        [Route("gdpr")]
        public void Gdpr(Guid qaKey)
        {
            if (qaKey == _applicationSettings.QaKey)
            {
                _emailGdprService.SupportGdpr();
            }
        }

        [HttpGet]
        [Route("assign-members")]
        public void AssignMembersToGroup()
        {
            var members = _intranetMemberService.GetAll().Where(s => !s.Inactive).Take(50);
            var creator = _intranetMemberService.GetByEmail("admin@testmember.com");

            _groupMemberService.Create(new GroupCreateModel()
            {
                Description = "Automation created",
                Title = $"{DateTime.Now.Ticks}",
            }, new GroupMemberSubscriptionModel()
            {
                IsAdmin = true,
                MemberId = creator.Id
            });

            var groupId = _groupService.GetAll().OrderByDescending(g => g.CreatedDate).First().Id;

            var subscriptions = members.Except(creator.ToEnumerableOfOne()).Select(m =>
                new GroupMemberSubscriptionModel()
                {
                    MemberId = m.Id,
                    IsAdmin = false,
                });

            _groupMemberService.AddMany(groupId, subscriptions);
        }



    }
}