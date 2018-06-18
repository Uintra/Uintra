using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.SingleLinkedList
{
    public static class SingleLinkedListExtensions
    {
        public static SingleLinkedList<T> ToSingleLinkedList<T>(T item, SingleLinkedList<T> tail = null) =>
            new SingleLinkedList<T>
            {
                Value = item,
                Tail = tail
            };

        public static TResult Catamorphism<T, TResult>(
            this SingleLinkedList<T> list,
            Func<T, TResult, TResult> node,
            Func<TResult> empty)
        {
            TResult CatamorphismRec(SingleLinkedList<T> lst) =>
                lst.Match(
                    list: (head, tail) => node(lst.Value, CatamorphismRec(lst.Tail)),
                    empty: empty);

            return CatamorphismRec(list);
        }

        public static SingleLinkedList<T> Where<T>(this SingleLinkedList<T> source, Func<T, bool> predicate) =>
            source.Catamorphism(
                node: (item, mappedResult) => predicate(item) ? ToSingleLinkedList(item, mappedResult) : mappedResult,
                empty: Empty<T>);

        public static SingleLinkedList<TResult> Select<T, TResult>(this SingleLinkedList<T> source, Func<T, TResult> func) =>
            source.Catamorphism(
                node: (item, mappedResult) => ToSingleLinkedList(func(item), mappedResult),
                empty: Empty<TResult>);

        public static TResult Match<T, TResult>(
            this SingleLinkedList<T> source,
            Func<T, SingleLinkedList<T>, TResult> list,
            Func<TResult> empty) =>
            source is null
                ? empty()
                : list(source.Value, source.Tail);


        public static SingleLinkedList<T> TakeWhile<T>(this SingleLinkedList<T> source, Func<T, bool> predicate)
        {
            SingleLinkedList<T> TakeWhileRec(SingleLinkedList<T> src) =>
                src.Match(
                    list: (head, tail) => predicate(head) ? ToSingleLinkedList(head, TakeWhileRec(tail)) : TakeWhileRec(tail),
                    empty: Empty<T>);

            return TakeWhileRec(source);
        }


        public static SingleLinkedList<T> SkipWhile<T>(this SingleLinkedList<T> source, Func<T, bool> predicate)
        {
            SingleLinkedList<T> SkipWhileRec(SingleLinkedList<T> src) =>
                src.Match(
                    list: (head, tail) => predicate(head) ? SkipWhileRec(tail) : ToSingleLinkedList(head, SkipWhileRec(tail)),
                    empty: Empty<T>);

            return SkipWhileRec(source);
        }

        public static (SingleLinkedList<T> result, SingleLinkedList<T> rest) Span<T>(this SingleLinkedList<T> source, Func<T, bool> predicate) =>
            (source.TakeWhile(predicate), source.SkipWhile(predicate));


        public static SingleLinkedList<T> ToSingleLinkedList<T>(this IEnumerable<T> source) =>
            source
                .Reverse()
                .Aggregate(Empty<T>(), (tail, item) => ToSingleLinkedList(item, tail));


        public static bool Any<T>(this IEnumerable<T> source) => source != null;

        public static SingleLinkedList<T> Empty<T>() => null;
    }
}