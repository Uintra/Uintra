using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Compent.Extensions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Uintra.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int divider = 2)
        {
            return source
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / divider)
                .Select(x => x.Select(v => v.Value));
        }

        public static bool IsEmpty<T>(this IList<T> list) => list.Count == 0;

        public static IEnumerable<TResult> TryCast<TResult>(this IEnumerable source)
        {
            foreach (var item in source)
            {
                if (item is TResult castedItem)
                {
                    yield return castedItem;
                }
            }
        }

        public static IEnumerable<T> WithUpdatedElement<T, TIdentity>(
            this IEnumerable<T> source,
            Func<T, TIdentity> identitySelector,
            T element)
        {
            var elementIdentity = identitySelector(element);

            return source.Where(e => !Equals(identitySelector(e), elementIdentity))
                .Append(element).ToList();
        }

        public static Option<T> Choose<T>(this Option<T> source, Option<T> other) => source.IsSome ? source : other;

        public static Option<T> Choose<T>(this Option<T> source, Func<Option<T>> other) => source.IsSome ? source : other();

        public static TType Cast<TType>(this object value)  => (TType) value;

        public static Expression<Func<T, bool>> AndAlso<T>(params Expression<Func<T, bool>>[] predicates) =>
            predicates.Aggregate(expr((T _) => true), ExpressionExtensions.AndAlso);

        public static Option<TValue> ItemOrNone<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.ContainsKey(key) ? Some(dictionary[key]) : None;

        public static IEnumerable<(T1, T2)> CartesianProduct<T1, T2>(this IEnumerable<T1> source1, IEnumerable<T2> source2) =>
            source1.SelectMany(x => source2.Select(y => (x, y)));
    }
}