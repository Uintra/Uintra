using System;
using Uintra.Tagging.UserTags.Models;

namespace Uintra.Tagging.UserTags
{
    public interface IUserTagsSearchIndexer
    {
        void FillIndex(UserTag userTag);

        void Delete(Guid id);
    }
}
