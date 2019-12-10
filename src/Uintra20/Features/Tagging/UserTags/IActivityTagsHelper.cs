using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Features.Tagging.UserTags
{
    public interface IActivityTagsHelper
    {
        void ReplaceTags(Guid entityId, string collectionString);
        void ReplaceTags(Guid entityId, IEnumerable<Guid> collection);
        Task ReplaceTagsAsync(Guid entityId, string collectionString);
    }
}
