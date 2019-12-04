using System;
using System.Collections.Generic;
using Uintra20.Core.Member.Models;

namespace Uintra20.Core.Member.Services
{
    public interface IMentionService
    {
        IEnumerable<Guid> GetMentions(string text);

        void ProcessMention(MentionModel model);
    }
}