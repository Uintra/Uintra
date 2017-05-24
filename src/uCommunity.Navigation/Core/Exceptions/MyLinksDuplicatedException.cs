using System;

namespace uCommunity.Navigation.Core.Exceptions
{
    public class MyLinksDuplicatedException : ApplicationException
    {
        public MyLinksDuplicatedException(Guid userId, int contentId)
            : base($"Can not add myLink with content {contentId} for {userId}, becase it's already existed")
        {
            
        }
    }
}
