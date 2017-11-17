using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uIntra.Core.Core.Extensions
{

    public class Tree<T>
    {
        public Tree(T value, IEnumerable<Tree<T>> children)
        {
            Value = value;
            Children = children;
        }

        public Tree(T value, params Tree<T>[] children) : this(value, children.AsEnumerable())
        {
        }

        public T Value { get; }
        public IEnumerable<Tree<T>> Children { get; }
    }

    public static class TreeExtensions
    {
        public static IEnumerable<Tree<T>> Where<T>(this Tree<T> tree, Func<T, bool> predicate)
        {
            IEnumerable<Tree<T>> WhereRec(Tree<T> tr)
            {
                if (predicate(tr.Value)) yield return tr;

                if (tr.Children.Any())
                {
                    foreach (var child in tr.Children.SelectMany(WhereRec))
                    {
                        yield return child;
                    }
                }
            }

            return WhereRec(tree);
        }

        public static TResult TreeCatamorphism<T, TResult>(this Tree<T> tree, Func<T, TResult> leafFunc, Func<T, IEnumerable<TResult>, TResult> nodeFunc)
        {
            TResult TreeCatamorphismRec(Tree<T> tr)
            {
                if (tr.Children.Any())
                {
                    var mappedChildren = tr.Children.Select(TreeCatamorphismRec);
                    return nodeFunc(tr.Value, mappedChildren);
                }
                else
                {
                    return leafFunc(tr.Value);
                }
            }

            return TreeCatamorphismRec(tree);
        }
    }
}
