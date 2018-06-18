using System;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed.Exceptions
{
    public class WrongTypeCentralFeedItemException : ApplicationException
    {
        public WrongTypeCentralFeedItemException(Guid id, IIntranetType type)
            :base($"Can not render central feed item, because activity {id} has wrong type: {type.Name}")
        {
            
        }
    }
}