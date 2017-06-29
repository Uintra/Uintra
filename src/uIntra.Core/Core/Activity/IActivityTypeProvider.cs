using System.Collections.Generic;
namespace uIntra.Core.Activity
{
    public interface IActivityTypeProvider
    {
        IActivityType Get(int id);
        IEnumerable<IActivityType> GetAll();
    }
}
