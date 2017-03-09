using System;
using System.Collections.Generic;

namespace uCommunity.Core.App_Plugins.Core.User
{
    public interface IIntranetUserService
    {
        IEnumerable<string> GetFullNamesByIds(IEnumerable<Guid> ids);
        IEnumerable<ListItemModel<Guid>> GetByIds(IEnumerable<Guid> ids);
        Guid GetCurrentUserId();
    }

    public interface IIntranetUserService<out TModel> : IIntranetUserService
        where TModel : IntranetUserBase
    {
        IEnumerable<TModel> GetAll();
        TModel GetActivityUser(int umbracoId);
        TModel GetCurrentUser();
    }
}