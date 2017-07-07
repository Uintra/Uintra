using System.Collections.Generic;
using Umbraco.Web;

namespace Compent.uIntra.Core.Comments
{
    public class CommentsPageHelper : ICommentsPageHelper
    {
        private readonly UmbracoHelper umbracoHelper;

        public CommentsPageHelper(UmbracoHelper umbracoHelper)
        {
            this.umbracoHelper = umbracoHelper;
        }

        public IEnumerable<CommentsPageTab> GetCommentsPageTab()
        {
            var result = new List<CommentsPageTab>();

            var homePage = umbracoHelper.AssignedContentItem.AncestorOrSelf("homePage");

            var previewPage = homePage.DescendantOrSelf("commentsPage");
            result.Add(new CommentsPageTab { Title = "Preview", Url = previewPage.Url });

            var editPage = homePage.DescendantOrSelf("commentsEditPage");
            result.Add(new CommentsPageTab { Title = editPage.Name, Url = editPage.Url });

            return result;
        }
    }
}