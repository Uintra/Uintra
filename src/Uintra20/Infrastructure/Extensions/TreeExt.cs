using Compent.Extensions.Trees;
using System;
using System.Collections.Generic;
using System.Linq;
using static Compent.Extensions.Trees.TreeExtensions;

namespace Uintra20.Infrastructure.Extensions
{
    public static class TreeExt
    {
        public static ITree<(T current, T parent)> WithAttachedParents<T>(this ITree<T> tree)
        {
            ITree<(T current, T parent)> SetNodeAsParent(T current, IEnumerable<ITree<(T current, T parent)>> mappedChildren)
            {
                var childrenWithUpdatedParent = mappedChildren
                    .Select(child => Tree(
                        new ValueTuple<T, T>(child.Value.current, current),
                        child.GetChildren()));

                var mappedCurrent = (current, default(T));

                return Tree(mappedCurrent, childrenWithUpdatedParent);
            }

            ITree<(T current, T parent)> SetEmptyParent(T current) => Tree((current, default(T)));

            return tree.Catamorphism(SetNodeAsParent, SetEmptyParent);
        }
    }
}