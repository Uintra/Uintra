using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        public static Option<T> Choose<T>(this Option<T> source, Option<T> other) => source.IsSome ? source : other;

        public static Option<T> Choose<T>(this Option<T> source, Func<Option<T>> other) => source.IsSome ? source : other();

        public static TType Cast<TType>(this object value)  => (TType) value;

        public static Expression<Func<T, bool>> AndAlso<T>(params Expression<Func<T, bool>>[] predicates) =>
            predicates.Aggregate(expr((T x) => true), ExpressionExtensions.AndAlso);
    }
}