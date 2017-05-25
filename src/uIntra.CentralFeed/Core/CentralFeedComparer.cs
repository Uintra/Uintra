using System;
using System.Collections.Generic;

namespace uCommunity.CentralFeed.Core
{
    public class CentralFeedComparer : IComparer<ICentralFeedItem>
    {
        private readonly DateTime _currentDate;

        public CentralFeedComparer()
        {
            _currentDate = DateTime.Now.Date.AddHours(8);
        }

        public int Compare(ICentralFeedItem x, ICentralFeedItem y)
        {
            if (IsFuture(x) && IsFuture(y))
            {
                return DateTime.Compare(x.PublishDate, y.PublishDate);
            }

            if (IsFuture(x))
            {
                return -1;
            }

            if (IsFuture(y))
            {
                return 1;
            }

            return DateTime.Compare(y.PublishDate, x.PublishDate);
        }

        private bool IsFuture(ICentralFeedItem item)
        {
            return item.PublishDate.Date >= _currentDate.Date;
        }
    }
}
