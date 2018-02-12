using System;

namespace Uintra.Navigation.Exceptions
{
    public class MyLinksNotExistedException : ApplicationException
    {
        public MyLinksNotExistedException(MyLinkDTO model)
            : base($"Can not delete myLink with content {model.ContentId} for {model.UserId} and querString {model.QueryString}, becase it's not existed")
        {

        }
    }
}
