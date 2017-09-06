using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupCentralFeedOverviewModel
    {
        public IIntranetType CurrentType { get; set; }

        public Guid GroupId { get; set; }

        public IEnumerable<GroupNavigationCreateTabViewModel> CreateTabs = Enumerable.Empty<GroupNavigationCreateTabViewModel>();

        //public IEnumerable<CentralFeedTabViewModel> ActivityTabs = Enumerable.Empty<CentralFeedTabViewModel>();
    }
}