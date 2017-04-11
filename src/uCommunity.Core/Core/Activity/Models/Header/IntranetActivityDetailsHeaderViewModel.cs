using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.User;

namespace uCommunity.Core.Activity.Models
{
    public class IntranetActivityDetailsHeaderViewModel
    {
        public string Title { get; set; }
        public IntranetActivityTypeEnum Type { get; set; }
        public IIntranetUser Creator { get; set; }
        public IEnumerable<string> Dates { get; set; }

        public IntranetActivityDetailsHeaderViewModel()
        {
            Dates = Enumerable.Empty<string>();
        }
    }
}