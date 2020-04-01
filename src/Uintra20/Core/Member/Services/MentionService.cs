using Compent.CommandBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Uintra20.Core.Member.Commands;
using Uintra20.Core.Member.Models;
using Uintra20.Core.User;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Services
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