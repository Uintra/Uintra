using System;
using System.Collections.Generic;

namespace Uintra.Users
{
    public interface IMentionService
    {        
        IEnumerable<Guid> GetMentions(string text);

        void ProcessMention(MentionModel model);
    }
}
