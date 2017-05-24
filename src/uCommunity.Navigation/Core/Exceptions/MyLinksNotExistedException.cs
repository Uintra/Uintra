using System;

namespace uCommunity.Navigation.Core.Exceptions
{
    public class MyLinksNotExistedException : ApplicationException
    {
        public MyLinksNotExistedException(Guid userId, Guid id)
            : base($"Can not delete myLink {id} for {userId}, becase it's not existed")
        {

        }
    }
}
