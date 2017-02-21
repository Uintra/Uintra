using System;
using System.Collections.Generic;

namespace uCommunity.Core.App_Plugins.Core.User
{
    public interface IIntranetUserService<out TModel> where TModel : IntranetUserBase
    {
        IEnumerable<TModel> GetAll();
        TModel GetActivityUser(int umbracoId);
    }
}