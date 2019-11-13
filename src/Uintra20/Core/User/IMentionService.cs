using System;
using System.Collections.Generic;

namespace Uintra20.Core.User
{
    public interface IMentionService
    {
        IEnumerable<Guid> GetMentions(string text);

        void ProcessMention(MentionModel model);
    }
}