using System;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Mention.Models
{
    public class MentionUserModel
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public UintraLinkModel Url { get; set; }
    }
}