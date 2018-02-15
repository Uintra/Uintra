using System;
using Uintra.Core.Extensions;

namespace Compent.Uintra.Installer.Migrations
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