using System.Collections.Generic;
using uCommunity.Core.User;

namespace uCommunity.Core.Activity.Models
{
    public class IntranetActivityHeaderBase
    {
        public IntranetActivityHeaderBase()
        {
            Dates = new List<string>();
        }

        public string Title { get; set; }
        public IntranetActivityTypeEnum Type { get; set; }
        public IIntranetUser Creator { get; set; }
        public IEnumerable<string> Dates { get; set; }
    }
}