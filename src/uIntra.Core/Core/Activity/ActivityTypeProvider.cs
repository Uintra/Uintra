using System;
using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Activity
{
    public class ActivityTypeProvider : IActivityTypeProvider
    {
        public virtual IActivityType Get(int id)
        {
            // TODO: Use .Cast?
            return GetAll().SingleOrDefault(at => at.Id == id);
        }

        public virtual IEnumerable<IActivityType> GetAll()
        {
            foreach (var enumRole in Enum.GetValues(typeof(IntranetActivityTypeEnum)))
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
