using System;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Mention.Models
{
    public class MentionUserModel
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public UintraLinkModel Url { get; set; }
    }
}