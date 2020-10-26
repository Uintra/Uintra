using System;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Core.Search.Indexers
{
    public interface IUserTagsSearchIndexer
    {
        void FillIndex(UserTag userTag);

        void Delete(Guid id);
    }
}
