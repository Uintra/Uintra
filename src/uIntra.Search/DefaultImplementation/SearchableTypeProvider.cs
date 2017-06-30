using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;

namespace uIntra.Search
{
    public class SearchableTypeProvider : ISearchableTypeProvider
    {
        public virtual IActivityType Get(int id)
        {
            return GetAll().SingleOrDefault(at => at.Id == id);
        }

        public virtual IEnumerable<IActivityType> GetAll()
        {
            foreach (var enumRole in Enum.GetValues(typeof(SearchableTypeEnum)))
            {
                yield return new ActivityType
                {
                    Id = enumRole.GetHashCode(),
                    Name = enumRole.ToString()
                };
            }
        }
    }
}
