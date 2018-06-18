using System;

namespace uIntra.Core.Caching
{
    public static class CacheHelper
    {
        public static DateTimeOffset GetMidnightUtcDateTimeOffset()
        {
            var midnightLocalTime = DateTimeOffset.UtcNow.AddDays(1).Date;
            var midnightUtcTime = DateTime.SpecifyKind(midnightLocalTime, DateTimeKind.Utc);

            return midnightUtcTime;
        }
    }
}
