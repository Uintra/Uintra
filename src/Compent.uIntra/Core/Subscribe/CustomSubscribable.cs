using System;
using System.Collections.Generic;
using Uintra.Subscribe;

namespace Compent.Uintra.Core.Subscribe
{
    public class CustomSubscribable : ISubscribable
    {
        public Guid Id { get; set; }

        public IEnumerable<global::Uintra.Subscribe.Subscribe> Subscribers { get; set; }

        public Enum Type { get; set; }
    }
}