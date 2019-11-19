using System;
using System.Collections.Generic;
using Uintra20.Features.User.Models;

namespace Uintra20.Features.User
{
    public interface IMentionService
    {
        IEnumerable<Guid> GetMentions(string text);

        void ProcessMention(MentionModel model);
    }
}