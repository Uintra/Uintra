using System.Collections.Generic;
using System.Linq;
using uIntra.Core.User;

namespace uIntra.Core.Activity
{
    public class IntranetActivityDetailsHeaderViewModel
    {
        public string Title { get; set; }
        public IActivityType Type { get; set; }
        public IIntranetUser Creator { get; set; }
        public IEnumerable<string> Dates { get; set; }

        public IntranetActivityDetailsHeaderViewModel()
        {
            Dates = Enumerable.Empty<string>();
        }
    }
}