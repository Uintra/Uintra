using System;
using Uintra.Core.Extensions;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.OldNotifications
{
    internal static class Helpers
    {
        private static int _guidLength = Guid.Empty.ToString().Length;

        internal static Guid ParseIdFromQueryString(this string url, string afterSubstring)
        {
            var id = url.SubstringAfter(afterSubstring).Take(_guidLength);
            return Guid.Parse(id);
        }
    }
}