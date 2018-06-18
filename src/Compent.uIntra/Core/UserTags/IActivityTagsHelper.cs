using System;

namespace Compent.Uintra.Core.UserTags
{
    public interface IActivityTagsHelper
    {
        void ReplaceTags(Guid entityId, string collectionString);
    }
}