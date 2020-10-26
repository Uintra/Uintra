using System.Linq;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Profile.Models;

namespace Uintra20.Core.User
{
    public class IntranetUserContentProvider : IIntranetUserContentProvider
    {
        private readonly INodeModelService _nodeModelService;

        public IntranetUserContentProvider(INodeModelService nodeModelService)
        {
            _nodeModelService = nodeModelService;
        }

        public ProfilePageModel GetProfilePage()
        {
            var profilePage = _nodeModelService.AsEnumerable().OfType<ProfilePageModel>().Single();

            return profilePage;
        }

        public ProfileEditPageModel GetEditPage()
        {
            var profileEditPage = _nodeModelService.AsEnumerable().OfType<ProfileEditPageModel>().Single();

            return profileEditPage;
        }
    }
}