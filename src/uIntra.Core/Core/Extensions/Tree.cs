using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Extensions
{
    public class Tree<T> 
    {
        public T Value { get; }
        public IEnumerable<Tree<T>> Children { get; }

        public Tree(T value, params Tree<T>[] children) : this(value, children.AsEnumerable())
        {
        }

        public Tree(T value, IEnumerable<Tree<T>> children)
        {
            Value = value;
            Children = children;
        }
    }
}