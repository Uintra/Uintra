using System;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Core.Search.Indexers
{
    public interface IUserTagsSearchIndexer
    {
        void FillIndex(UserTag userTag);

        void Delete(Guid id);
    }
}
