using System;

namespace Compent.uIntra.Core.UserTags
{
    public interface IActivityTagsHelper
    {
        void ReplaceTags(Guid entityId, string collectionString);
    }
}