using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public class GroupFeedOverviewModel
    {
        public IIntranetType CurrentType { get; set; }

        public Guid GroupId { get; set; }

        public IEnumerable<GroupNavigationCreateTabViewModel> CreateTabs = Enumerable.Empty<GroupNavigationCreateTabViewModel>();

        public IEnumerable<CentralFeedTabViewModel> ActivityTabs = Enumerable.Empty<CentralFeedTabViewModel>();
    }
}