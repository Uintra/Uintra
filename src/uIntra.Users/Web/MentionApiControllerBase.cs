using System.Collections.Generic;
using Umbraco.Web.WebApi;

namespace Uintra.Users.Web
{
    public abstract class MentionApiControllerBase : UmbracoApiController
    {
        public abstract IEnumerable<MentionUserModel> SearchMention(string query);
    }
}
