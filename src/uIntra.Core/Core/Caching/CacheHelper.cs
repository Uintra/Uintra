using System;

namespace uIntra.Core.Caching
{
    public static class CacheHelper
    {
        public static DateTimeOffset GetDateTimeOffsetToMidnight()
        {
            var currentDateTime = DateTimeOffset.Now;
            var midnightTime = currentDateTime.AddDays(1).Date;
            var timeToMidnight = midnightTime - currentDateTime;

            return DateTimeOffset.Now.Add(timeToMidnight);
        }
    }
}
