using EmailWorker.Data.Extensions;
using EmailWorker.Web.Helper.Gdpr;
using System;
using System.Linq;
using System.Web.Http;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Notification;
using Uintra.Notification.Jobs;
using Umbraco.Web.WebApi;

namespace Compent.Uintra.Controllers.Api
{
    public class QaController : UmbracoApiController
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IEmailBroadcastService<MonthlyMailBroadcast> _monthlyEmailBroadcastService;
        private readonly IEmailBroadcastService<WeeklyMailBroadcast> _weeklyEmailBroadcastService;
        private readonly IReminderJob _reminderJob;
        private readonly IEmailGdprService _emailGdprService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IGroupService _groupService;
        private readonly IGroupMemberService _groupMemberService;

        public QaController(
            IApplicationSettings applicationSettings,
            IEmailBroadcastService<MonthlyMailBroadcast> monthlyEmailBroadcastService,
            IReminderJob reminderJob,
            IEmailGdprService emailGdprService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IGroupService groupService,
            IGroupMemberService groupMemberService,
            IEmailBroadcastService<WeeklyMailBroadcast> weeklyEmailBroadcastService)
        {
            _applicationSettings = applicationSettings;
            _monthlyEmailBroadcastService = monthlyEmailBroadcastService;
            _reminderJob = reminderJob;
            _emailGdprService = emailGdprService;
            _intranetMemberService = intranetMemberService;
            _groupService = groupService;
            _groupMemberService = groupMemberService;
            _weeklyEmailBroadcastService = weeklyEmailBroadcastService;
        }

        [HttpGet]
        public void SendMonthlyEmail()
        {
            _monthlyEmailBroadcastService.Broadcast();
        }

        [HttpGet]
        public void SendWeeklyEmail()
        {
            _weeklyEmailBroadcastService.IsBroadcastable();
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

        [HttpGet]
        public void AssignMembersToGroup()
        {
            var creator = _intranetMemberService.GetByEmail("admin@testmember.com");

            _groupMemberService.Create(new GroupCreateModel()
            {
                Creator = new GroupMemberSubscriptionModel()
                {
                    IsAdmin = true,
                    MemberId = creator.Id
                },
                Description = "Automation created",
                Title = $"{DateTime.Now.Ticks}",
            });

            var groupId = _groupService.GetAll().OrderByDescending(g => g.CreatedDate).First().Id;

            var members = _intranetMemberService.GetAll().Where(s => !s.Inactive).Take(50);

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