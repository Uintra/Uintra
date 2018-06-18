using System;
using Uintra.Core.Links;

namespace Uintra.CentralFeed
{
    public class EditViewModel
    {
        public Guid Id { get; set; }
        public IActivityLinks Links { get; set; }
        public FeedSettings Settings { get; set; }
    }
}