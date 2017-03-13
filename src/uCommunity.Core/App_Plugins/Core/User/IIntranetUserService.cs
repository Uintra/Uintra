using System;
using System.Collections.Generic;

namespace uCommunity.Core.User
{
    public interface IIntranetUserService
    {
        Guid GetCurrentUserId();

        IEnumerable<Tuple<Guid, string>> GetManyNames(IEnumerable<Guid> ids);
    }

    public interface IIntranetUserService<TModel> : IIntranetUserService
        where TModel : IntranetUserBase
    {
        TModel Get(int umbracoId);
        TModel GetCurrentUser();
        IEnumerable<TModel> GetMany(IEnumerable<Guid> ids);
        IEnumerable<TModel> GetAll();
        void FillCreator(IHasCreator<TModel> model);
    }
}