using System;
using System.Collections.Generic;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Subscribe
{
    public class CustomSubscribable : ISubscribable
    {
        public Guid Id { get; set; }

        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }

        public Enum Type { get; set; }
    }
}