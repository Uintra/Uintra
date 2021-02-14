using System;
using Uintra.Core.Activity.Entities;

namespace Uintra.Features.News.Extensions
{
    public static class NewsExtensions
    {
        public  static bool IsInCache(this Entities.News news)
        {
            return !IsNewsHidden(news) && IsActualPublishDate(news);
        }

        public  static bool IsNewsHidden(this IIntranetActivity news) =>
            news == null || news.IsHidden;

        public  static bool IsActualPublishDate(this INewsBase news) =>
            DateTime.Compare(news.PublishDate, DateTime.UtcNow) <= 0;

    }
}