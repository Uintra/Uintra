using System;
using System.Collections.Generic;

namespace uIntra.CentralFeed
{
    public class CentralFeedEventComparer : IComparer<IFeedItem>
    {
        private readonly DateTime _currentDate;

        public CentralFeedEventComparer()
        {
            _currentDate = DateTime.Now.Date.AddHours(8);
        }

        public int Compare(IFeedItem x, IFeedItem y)
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

        private bool IsFuture(IFeedItem item)
        {
            return item.PublishDate.Date >= _currentDate.Date;
        }
    }
}
