using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using uCommunity.Core.User;
using uCommunity.Users.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uCommunity.Users.DefaultImplementation
{
    public class IntranetUserService : IIntranetUserService
    {
        private const string UmbracoUserIdPropertyAlias = "umbracoUserId";
        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;

        public IntranetUserService(IMemberService memberService,
            UmbracoContext umbracoContext)
        {
            _memberService = memberService;
            _umbracoContext = umbracoContext;
        }

        public IIntranetUser Get(int umbracoId)
        {
            var member = _memberService.GetMembersByPropertyValue(UmbracoUserIdPropertyAlias, umbracoId).SingleOrDefault();

            if (member != null)
            {
                return Map(member);
            }

            return null;
        }

        public IIntranetUser Get(Guid id)
        {
            var member = _memberService.GetByKey(id);

            if (member != null)
            {
                return Map(member);
            }

            return null;
        }

        public IEnumerable<IIntranetUser> GetAll()
        {
            var members = _memberService.GetAllMembers().Select(Map);
            return members;
        }

        public IEnumerable<IIntranetUser> GetMany(IEnumerable<Guid> ids)
        {
            var members = _memberService.GetAllMembers().Where(s => ids.Contains(s.Key)).Select(Map);
            return members;
        }

        public IEnumerable<IIntranetUser> GetMany(IEnumerable<int> ids)
        {
            var members = _memberService.GetAllMembers().Select(Map);
            return members.Where(s => s.UmbracoId.HasValue && ids.Contains(s.UmbracoId.Value));
        }

        public void FillCreator(IHaveCreator model)
        {
            IIntranetUser member;

            if (model.UmbracoCreatorId.HasValue)
            {
                member = Get(model.UmbracoCreatorId.Value);
            }
            else
            {
                member = Get(model.CreatorId);
            }
            model.Creator = member;
        }

        public IIntranetUser GetCurrentUser()
        {
            var userName = "";
            if (HostingEnvironment.IsHosted) //TODO: WTF IS THIS
            {
                var httpContext = _umbracoContext.HttpContext;
                if (httpContext.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    userName = httpContext.User.Identity.Name;
                }
            }

            var currentPrincipal = Thread.CurrentPrincipal;
            if (currentPrincipal?.Identity != null)
            {
                userName = currentPrincipal.Identity.Name;
            }
            var user = GetByName(userName);
            return user;
        }

        public static IntranetUser Map(IMember member)
        {
            var user = new IntranetUser
            {
                Id = member.Key,
                UmbracoId = member.GetValue<int?>(UmbracoUserIdPropertyAlias),
                DisplayedName = member.Name,
                Email = member.GetValue<string>("email"),
                FirstName = member.GetValue<string>("firstName"),
                LastName = member.GetValue<string>("lastName"),
                Photo = member.GetValue<string>("photo")
            };
            return user;
        }

        private IntranetUser GetByName(string name)
        {
            var user = _memberService.GetByUsername(name);

            if (user == null)
            {
                return null;
            }

            return Map(user);
        }
    }
}
