using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions.Trees;

namespace Uintra.Core.Permissions.Models
{
    public  class ResourceToActionRelation
    {
        public ResourceToActionRelation(Enum resource, IEnumerable<ITree<Enum>> actions)
        {
            Resource = resource;
            Actions = actions;
        }

        public Enum Resource { get; }
        public IEnumerable<ITree<Enum>> Actions { get; }

        public static ResourceToActionRelation Of<TAction>(Enum resource, params ITree<TAction>[] actions) where TAction : struct =>
            new ResourceToActionRelation(resource, actions.Select(a => a.Select(e => (Enum) (object)e)));

        public static ResourceToActionRelation Of(Enum resource, IEnumerable<ITree<Enum>> actions) =>
            new ResourceToActionRelation(resource, actions);
    }
}
