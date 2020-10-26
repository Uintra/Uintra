using Compent.CommandBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Uintra.Core.Member.Commands;
using Uintra.Core.Member.Models;
using Uintra.Core.User;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Member.Services
{
    public class MentionService : IMentionService
    {
        private readonly ICommandPublisher _commandPublisher;
        private const string MentionDetectionRegex = "(?<=\\bdata-id=\")[^\"]*";

        public MentionService(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        public IEnumerable<Guid> GetMentions(string text)
        {
            var matches = Regex.Matches(text, MentionDetectionRegex)
                .Cast<Match>()
                .Select(x => x.Value);
            
            return matches
                .Select(m =>
                {
                    Guid.TryParse(m, out var guid);
                    return guid;
                })
                .Where(g => g != default(Guid));
        }

        public void ProcessMention(MentionModel model)
        {
            var command = model.Map<MentionCommand>();
            _commandPublisher.Publish(command);
        }
    }
}