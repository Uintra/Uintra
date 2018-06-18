using System.Linq;
using System.Net;
using System.Web;
using umbraco.interfaces;
using Umbraco.Web;

namespace Uintra.Core.Handlers
{
    public abstract class NotFoundHandlerBase : INotFoundHandler
    {
        protected virtual string ErrorPageDocType { get; } = "errorPage";
        private int _redirectId;

        public bool CacheUrl => false;

        public int redirectID => _redirectId;

        public bool Execute(string url)
        {
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var errorpage = helper.TypedContentAtRoot().Where(x => x.GetCulture().Equals(UmbracoContext.Current.PublishedContentRequest.Culture))
                .DescendantsOrSelf(ErrorPageDocType)
                .FirstOrDefault();

            if (errorpage == null)
            {
                return false;
            }

            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.NotFound;
            _redirectId = errorpage.Id;
            return true;
        }
    }
}
