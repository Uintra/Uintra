using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace uCommunity.Users.Core
{
    public class IntranetUserService : IIntranetUserService
    {
        protected virtual string MemberTypeAlias => "Member";
        protected virtual string UmbracoUserIdPropertyAlias => "relatedUser";
        private readonly IMemberService _memberService;
        private readonly UmbracoContext _umbracoContext;
        private readonly UmbracoHelper _umbracoHelper;

        public IntranetUserService(IMemberService memberService,
            UmbracoContext umbracoContext, 
            UmbracoHelper umbracoHelper)
        {
            _memberService = memberService;
            _umbracoContext = umbracoContext;
            _umbracoHelper = umbracoHelper;
        }

        public virtual IIntranetUser Get(int umbracoId)
        {
            var member = _memberService.GetMembersByPropertyValue(UmbracoUserIdPropertyAlias, umbracoId).SingleOrDefault();

            if (member != null)
            {
                return Map(member);
            }

            return null;
        }

        public virtual IIntranetUser Get(Guid id)
        {
            var member = _memberService.GetByKey(id);

            if (member != null)
            {
                return Map(member);
            }

            return null;
        }

        public virtual IEnumerable<IIntranetUser> GetAll()
        {
            var members = _memberService.GetAllMembers().Select(Map);
            return members;
        }

        public virtual IEnumerable<IIntranetUser> GetMany(IEnumerable<Guid> ids)
        {
            var members = _memberService.GetAllMembers().Where(s => ids.Contains(s.Key)).Select(Map);
            return members;
        }

        public virtual IEnumerable<IIntranetUser> GetMany(IEnumerable<int> ids)
        {
            var members = _memberService.GetAllMembers().Select(Map);
            return members.Where(s => s.UmbracoId.HasValue && ids.Contains(s.UmbracoId.Value));
        }

        public virtual void FillCreator(IHaveCreator model)
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

        public virtual IIntranetUser GetCurrentUser()
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
            if (string.IsNullOrEmpty(userName))
            {
                var currentPrincipal = Thread.CurrentPrincipal;
                if (currentPrincipal?.Identity != null)
                {
                    userName = currentPrincipal.Identity.Name;
                }
            }
            var user = GetByName(userName);
            return user;
        }

        public virtual IntranetUser Map(IMember member)
        {
            var user = new IntranetUser
            {
                Id = member.Key,
                UmbracoId = member.GetValueOrDefault<int?>(UmbracoUserIdPropertyAlias),
                Email = member.Email,
                FirstName = member.GetValueOrDefault<string>("firstName"),
                LastName = member.GetValueOrDefault<string>("lastName")
            };

            var userPhotoId = member.GetValueOrDefault<int?>("photo");
            if (userPhotoId.HasValue)
            {
                var media = _umbracoHelper.TypedMedia(userPhotoId.Value);
                user.Photo = media.Url;
            }
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
