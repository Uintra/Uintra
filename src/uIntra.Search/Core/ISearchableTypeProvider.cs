using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.Search
{
    public interface ISearchableTypeProvider
    {
        IActivityType Get(int id);
        IEnumerable<IActivityType> GetAll();
    }
}
