using System;
using uIntra.Core.Activity;

namespace uCommunity.CentralFeed.Exceptions
{
    public class WrongTypeCentralFeedItemException : ApplicationException
    {
        public WrongTypeCentralFeedItemException(Guid id, IntranetActivityTypeEnum type)
            :base($"Can not render central feed item, because activity {id} has wrong type: {type}")
        {
            
        }
    }
}