using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Search;
using Uintra.Users;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MentionApiController : MentionApiControllerBase
    {
        private readonly IElasticIndex _elasticIndex;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public MentionApiController(IElasticIndex elasticIndex, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _elasticIndex = elasticIndex;
            _intranetUserService = intranetUserService;
        }

        [HttpGet]
        public override IEnumerable<MentionUserModel> SearchMention(string query)
        {
            var searchResult = _elasticIndex.Search(new SearchTextQuery
            {
                Text = query,
                Take = 5,
                SearchableTypeIds = UintraSearchableTypeEnum.User.ToInt().ToListOfOne(),
                OnlyPinned = false,
                ApplyHighlights = false
            });

            //TODO replace with elastic search
            var mentionUsers = _intranetUserService.GetAll()
                .Where(u => u.DisplayedName.StartsWith(query))
                .Take(6)
                .Map<IEnumerable<MentionUserModel>>();

            return mentionUsers;
        }
    }
}