using System.Collections.Generic;
using System.Web.Mvc;
using Compent.uIntra.Core.Extentions;
using uIntra.Core;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Web;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class GroupController : GroupControllerBase
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public GroupController(IGroupService groupService, 
            IGroupMemberService groupMemberService, 
            IMediaHelper mediaHelper,
            IGroupLinkProvider groupLinkProvider, 
            IUserService userService, 
            IGroupMediaService groupMediaService, 
            IIntranetUserService<IGroupMember> intranetUserService, IProfileLinkProvider profileLinkProvider, UmbracoHelper umbracoHelper, IDocumentTypeAliasProvider documentTypeAliasProvider) 
            : base(groupService, groupMemberService, mediaHelper, groupMediaService, intranetUserService, profileLinkProvider, groupLinkProvider)
        {
            _umbracoHelper = umbracoHelper;
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public override ActionResult LeftNavigation()
        {
            var result = new List<GroupLeftNavigationItemViewModel>();
            var xpath = XPathHelper.GetXpath(_documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetGroupOverviewPage());
            var groupPage = _umbracoHelper.TypedContentSingleAtXPath(xpath);
            var children = groupPage.Children;

            foreach (var child in children)
            {
                if (child.IsShowPageInSubNavigation())
                {
                    result.Add(new GroupLeftNavigationItemViewModel()
                    {
                        Name = child.GetNavigationName(),
                        Url = child.Url
                    });
                }
            }

            return PartialView(LeftNavigationPath, result);
        }
    }
}