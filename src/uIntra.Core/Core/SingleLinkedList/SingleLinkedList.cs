using System.Collections;
using System.Collections.Generic;

namespace Uintra.Core.SingleLinkedList
{
    public class SingleLinkedList<T> : IEnumerable<T>
    {
        public T Value { get; set; }
        public SingleLinkedList<T> Tail { get; set; }

        public IEnumerator<T> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> Enumerate()
        {
            var list = this;

            while (list != null)
            {
                yield return list.Value;
                list = list.Tail;
            }
        }
    }
}