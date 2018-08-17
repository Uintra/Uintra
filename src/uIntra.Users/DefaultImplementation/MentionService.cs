using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Compent.CommandBus;
using Uintra.Core.Extensions;
using Uintra.Users.Commands;

namespace Uintra.Users
{
    public class MentionService : IMentionService
    {
        private readonly ICommandPublisher _commandPublisher;

        public MentionService(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        private const string MentionDetectionRegex = "(?<=\\bdata-id=\")[^\"]*";

        public IEnumerable<Guid> GetMentions(string text)
        {
            var mentionIds = new List<Guid>();

            foreach (var match in Regex.Matches(text, MentionDetectionRegex))
            {
                if (Guid.TryParse(match.ToString(), out var id))
                {
                    mentionIds.Add(id);
                }
            }

            return mentionIds;
        }

        public void PreccessMention(MentionModel model)
        {
            var command = model.Map<MentionCommand>();
            _commandPublisher.Publish(command);
        }
    }
}
