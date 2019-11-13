using System;
using System.Threading.Tasks;

namespace Uintra20.Core.UserTags
{
    public interface IActivityTagsHelper
    {
        void ReplaceTags(Guid entityId, string collectionString);
        Task ReplaceTagsAsync(Guid entityId, string collectionString);
    }
}
