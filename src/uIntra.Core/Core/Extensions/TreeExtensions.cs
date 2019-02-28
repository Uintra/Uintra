using System.Collections.Generic;
using System.Linq;
using Compent.Extensions.Trees;
using LanguageExt;
using static Compent.Extensions.Trees.TreeExtensions;

namespace Uintra.Core.Extensions
{
    public static  class TreeExt
    {
        public static  ITree<(T current, Option<T> parent)> WithAttachedParents<T> (this ITree<T> tree)
        {
            ITree<(T current, Option<T> parent)> SetNodeAsParent(T current, IEnumerable<ITree<(T current, Option<T> parent)>> mappedChildren)
            {
                var childrenWithUpdatedParent = mappedChildren
                    .Select(child => Tree(
                        child.Value.MapSecond(_ => current),
                        child.GetChildren()));

                var mappedCurrent = (current, Option<T>.None);

                return Tree(mappedCurrent, childrenWithUpdatedParent);
            }

            ITree<(T current, Option<T> parent)> SetEmptyParent(T current) => Tree((current, Option<T>.None));

            return tree.Catamorphism(SetNodeAsParent, SetEmptyParent);
        }
    }
}
