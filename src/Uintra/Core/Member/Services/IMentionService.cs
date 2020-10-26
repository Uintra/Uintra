using System;
using System.Collections.Generic;
using Uintra.Core.Member.Models;

namespace Uintra.Core.Member.Services
{
    public interface IMentionService
    {
        IEnumerable<Guid> GetMentions(string text);

        void ProcessMention(MentionModel model);
    }
}