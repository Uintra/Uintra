using System;

namespace uIntra.CentralFeed.Exceptions
{
    public class WrongTypeCentralFeedItemException : ApplicationException
    {
        public WrongTypeCentralFeedItemException(Guid id, Enum type)
            :base($"Can not render central feed item, because activity {id} has wrong type: {type.ToString()}")
        {
            
        }
    }
}