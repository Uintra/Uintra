using System.Collections.Generic;
using System.Web.Http;

namespace Uintra.Users.Web
{
    public abstract class MentionApiControllerBase : ApiController
    {
        public abstract IEnumerable<MentionUserModel> SearchMention(string query);
    }
}
