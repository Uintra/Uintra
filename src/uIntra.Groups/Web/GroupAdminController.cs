using System.Web.Mvc;
using Uintra.Groups.Sql;
using Umbraco.Web.Mvc;

namespace Uintra.Groups.Web
{
    public class GroupAdminController : SurfaceController
    {
        private IGroupMemberService _groupMemberService;

        public GroupAdminController(IGroupMemberService groupMemberService) => 
            _groupMemberService = groupMemberService;

        [HttpPut]
        public ActionResult Assign(GroupMemberSubscriptionModel subscription)
        {
            if (!ModelState.IsValid) return RedirectToCurrentUmbracoPage(Request.QueryString);
            var groupMember = _groupMemberService.Get(subscription.MemberId);

            if (groupMember.IsAdmin)
            {
                OnAssign(groupMember);
            }
            else
            {
                OnAssign(groupMember);
            }

            return RedirectToCurrentUmbracoPage(Request.QueryString);
        }

        private void OnAssign(GroupMember groupMember)
        {
            groupMember.IsAdmin = !groupMember.IsAdmin;
            _groupMemberService.Update(groupMember);
        }
    }
}